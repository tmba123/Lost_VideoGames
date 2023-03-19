namespace Lost_Videogames.Models
{
    public class Movement
    {
        public int id_movement { get; set; }
        public int id_game { get; set; }
        public string game_name { get; set; }
        public int id_warehouse { get; set; }
        public string warehouse_location { get; set; }
        public string movement_type { get; set; }
        public int quantity { get; set; }
        public DateTime movement_date { get; set; }
    
    }
}
