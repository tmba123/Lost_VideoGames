using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class MovementModel : PageModel
    {
        public string errorMessage = "";

        public List<Game> Games = new List<Game>();
        public List<Warehouse> Warehouses = new List<Warehouse>();

        [BindProperty]
        public IEnumerable<Movement> Movements { get; set; }
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Movements = context.GetAllMovements();
            Games = context.GetAllGames();
            Warehouses = context.GetAllWarehouses();

        }

        public void OnGet2()
        {
            LostGamesContext context = new LostGamesContext();
            Games = context.GetAllGames();
            Warehouses = context.GetAllWarehouses();
        }

        public void OnPost()
        {
            if (
                Request.Form["selectgame"] == "" &&
                Request.Form["selectwarehouse"] == "" &&
                Request.Form["selectmovement"] == "" &&
                Request.Form["date"] == ""
                )
            {
                errorMessage = "At least one field is required";
                OnGet();
                return;

            }
            LostGamesContext context = new LostGamesContext();
            try
            {
                this.Movements = context.SearchMovements(Request.Form["selectgame"], Request.Form["selectwarehouse"], Request.Form["selectmovement"], Request.Form["date"]);
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
