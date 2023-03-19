using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class WarehouseCreateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {

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
            try
            {

                Warehouse warehouse = new Warehouse();

                warehouse.location = Request.Form["location"];
                warehouse.state = Request.Form["state"];
                context.CreateWarehouses(warehouse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Warehouse created successfully";
            OnGet();
        }

    }
}
