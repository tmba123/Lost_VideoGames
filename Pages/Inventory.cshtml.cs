using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class InventoryModel : PageModel
    {
        public string errorMessage = "";
        

        public List<Game> GamesEnabled = new List<Game>();
        public List<Warehouse> WarehousesEnabled = new List<Warehouse>();

        [BindProperty]
        public IEnumerable<Inventory> Inventories { get; set; }
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Inventories = context.GetAllInventory();

            GamesEnabled = context.SearchGames("g.state", "enabled");
            WarehousesEnabled = context.SearchWarehouses("state", "enabled");
        }

        public void OnGet2()
        {
            LostGamesContext context = new LostGamesContext();
            GamesEnabled = context.SearchGames("g.state", "enabled");
            WarehousesEnabled = context.SearchWarehouses("state", "enabled");
        }
        public void OnPost()
        {
            if ( Request.Form["selectgame"] == "" &&
                 Request.Form["selectwarehouse"] == "")
            {
                errorMessage = "At least one field is required";
                OnGet();
                return;

            }
            LostGamesContext context = new LostGamesContext();
            try
            {
                this.Inventories = context.SearchInventory(Request.Form["selectgame"], Request.Form["selectwarehouse"]);
                OnGet2();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }
            


        }
    }
}
