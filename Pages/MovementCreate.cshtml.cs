using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class MovementCreateModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm
        public string successMessage = "";//Variável para apresentação de success na pagina .cshtm
        public string movementtype = ""; //Variável que passa para o .cshtml o movimento escolhido pelo  utilizador

        public List<Game> GamesEnabled = new List<Game>(); //Lista de Games
        public List<Warehouse> WarehousesEnabled = new List<Warehouse>(); //Lista de Warehouses
        public List<Inventory> InventorySearch = new List<Inventory>(); //Lista de Inventory
        public Inventory Inventory = new Inventory(); //Variável objeto do tipo Inventory

        public void OnGet()
        {


        }
        public void OnPost()
        {


            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Verifica qual a opção escolhida pelo utilizador e define o valor da variável “movementtype”
            //de acordo com a opção escolhida e a mesma é tratada no .cshtml
            if (Request.Form["movement_type"] == "add_stock")
            {
                movementtype = "add_stock";
                
                //Pesquisa de Games no estado enabled
                GamesEnabled = context.SearchGames("g.state", "enabled");

                //Pesquisa de Warehouses no estado enabled
                WarehousesEnabled = context.SearchWarehouses("state", "enabled");
            }

            if (Request.Form["movement_type"] == "remove_stock")
            {
                movementtype = "remove_stock";

                //Pesquisa de Inventory sem incluir o armazém Transito
                InventorySearch = context.GetInventoryNoTransit();
            }

            if (Request.Form["movement_type"] == "in_transit")
            {
                movementtype = "in_transit";

                //Pesquisa de Warehouses no estado enabled
                WarehousesEnabled = context.SearchWarehouses("state", "enabled");

                //Pesquisa de Inventory onde warehouse igual "Transito"
                InventorySearch = context.SearchInventory("", "1");
            }


        }
        public void OnPostAdd()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Verifica se todos os campos do formulário estão preenchidos,
            //mensagem de erro no caso de faltar algum campo
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
                //Cria objeto movimento com os dados do formulário
                Movement movement = new Movement();

                movement.id_game = Int32.Parse(Request.Form["selectgame"]);
                movement.id_warehouse = Int32.Parse(Request.Form["selectwarehouse"]);
                movement.movement_type = Request.Form["movement_type"];
                movement.quantity = Int32.Parse(Request.Form["quantity"]);


                //Pesquisa o inventory pela combinação id_game e id_warehouse
                InventorySearch = context.SearchInventory(movement.id_game.ToString(), movement.id_warehouse.ToString());

                //Se o resultado da pesquisa for zero significa que não existe a combinação em Inventory
                //desta forma é dada uma nova entrada em Inventory
                if (InventorySearch.Count() == 0)
                {
                    //Cria objeto Inventory com os dados do formulário
                    Inventory inventory = new Inventory();
                    inventory.id_game = Int32.Parse(Request.Form["selectgame"]);
                    inventory.id_warehouse = Int32.Parse(Request.Form["selectwarehouse"]);
                    inventory.quantity = Int32.Parse(Request.Form["quantity"]);


                    context.CreateMovement(movement); //Movement Insert na base de dados
                    context.CreateInventory(inventory); //Inventory Insert na base de dados


                }
                else //caso a pesquisa encontre a combinação id_game e id_warehouse em Inventory
                {
                    //Cria objeto Inventory com os dados encontrados na pesquisa
                    Inventory inventory = InventorySearch.First();

                    //Adiciona a quantidade introduzida pelo utilizador à quantidade existente em stock
                    inventory.quantity += Int32.Parse(Request.Form["quantity"]);


                    context.CreateMovement(movement); //Movement Insert na base de dados
                    context.UpdateInventory(inventory); //Inventory Update na base de dados

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Stock Added successfully"; //Apresenta mensagem de sucesso no caso do try = true
            OnGet();
        }

        public void OnPostRemove(string id_game, string id_warehouse, string count)
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Pesquisa o inventory pela combinação id_game e id_warehouse
            InventorySearch = context.SearchInventory(id_game, id_warehouse);

            //Cria objeto Inventory com os dados encontrados na pesquisa
            Inventory inventory = InventorySearch.First();

            //Verifica se todos os campos do formulário estão preenchidos,
            //mensagem de erro no caso de faltar algum campo
            if (
               Request.Form["qremove" + count] == "" ||
               Request.Form["reason" + count] == ""
               )

            {
                errorMessage = "All fields are required";
                OnGet();
                return;
            }

            //Verifica se a quantidade a remover introduzida pelo utilizado não é superior à quantidade existente em stock
            //mensagem de erro no caso de quantidade ser superior
            if (Int32.Parse(Request.Form["qremove" + count]) > inventory.quantity)

            {
                errorMessage = "Inserted quantity superior to quantity available!";
                OnGet();
                return;

            }


            try
            {
                //Cria objeto movimento com os dados do formulário
                Movement movement = new Movement();

                movement.id_game = Int32.Parse(id_game);
                movement.id_warehouse = Int32.Parse(id_warehouse);
                movement.movement_type = Request.Form["reason" + count];
                movement.quantity = Int32.Parse("-" + Request.Form["qremove" + count]); //Valor negativo por se tratar de uma operação de remove quantity

                //Remove a quantidade introduzida pelo utilizador à quantidade existente em stock
                inventory.quantity = inventory.quantity - Int32.Parse(Request.Form["qremove" + count]);

                //Se tipo de movimento for “in_transit” necessário criar dois movimentos um de saída e um de entrada para armazém Transit
                //E atualizar o stock do armazém onde a quantidade foi removida 
                if (movement.movement_type == "in_transit")
                {
                    context.CreateMovement(movement); //Movement de saida Insert na base de dados

                    movement.id_warehouse = 1; //Definido armazém Transit = id_warehouse 1
                    movement.quantity = Int32.Parse(Request.Form["qremove" + count]); //Definida a quantidade a adicionar ao armazém Transit igual à quantidade removida no outro armazém

                    context.CreateMovement(movement); //Movement de entrada Insert na base de dados

                    //Se o resultado da subtração de quantidade em Inventory for igual a 0 então o registo é apagado do Inventorio
                    if (inventory.quantity == 0)
                    {
                        context.DeleteInventory(inventory); //Inventory delete na base de dados

                    }
                    else //Caso a quantidade restante seja superior a 0 faz update ao Inventory
                    {
                        context.UpdateInventory(inventory); //Inventory Update na base de dados
                    }

                    inventory.id_warehouse = 1; //Definido armazém Transit = id_warehouse 1 
                    inventory.quantity = Int32.Parse(Request.Form["qremove" + count]); //Definida a quantidade a adicionar ao armazém Transit igual à quantidade removida no outro armazém

                    //Pesquisa o inventory pela combinação id_game e id_warehouse
                    InventorySearch = context.SearchInventory(inventory.id_game.ToString(), inventory.id_warehouse.ToString());

                    //Se o resultado da pesquisa for zero significa que não existe a combinação em Inventory
                    //desta forma é dada uma nova entrada em Inventory
                    if (InventorySearch.Count() == 0)
                    {
                        context.CreateInventory(inventory); //Inventory Insert na base de dados
                    }
                    else //caso a pesquisa encontre a combinação id_game e id_warehouse em Inventory
                    {
                        context.UpdateInventory(inventory); //Inventory Update na base de dados
                    }




                } ////Se tipo de movimento igual a “remove_stock” ou "stock_reconciliation" apenas necessário criar um movimentos
                else if (movement.movement_type == "remove_stock" || movement.movement_type == "stock_reconciliation")
                {

                    //Se o resultado da subtração de quantidade em Inventory for igual a 0 então o registo é apagado do Inventorio
                    if (inventory.quantity == 0)
                    {
                        context.CreateMovement(movement); //Movement Insert na base de dados
                        context.DeleteInventory(inventory); //Inventory delete na base de dados

                    }
                    else
                    {
                        context.CreateMovement(movement); //Movement Insert na base de dados
                        context.UpdateInventory(inventory); //Inventory Update na base de dados
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Movement operation performed successfully!"; //Apresenta mensagem de sucesso no caso do try = true
            OnGet();

        }
        public void OnPostTransit(string id_game, string id_warehouse, string count)
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Pesquisa o inventory pela combinação id_game e id_warehouse
            InventorySearch = context.SearchInventory(id_game, id_warehouse);

            //Cria objeto Inventory com os dados encontrados na pesquisa
            Inventory inventory = InventorySearch.First();

            //Verifica se todos os campos do formulário estão preenchidos,
            //mensagem de erro no caso de faltar algum campo
            if (
               Request.Form["qremove" + count] == "" ||
               Request.Form["selectwarehouse" + count] == ""
               )

            {
                errorMessage = "All fields are required";
                OnGet();
                return;
            }

            //Verifica se a quantidade a remover introduzida pelo utilizado não é superior à quantidade existente em stock
            //mensagem de erro no caso de quantidade ser superior
            if (Int32.Parse(Request.Form["qremove" + count]) > inventory.quantity)

            {
                errorMessage = "Inserted quantity superior to quantity available!";
                OnGet();
                return;

            }



            try
            {
                //Movimento “in_transit” necessário criar dois movimentos em Movement e em Inventory um de saída e um de entrada

                //Cria objeto movimento com os dados do formulário
                Movement movement = new Movement();


                movement.id_game = Int32.Parse(id_game);
                movement.id_warehouse = Int32.Parse(id_warehouse); //armazém Transit
                movement.movement_type = "in_transit";
                movement.quantity = Int32.Parse("-" + Request.Form["qremove" + count]); //Valor negativo por se tratar de uma operação de remove quantity

                context.CreateMovement(movement); //Movement de saida Insert na base de dados



                movement.id_game = Int32.Parse(id_game);
                movement.id_warehouse = Int32.Parse(Request.Form["selectwarehouse" + count]); //novo armazém
                movement.movement_type = "in_transit";
                movement.quantity = Int32.Parse(Request.Form["qremove" + count]); //Quantidade a adicionar ao novo armazém igual à quantidade removida do armazém Transit

                context.CreateMovement(movement); //Movement de entrada Insert na base de dados





                //Remove a quantidade introduzida pelo utilizador à quantidade existente em stock
                inventory.quantity = inventory.quantity - Int32.Parse(Request.Form["qremove" + count]);

                //Se o resultado da subtração de quantidade em Inventory for igual a 0 então o registo é apagado do Inventorio
                if (inventory.quantity == 0)
                {
                    //Saida de stock
                    context.DeleteInventory(inventory); //Inventory delete na base de dados


                    //Entrada de stock
                    inventory.id_game = Int32.Parse(id_game);
                    inventory.id_warehouse = Int32.Parse(Request.Form["selectwarehouse" + count]);
                    inventory.quantity = Int32.Parse(Request.Form["qremove" + count]);

                    //Pesquisa o inventory pela combinação id_game e id_warehouse
                    InventorySearch = context.SearchInventory(inventory.id_game.ToString(), inventory.id_warehouse.ToString());


                    //Se o resultado da pesquisa for zero significa que não existe a combinação em Inventory
                    //desta forma é dada uma nova entrada em Inventory
                    if (InventorySearch.Count() == 0)
                    {
                        context.CreateInventory(inventory);  //Inventory Insert na base de dados
                    }
                    else //caso a pesquisa encontre a combinação id_game e id_warehouse em Inventory
                    {
                        context.UpdateInventory(inventory); //Inventory Update na base de dados
                    }


                }
                else //Resultado da subtração de quantidade em Inventory superior a 0
                {
                    context.UpdateInventory(inventory); //Inventory Update na base de dados (Transit)

                    //Entrada de stock
                    inventory.id_game = Int32.Parse(id_game);
                    inventory.id_warehouse = Int32.Parse(Request.Form["selectwarehouse" + count]);
                    inventory.quantity = Int32.Parse(Request.Form["qremove" + count]);

                    //Pesquisa o inventory pela combinação id_game e id_warehouse
                    InventorySearch = context.SearchInventory(inventory.id_game.ToString(), inventory.id_warehouse.ToString());

                    //Se o resultado da pesquisa for zero significa que não existe a combinação em Inventory
                    //desta forma é dada uma nova entrada em Inventory
                    if (InventorySearch.Count() == 0)
                    {
                        context.CreateInventory(inventory); //Inventory Insert na base de dados
                    }
                    else //caso a pesquisa encontre a combinação id_game e id_warehouse em Inventory
                    {
                        context.UpdateInventory(inventory); //Inventory Update na base de dados
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Movement operation performed successfully!"; //Apresenta mensagem de sucesso no caso do try = true
            OnGet();




        }
    }
}