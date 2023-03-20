using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class GameModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm

        [BindProperty]
        public IEnumerable<Game> Games { get; set; } //IEnumerable para lista de Games
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext();//Context ligação entre o .Net e base de dados MySQL

            //Preenche a lista Games com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Games = context.GetAllGames();
        }

        public void OnPost()
        {
            LostGamesContext context = new LostGamesContext();

            try
            {

                //Chama o método search para apresentar os resultados filtrados com as opções selecionadas pelo utilizador
                this.Games = context.SearchGames(Request.Form["searchoption"], Request.Form["searchtext"]);
                


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

        }

    }


}