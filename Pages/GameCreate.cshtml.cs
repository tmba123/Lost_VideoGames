using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lost_Videogames.Pages
{
    public class GameCreateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";

        public List<Publisher> PublishersEnabled = new List<Publisher>();
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
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
            try
            {

                Game game = new Game();
        
                game.id_publisher = Int32.Parse(Request.Form["selectpublisher"]);
                game.img_url = Request.Form["img_url"];
                game.name = Request.Form["name"];
                game.genre = Request.Form["genre"];
                game.platform = Request.Form["platform"];
                game.release_year = Int32.Parse(Request.Form["release_year"]);
                game.state = Request.Form["state"];
                
                context.CreateGames(game);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Game created successfully";
            OnGet();
        }
    }
}
