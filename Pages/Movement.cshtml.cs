using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class MovementModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm

        public List<Game> Games = new List<Game>(); //Lista de Games
        public List<Warehouse> Warehouses = new List<Warehouse>(); //Lista de Warehouses

        [BindProperty]
        public IEnumerable<Movement> Movements { get; set; } //IEnumerable para lista de Movements
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Preenche a lista Movements com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Movements = context.GetAllMovements();

            //Preenche a lista Games com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Games = context.GetAllGames();

            //Preenche a lista Warehouses com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Warehouses = context.GetAllWarehouses();

        }

        //Método OnGet2 substitui o método OnGet após o OnPost para conseguir apresentar apenas
        //os dados filtrados da pesquisa e permitir novas pesquisas.
        //O método OnGet devolve sempre a lista de Movements completa (GetAllGetAllMovements).
        public void OnGet2()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Preenche a lista Games com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Games = context.GetAllGames();
            
            //Preenche a lista Warehouses com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Warehouses = context.GetAllWarehouses();
        }

        public void OnPost()
        {
            //Verifica que pelo menos um dos campos do formulário está preenchido,
            //mensagem de erro em caso contrário
            if (
                Request.Form["selectgame"] == "" &&
                Request.Form["selectwarehouse"] == "" &&
                Request.Form["selectmovement"] == "" &&
                Request.Form["date"] == ""
                )
            {
                errorMessage = "At least one field is required";
                OnGet();
                return;

            }
            LostGamesContext context = new LostGamesContext();
            try
            {
                //Chama o método search para apresentar os resultados filtrados com as opções selecionadas pelo utilizador
                this.Movements = context.SearchMovements(Request.Form["selectgame"], Request.Form["selectwarehouse"], Request.Form["selectmovement"], Request.Form["date"]);
                OnGet2();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }



        }
    }
}
