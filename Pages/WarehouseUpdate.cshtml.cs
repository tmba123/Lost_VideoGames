using Lost_Videogames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lost_Videogames.Pages
{
    public class WarehouseUpdateModel : PageModel
    {
        public string errorMessage = ""; //Variável para apresentação de erros na pagina .cshtm
        public string successMessage = ""; //Variável para apresentação de success na pagina .cshtm

        public Warehouse Warehouse = new Warehouse(); //Variável objeto do tipo warehouse

        public IEnumerable<Inventory> Inventories { get; set; } //IEnumerable para lista de Inventory


        public void OnGet()
        {
            LostGamesContext context = new LostGamesContext(); //Context ligação entre o .Net e base de dados MySQL

            //Pesquisa de Warehouse através do id_game submetido na Query parameter da página Warehouse
            Warehouse = context.SearchWarehouses("id_warehouse", Request.Query["id_warehouse"]).First();
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

            LostGamesContext context = new LostGamesContext();

            Inventories = context.GetAllInventory();//Preenche a lista Inventories com a informação presente na base de dados.

            foreach (var item in Inventories)
            {
                //Verifica na lista de Inventory se existe o Warehouse.
                //Caso exista envia mensagem de erro se o utilizador colocar o Warehouse state a disabed.
                if (Request.Form["state"] == "disabled" && Int32.Parse(Request.Form["id_warehouse"]) == item.id_warehouse) 
                {
                    errorMessage = "Warehouse contains products! Cannot be disabled.";
                    OnGet();
                    return;

                }
            }

            //Cria objeto Warehouse com os dados do formulário
            //e faz update dos novos dados na base de dados
            try
            {

                Warehouse warehouse = new Warehouse()
                {
                    id_warehouse = Int32.Parse(Request.Form["id_warehouse"]),
                    location = (Request.Form["location"]),
                    state = (Request.Form["state"])
                };
                context.UpdateWarehouse(warehouse);//Warehouse Update na base de dados
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
                return;
            }

            successMessage = "Warehouse updated successfully";//Apresenta mensagem de sucesso no caso do try = true
            OnGet();
        }

    }
}