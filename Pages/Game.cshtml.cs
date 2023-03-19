using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class GameModel : PageModel
    {
        public string errorMessage = "";

        [BindProperty]
        public IEnumerable<Game> Games { get; set; }
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Games = context.GetAllGames();
        }

        public void OnPost()
        {
            LostGamesContext context = new LostGamesContext();

            try
            {

                if (Request.Form["search"].Equals("search"))
                {
                    this.Games = context.SearchGames(Request.Form["searchoption"], Request.Form["searchtext"]);
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