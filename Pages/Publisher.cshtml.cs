using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    
    public class PublisherModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm

        [BindProperty]
        public IEnumerable<Publisher> Publishers { get; set; }
        public void OnGet()
        {   
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL
            
            //Preenche a lista Publishers com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Publishers = context.GetAllPublishers();
        }

        public void OnPost()
        {
            LostGamesContext context = new LostGamesContext();

            try
            {

                if (Request.Form["search"].Equals("search")) 
                {
                    //Chama o método search para apresentar os resultados filtrados com as opções selecionadas pelo utilizador
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

