using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lost_Videogames.Pages
{
    public class PublisherCreateModel : PageModel
    {


        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm
        public string successMessage = ""; //Variável para apresentação de success na pagina .cshtm
        public void OnGet()
        {
           
        }
        public void OnPost()
        {
            //Verifica que se todos os campos do formulário estão preenchidos,
            //mensagem de erro no caso de faltar algum campo
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
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL
            try
            {
                //Cria objeto Publisher com os dados do formulário
                //e faz Insert dos novos dados na base de dados
                Publisher publisher = new Publisher();

                publisher.img_url = Request.Form["img_url"];
                publisher.name = Request.Form["name"];
                publisher.country = Request.Form["country"];
                publisher.year = Int32.Parse(Request.Form["year"]);
                publisher.state = Request.Form["state"];
                
                context.CreatePublisher(publisher); //Publisher Insert na base de dados
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }
                
            successMessage = "Publisher created successfully"; //Apresenta mensagem de sucesso no caso do try = true 
            OnGet();
        }

    }
}
