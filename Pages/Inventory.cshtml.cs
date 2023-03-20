using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class InventoryModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm


        public List<Game> GamesEnabled = new List<Game>(); //Lista de Games
        public List<Warehouse> WarehousesEnabled = new List<Warehouse>(); //Lista de Warehouses

        [BindProperty]
        public IEnumerable<Inventory> Inventories { get; set; } //IEnumerable para lista de Inventory
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Preenche a lista Inventories com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Inventories = context.GetAllInventory();

            //Pesquisa de Games no estado enabled
            GamesEnabled = context.SearchGames("g.state", "enabled");
            //Pesquisa de Warehouses no estado enabled
            WarehousesEnabled = context.SearchWarehouses("state", "enabled");
        }


        //Método OnGet2 substitui o método OnGet após o OnPost para conseguir apresentar apenas
        //os dados filtrados da pesquisa e permitir novas pesquisas.
        //O método OnGet devolve sempre a lista de Inventory completa (GetAllInventory).
        public void OnGet2()  
        {

            LostGamesContext context = new LostGamesContext();
            //Pesquisa de Games no estado enabled
            GamesEnabled = context.SearchGames("g.state", "enabled");
            //Pesquisa de Warehouses no estado enabled
            WarehousesEnabled = context.SearchWarehouses("state", "enabled");
        }
        public void OnPost()
        {
            //Verifica que pelo menos um dos campos do formulário está preenchido,
            //mensagem de erro em caso contrário
            if ( Request.Form["selectgame"] == "" &&
                 Request.Form["selectwarehouse"] == "")
            {
                errorMessage = "At least one field is required";
                OnGet();
                return;

            }
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL
            try
            {
                //Chama o método search para apresentar os resultados filtrados com as opções selecionadas pelo utilizador
                this.Inventories = context.SearchInventory(Request.Form["selectgame"], Request.Form["selectwarehouse"]);
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
