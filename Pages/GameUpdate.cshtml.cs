using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace Lost_Videogames.Pages
{
    public class GameUpdateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";

        public Game Game = new Game();

        public IEnumerable<Inventory> Inventories { get; set; }

        public List<Publisher> PublishersEnabled = new List<Publisher>();
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Game = context.SearchGames("id_game", Request.Query["id_game"]).First();
            
            PublishersEnabled = context.SearchPublishers("state", "enabled");
        }

        public void OnPost()
        {
            if (
                Request.Form["selectpublisher"] == "" ||
                Request.Form["img_url"] == "" ||
                Request.Form["name"] == "" ||
                Request.Form["genre"] == "" ||
                Request.Form["patform"] == "" ||
                Request.Form["release_year"] == "" ||
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
                if (Request.Form["state"] == "disabled" && Int32.Parse(Request.Form["id_game"]) == item.id_game && item.quantity > 0)
                {
                    errorMessage = "Product exists in Inventory! Cannot be disabled.";
                    OnGet();
                    return;

                }
            }


            try
            {
                Game game = new Game()
                {
                    id_game = Int32.Parse(Request.Form["id_game"]),
                    id_publisher = Int32.Parse(Request.Form["selectpublisher"]),
                    img_url = Request.Form["img_url"],
                    name = Request.Form["name"],
                    genre = Request.Form["genre"],
                    platform = Request.Form["platform"],
                    release_year = Int32.Parse(Request.Form["release_year"]),
                    state = Request.Form["state"]

                };
                context.UpdateGame(game);
               
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Game updated successfully";
            OnGet();

        }

    }
}


