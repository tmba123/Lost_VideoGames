using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class WarehouseUpdateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";

        public Warehouse Warehouse = new Warehouse();

        public IEnumerable<Inventory> Inventories { get; set; }


        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Warehouse = context.SearchWarehouses("id_warehouse", Request.Query["id_warehouse"]).First();
        }

        public void OnPost()
        {
            if (Request.Form["location"] == "" ||
               Request.Form["state"] == "")
            {
                errorMessage = "All fields are required";
                OnGet();
                return;

            }

            LostGamesContext context = new LostGamesContext();

            Inventories = context.GetAllInventory();

            foreach (var item in Inventories)
            {
                if (Request.Form["state"] == "disabled" && Int32.Parse(Request.Form["id_warehouse"]) == item.id_warehouse && item.quantity > 0) 
                {
                    errorMessage = "Warehouse contains products! Cannot be disabled.";
                    OnGet();
                    return;

                }
            }


            try
            {

                Warehouse warehouse = new Warehouse()
                {
                    id_warehouse = Int32.Parse(Request.Form["id_warehouse"]),
                    location = (Request.Form["location"]),
                    state = (Request.Form["state"])
                };
                context.UpdateWarehouse(warehouse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Warehouse updated successfully";
            OnGet();
        }

    }
}