using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lost_Videogames.Pages
{
    public class PublisherCreateModel : PageModel
    {


        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
           
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
            try
            {

                Publisher publisher = new Publisher();

                publisher.img_url = Request.Form["img_url"];
                publisher.name = Request.Form["name"];
                publisher.country = Request.Form["country"];
                publisher.year = Int32.Parse(Request.Form["year"]);
                publisher.state = Request.Form["state"];
                context.CreatePublisher(publisher);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }
                
            successMessage = "Publisher created successfully";
            OnGet();
        }

    }
}
