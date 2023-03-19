namespace Lost_Videogames.Models
{
    public class Game
    {
        public int id_game { get; set; }
        public int id_publisher { get; set; }
        public string publisher_name { get; set; }
        public string img_url { get; set; }
        public string name { get; set; }
        public string genre { get; set; }
        public string platform { get; set; }
        public int release_year { get; set; }
        public string state { get; set; }

    }
}
