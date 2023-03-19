using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class PublisherUpdateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";

        public Publisher Publisher = new Publisher();

        public IEnumerable<Game> Games { get; set; }


        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();
            Publisher = context.SearchPublishers("id_publisher", Request.Query["id_publisher"]).First();
        }

       public void OnPost()
        {
            if (Request.Form["img_url"] == "" ||
               Request.Form["name"] == "" ||
               Request.Form["country"] == "" ||
               Request.Form["year"] == "" ||
               Request.Form["state"] == "")
            {
                errorMessage = "All fields are required";
                OnGet();
                return;

            }

            LostGamesContext context = new LostGamesContext();

            Games = context.GetAllGames();

            foreach (var item in Games)
            {
                if (Request.Form["state"] == "disabled" && Int32.Parse(Request.Form["id_publisher"]) == item.id_publisher)
                {
                    errorMessage = "This Publisher contains products! Cannot be disabled.";
                    OnGet();
                    return;

                }
            }



            try
            {
                Publisher publisher = new Publisher()
                {
                    id_publisher = Int32.Parse(Request.Form["id_publisher"]),
                    img_url = (Request.Form["img_url"]),
                    name = (Request.Form["name"]),
                    country = (Request.Form["country"]),
                    year = Int32.Parse(Request.Form["year"]),
                    state = (Request.Form["state"])
                };
                context.UpdatePublisher(publisher);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Publisher updated successfully";
            OnGet();
        }
    
    }
}
