using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class WarehouseCreateModel : PageModel
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
            if (Request.Form["location"] == "" ||
                Request.Form["state"] == "")
            {
                errorMessage = "All fields are required";
                OnGet();
                return;

            }
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL
            try
            {
                //Cria objeto Warehouse com os dados do formulário
                //e faz Insert dos novos dados na base de dados
                Warehouse warehouse = new Warehouse();

                warehouse.location = Request.Form["location"];
                warehouse.state = Request.Form["state"];

                context.CreateWarehouses(warehouse); //Warehouse Insert na base de dados
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Warehouse created successfully"; //Apresenta mensagem de sucesso no caso do try = true
            OnGet();
        }

    }
}
