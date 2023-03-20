using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace Lost_Videogames.Pages
{
    public class GameUpdateModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm
        public string successMessage = ""; //Variável para apresentação de success na pagina .cshtm

        public Game Game = new Game(); //Variável objeto do tipo game

        public IEnumerable<Inventory> Inventories { get; set; } //IEnumerable para lista de Inventory

        public List<Publisher> PublishersEnabled = new List<Publisher>(); //Lista de Publishers
        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Pesquisa de Game através do id_game submetido na Query parameter da página Game
            Game = context.SearchGames("id_game", Request.Query["id_game"]).First();

            //Pesquisa de publishers no apenas no estado enabled
            PublishersEnabled = context.SearchPublishers("state", "enabled");
        }

        public void OnPost()
        {
            //Verifica que se todos os campos do formulário estão preenchidos,
            //mensagem de erro no caso de faltar algum campo
            if (
                Request.Form["selectpublisher"] == "" ||
                Request.Form["img_url"] == "" ||
                Request.Form["name"] == "" ||
                Request.Form["genre"] == "" ||
                Request.Form["patform"] == "" ||
                Request.Form["release_year"] == "" ||
                Request.Form["state"] == "")
            {
                errorMessage = "All fields are required";
                OnGet();
                return;

            }

            LostGamesContext context = new LostGamesContext();

            Inventories = context.GetAllInventory();//Preenche a lista Inventories com a informação presente na base de dados.

            foreach (var item in Inventories)
            {
                //Verifica na lista de Inventory se existe o jogo com quantidade de stock superior a 0.
                //Caso exista envia mensagem de erro se o utilizador colocar o Game state a disabed.
                if (Request.Form["state"] == "disabled" && Int32.Parse(Request.Form["id_game"]) == item.id_game && item.quantity > 0)
                {
                    errorMessage = "Product exists in Inventory! Cannot be disabled.";
                    OnGet();
                    return;

                }

            }

            PublishersEnabled = context.GetAllPublishers(); //Preenche a lista Publishers com a informação presente na base de dados.

            foreach (var item in PublishersEnabled)
            {
                //Não permite colocar o state do jogo a enabled
                //caso o publisher desse jogo se encontre no estado disabled. Mensagem de erro.
                if (Request.Form["state"] == "enabled" && Int32.Parse(Request.Form["selectpublisher"]) == item.id_publisher && item.state == "disabled")
                {
                    errorMessage = "Publisher for this game is disabled! Cannot be enabled.";
                    OnGet();
                    return;

                }

            }

            //Cria objeto Game com os dados do formulário
            //e faz update dos novos dados na base de dados
            try
            {
                Game game = new Game()
                {
                    id_game = Int32.Parse(Request.Form["id_game"]),
                    id_publisher = Int32.Parse(Request.Form["selectpublisher"]),
                    img_url = Request.Form["img_url"],
                    name = Request.Form["name"],
                    genre = Request.Form["genre"],
                    platform = Request.Form["platform"],
                    release_year = Int32.Parse(Request.Form["release_year"]),
                    state = Request.Form["state"]

                };
                context.UpdateGame(game); //Game Update na base de dados

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Game updated successfully"; //Apresenta mensagem de sucesso no caso do try = true
            OnGet();

        }

    }
}


