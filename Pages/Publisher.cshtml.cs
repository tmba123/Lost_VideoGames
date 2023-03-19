using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    
    public class PublisherModel : PageModel
    {
        public string errorMessage = "";

        [BindProperty]
        public IEnumerable<Publisher> Publishers { get; set; }
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Publishers = context.GetAllPublishers();
        }

        public void OnPost()
        {
            LostGamesContext context = new LostGamesContext();

            try
            {

                if (Request.Form["search"].Equals("search"))
                {
                    this.Publishers = context.SearchPublishers(Request.Form["searchoption"], Request.Form["searchtext"]);
                }


            } catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }
            OnGet();
        }

    }


}

