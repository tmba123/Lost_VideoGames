using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class MovementCreateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";
        public string movementtype = ""; //

        public List<Game> GamesEnabled = new List<Game>();
        public List<Warehouse> WarehousesEnabled = new List<Warehouse>();
        public List<Inventory> InventorySearch = new List<Inventory>();
        public Inventory Inventory = new Inventory();

        public IEnumerable<Inventory> Inventories { get; set; }


        public void OnGet()
        {


        }
        public void OnPost()
        {
            LostGamesContext context = new LostGamesContext();

            if (Request.Form["movement_type"] == "add_stock")
            {
                movementtype = "add_stock";
                GamesEnabled = context.SearchGames("g.state", "enabled");
                WarehousesEnabled = context.SearchWarehouses("state", "enabled");
            }

            if (Request.Form["movement_type"] == "remove_stock")
            {
                movementtype = "remove_stock";
                InventorySearch = context.GetInventoryNoTransit();
            }

            if (Request.Form["movement_type"] == "in_transit")
            {
                movementtype = "in_transit";
                WarehousesEnabled = context.SearchWarehouses("state", "enabled");
                InventorySearch = context.SearchInventory("", "1");
            }


        }
        public void OnPostAdd()
        {
            LostGamesContext context = new LostGamesContext();


            if (
                Request.Form["movement_type"] == "" ||
                Request.Form["selectgame"] == "" ||
                Request.Form["selectwarehouse"] == "" ||
                Request.Form["quantity"] == ""
            )

            {

                errorMessage = "All fields are required";
                OnGet();
                return;

            }

            try
            {

                Movement movement = new Movement();

                movement.id_game = Int32.Parse(Request.Form["selectgame"]);
                movement.id_warehouse = Int32.Parse(Request.Form["selectwarehouse"]);
                movement.movement_type = Request.Form["movement_type"];
                movement.quantity = Int32.Parse(Request.Form["quantity"]);



                InventorySearch = context.SearchInventory(movement.id_game.ToString(), movement.id_warehouse.ToString());

                if (InventorySearch.Count() == 0)
                {

                    Inventory inventory = new Inventory();
                    inventory.id_game = Int32.Parse(Request.Form["selectgame"]);
                    inventory.id_warehouse = Int32.Parse(Request.Form["selectwarehouse"]);
                    inventory.quantity = Int32.Parse(Request.Form["quantity"]);


                    context.CreateMovement(movement);
                    context.CreateInventory(inventory);


                }
                else
                {

                    Inventory inventory = InventorySearch.First();
                    inventory.quantity += Int32.Parse(Request.Form["quantity"]);


                    context.CreateMovement(movement);
                    context.UpdateInventory(inventory);

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Stock Added successfully";
            OnGet();
        }

        public void OnPostRemove(string id_game, string id_warehouse, string count)
        {
            LostGamesContext context = new LostGamesContext();

            InventorySearch = context.SearchInventory(id_game, id_warehouse);

            Inventory inventory = InventorySearch.First();

            if (
               Request.Form["qremove" + count] == "" ||
               Request.Form["reason" + count] == ""
               )

            {
                errorMessage = "All fields are required";
                OnGet();
                return;
            }
            if (Int32.Parse(Request.Form["qremove" + count]) > inventory.quantity)

            {
                errorMessage = "Inserted quantity superior to quantity available!";
                OnGet();
                return;

            }


            try
            {

                Movement movement = new Movement();

                movement.id_game = Int32.Parse(id_game);
                movement.id_warehouse = Int32.Parse(id_warehouse);
                movement.movement_type = Request.Form["reason" + count];
                movement.quantity = Int32.Parse("-" + Request.Form["qremove" + count]);

                inventory.quantity = inventory.quantity - Int32.Parse(Request.Form["qremove" + count]);


                if (movement.movement_type == "in_transit")
                {
                    context.CreateMovement(movement);

                    movement.id_warehouse = 1;
                    movement.quantity = Int32.Parse(Request.Form["qremove" + count]);

                    context.CreateMovement(movement);

                    if (inventory.quantity == 0)
                    {
                        context.DeleteInventory(inventory);

                    }
                    else
                    {
                        context.UpdateInventory(inventory);
                    }

                    inventory.id_warehouse = 1;
                    inventory.quantity = Int32.Parse(Request.Form["qremove" + count]);

                    InventorySearch = context.SearchInventory(inventory.id_game.ToString(), inventory.id_warehouse.ToString());


                    if (InventorySearch.Count() == 0)
                    {
                        context.CreateInventory(inventory);
                    }
                    else
                    {
                        context.UpdateInventory(inventory);
                    }




                }
                else if (movement.movement_type == "remove_stock" || movement.movement_type == "stock_reconciliation")
                {


                    if (inventory.quantity == 0)
                    {
                        context.CreateMovement(movement);
                        context.DeleteInventory(inventory);

                    }
                    else
                    {
                        context.CreateMovement(movement);
                        context.UpdateInventory(inventory);
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Movement operation performed successfully!";
            OnGet();

        }
        public void OnPostTransit(string id_game, string id_warehouse, string count)
        {
            LostGamesContext context = new LostGamesContext();

            InventorySearch = context.SearchInventory(id_game, id_warehouse);

            Inventory inventory = InventorySearch.First();

            if (
               Request.Form["qremove" + count] == "" ||
               Request.Form["selectwarehouse" + count] == ""
               )

            {
                errorMessage = "All fields are required";
                OnGet();
                return;
            }
            if (Int32.Parse(Request.Form["qremove" + count]) > inventory.quantity)

            {
                errorMessage = "Inserted quantity superior to quantity available!";
                OnGet();
                return;

            }



            try
            {

                Movement movement = new Movement();





                movement.id_game = Int32.Parse(id_game);
                movement.id_warehouse = Int32.Parse(id_warehouse);
                movement.movement_type = "in_transit";
                movement.quantity = Int32.Parse("-" + Request.Form["qremove" + count]);

                context.CreateMovement(movement);



                movement.id_game = Int32.Parse(id_game);
                movement.id_warehouse = Int32.Parse(Request.Form["selectwarehouse" + count]);
                movement.movement_type = "in_transit";
                movement.quantity = Int32.Parse(Request.Form["qremove" + count]);

                context.CreateMovement(movement);






                inventory.quantity = inventory.quantity - Int32.Parse(Request.Form["qremove" + count]);

                if (inventory.quantity == 0)
                {
                    context.DeleteInventory(inventory);

                    inventory.id_game = Int32.Parse(id_game);
                    inventory.id_warehouse = Int32.Parse(Request.Form["selectwarehouse" + count]);
                    inventory.quantity = Int32.Parse(Request.Form["qremove" + count]);

                    InventorySearch = context.SearchInventory(inventory.id_game.ToString(), inventory.id_warehouse.ToString());


                    if (InventorySearch.Count() == 0)
                    {
                        context.CreateInventory(inventory);
                    }
                    else
                    {
                        context.UpdateInventory(inventory);
                    }


                }
                else
                {
                    context.UpdateInventory(inventory);

                    inventory.id_game = Int32.Parse(id_game);
                    inventory.id_warehouse = Int32.Parse(Request.Form["selectwarehouse" + count]);
                    inventory.quantity = Int32.Parse(Request.Form["qremove" + count]);

                    InventorySearch = context.SearchInventory(inventory.id_game.ToString(), inventory.id_warehouse.ToString());

                    if (InventorySearch.Count() == 0)
                    {
                        context.CreateInventory(inventory);
                    }
                    else
                    {
                        context.UpdateInventory(inventory);
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Movement operation performed successfully!";
            OnGet();




        }
    }
}