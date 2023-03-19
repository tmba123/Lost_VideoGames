namespace Lost_Videogames.Models
{
    public class Inventory
    {
        public int id_game { get; set; }
        public string game_name { get; set; }
        public int id_warehouse { get; set; }
        public string warehouse_location { get; set; }
        public int quantity { get; set; }
        public string img_url { get; set; }
    }
}
