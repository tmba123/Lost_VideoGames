using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class PublisherUpdateModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm
        public string successMessage = ""; //Variável para apresentação de success na pagina .cshtm

        public Publisher Publisher = new Publisher(); //Variável objeto do tipo Publisher

        public IEnumerable<Game> Games { get; set; } //IEnumerable para lista de Games


        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Pesquisa de Publisher através do id_publisher submetido na Query parameter da página Publisher
            Publisher = context.SearchPublishers("id_publisher", Request.Query["id_publisher"]).First(); 
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

            LostGamesContext context = new LostGamesContext();

            Games = context.GetAllGames();//Preenche a lista Games com a informação presente na base de dados.

            foreach (var item in Games)
            {
                //Verifica na lista de jogos se existem jogos no estado enabled deste publisher.
                //Caso exista envia mensagem de erro se o utilizador colocar o publisher state a disabed.
                if (Request.Form["state"] == "disabled" && Int32.Parse(Request.Form["id_publisher"]) == item.id_publisher && item.state == "enabled")
                {
                    errorMessage = "This Publisher contains products! Cannot be disabled.";
                    OnGet();
                    return;

                }
            }


            //Cria objeto Publisher com os dados do formulário
            //e faz update dos novos dados na base de dados
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
                context.UpdatePublisher(publisher); //Publisher Update na base de dados
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Publisher updated successfully"; //Apresenta mensagem de sucesso no caso do try = true 
            OnGet();
        }
    
    }
}
