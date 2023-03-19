using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lost_Videogames.Models;

namespace Lost_Videogames.Pages
{
    public class WarehouseModel : PageModel
    {

        public string errorMessage = "";

        [BindProperty]
        public IEnumerable<Warehouse> Warehouses { get; set; }
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Warehouses = context.GetAllWarehouses();
        }

        public void OnPost()
        {
            LostGamesContext context = new LostGamesContext();

            try
            {

                if (Request.Form["search"].Equals("search"))
                {
                    this.Warehouses = context.SearchWarehouses(Request.Form["searchoption"], Request.Form["searchtext"]);
                }


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

        }
    }
}
