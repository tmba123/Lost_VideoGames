using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lost_Videogames.Models;

namespace Lost_Videogames.Pages
{
    public class WarehouseModel : PageModel
    {

        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm

        [BindProperty]
        public IEnumerable<Warehouse> Warehouses { get; set; } //IEnumerable para lista de Warehouses
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Preenche a lista Warehouses com a informação presente na base de dados.
            //Esta lista vai ser utilizada no .cshtml para mostrar a tabela de resultados ao utilizador
            Warehouses = context.GetAllWarehouses();
        }

        public void OnPost()
        {
            LostGamesContext context = new LostGamesContext();

            try
            {
                //Chama o método search para apresentar os resultados filtrados com as opções selecionadas pelo utilizador       
                this.Warehouses = context.SearchWarehouses(Request.Form["searchoption"], Request.Form["searchtext"]);

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

        }
    }
}
