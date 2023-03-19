using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Xml.Linq;
using static Lost_Videogames.Models.Publisher;

namespace Lost_Videogames.Models;

//
public class LostGamesContext
{
    public string ConnectionString { get; set; }

    //Construtor recebe como parametro a connection String.
    public LostGamesContext()
    {
        this.ConnectionString = "server=localhost;port=3306;database=gamestoreinventory;user=root;password=1234";
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }


    //########################################################--Publisher

    //Devolve uma lista não filtrada de todos os publishers na base de dados. - Read
    public List<Publisher> GetAllPublishers()
    {

        List<Publisher> publisherlist = new List<Publisher>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM publisher;", connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    publisherlist.Add(new Publisher()
                    {
                        id_publisher = reader.GetInt32("id_publisher"),
                        img_url = reader.GetString("img_url"),
                        name = reader.GetString("name"),
                        country = reader.GetString("country"),
                        year = reader.GetInt32("year"),
                        state = reader.GetString("state")

                    });
                }

            }

        }
        return publisherlist;
    }

    //Cria novo Publisher na base de dades - Create
    public void CreatePublisher(Publisher publisher)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("INSERT INTO publisher (img_url,name,country,year,state) VALUES (@img_url,@name,@country,@year,@state);", conn);


            cmd.Parameters.AddWithValue("@img_url", publisher.img_url);
            cmd.Parameters.AddWithValue("@name", publisher.name);
            cmd.Parameters.AddWithValue("@country", publisher.country);
            cmd.Parameters.AddWithValue("@year", publisher.year);
            cmd.Parameters.AddWithValue("@state", publisher.state);


            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }

    //Faz update a Publisher presente na base de dados - Update
    public void UpdatePublisher(Publisher publisher)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("UPDATE Publisher SET img_url=@img_url, name=@name, country=@country, year=@year, state=@state WHERE id_publisher=@id_publisher;", conn);


            cmd.Parameters.AddWithValue("@id_publisher", publisher.id_publisher);
            cmd.Parameters.AddWithValue("@img_url", publisher.img_url);
            cmd.Parameters.AddWithValue("@name", publisher.name);
            cmd.Parameters.AddWithValue("@country", publisher.country);
            cmd.Parameters.AddWithValue("@year", publisher.year);
            cmd.Parameters.AddWithValue("@state", publisher.state);

            cmd.ExecuteNonQuery();


            conn.Close();

        }
    }

    //Permite realizar pesquisas filtradas de publishers - Search 
    public List<Publisher> SearchPublishers(string searchoption, string searchtext)
    {
        List<Publisher> publisherlist = new List<Publisher>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM publisher WHERE " + searchoption + " LIKE CONCAT('%',@searchtext,'%');", connection);
            cmd.Parameters.AddWithValue("searchtext", searchtext);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    publisherlist.Add(new Publisher()
                    {
                        id_publisher = reader.GetInt32("id_publisher"),
                        img_url = reader.GetString("img_url"),
                        name = reader.GetString("name"),
                        country = reader.GetString("country"),
                        year = reader.GetInt32("year"),
                        state = reader.GetString("state")

                    });
                }

            }

        }
        return publisherlist;

    }

    //########################################################--Game

    //Devolve uma lista não filtrada de todos os games na base de dados. - Read
    public List<Game> GetAllGames()
    {

        List<Game> gamelist = new List<Game>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(

                "SELECT g.id_game, g.id_publisher, p.name AS pname, g.img_url, g.name AS gname, g.genre, g.platform, g.release_year, g.state" +
                " FROM game AS g" +
                " JOIN publisher AS p" +
                " USING(id_publisher)"

                , connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    gamelist.Add(new Game()
                    {
                        id_game = reader.GetInt32("id_game"),
                        id_publisher = reader.GetInt32("id_publisher"),
                        publisher_name = reader.GetString("pname"),
                        img_url = reader.GetString("img_url"),
                        name = reader.GetString("gname"),
                        genre = reader.GetString("genre"),
                        platform = reader.GetString("platform"),
                        release_year = reader.GetInt32("release_year"),
                        state = reader.GetString("state")

                    });
                }

            }

        }
        return gamelist;
    }

    //Cria novo game na base de dades – Create
    public void CreateGames(Game game)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("INSERT INTO game (id_publisher,img_url,name,genre,platform,release_year,state)" +
                " VALUES (@selectpublisher, @img_url,@name,@genre,@platform,@release_year,@state);", conn);

            cmd.Parameters.AddWithValue("@selectpublisher", game.id_publisher);
            cmd.Parameters.AddWithValue("@img_url", game.img_url);
            cmd.Parameters.AddWithValue("@name", game.name);
            cmd.Parameters.AddWithValue("@genre", game.genre);
            cmd.Parameters.AddWithValue("@platform", game.platform);
            cmd.Parameters.AddWithValue("@release_year", game.release_year);
            cmd.Parameters.AddWithValue("@state", game.state);


            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }

    //Faz update a Game presente na base de dados – Update
    public void UpdateGame(Game game)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("UPDATE game SET id_publisher=@selectpublisher, img_url=@img_url, name=@name, genre=@genre, platform=@platform, release_year=@release_year, state=@state WHERE id_game=@id_game;", conn);

            cmd.Parameters.AddWithValue("@id_game", game.id_game);
            cmd.Parameters.AddWithValue("@selectpublisher", game.id_publisher);
            cmd.Parameters.AddWithValue("@img_url", game.img_url);
            cmd.Parameters.AddWithValue("@name", game.name);
            cmd.Parameters.AddWithValue("@genre", game.genre);
            cmd.Parameters.AddWithValue("@platform", game.platform);
            cmd.Parameters.AddWithValue("@release_year", game.release_year);
            cmd.Parameters.AddWithValue("@state", game.state);

            cmd.ExecuteNonQuery();


            conn.Close();

        }
    }

    //Permite realizar pesquisas filtradas de Games - Search 
    public List<Game> SearchGames(string searchoption, string searchtext)
    {
        List<Game> gamelist = new List<Game>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand cmd = new MySqlCommand(


                "SELECT g.id_game, g.id_publisher, p.name AS pname, g.img_url, g.name AS gname, g.genre, g.platform, g.release_year, g.state" +
                " FROM game AS g" +
                " JOIN publisher AS p" +
                " USING(id_publisher)" +
                " WHERE " + searchoption +
                " LIKE CONCAT('%',@searchtext,'%');"
                , connection);

            cmd.Parameters.AddWithValue("searchtext", searchtext);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    gamelist.Add(new Game()
                    {
                        id_game = reader.GetInt32("id_game"),
                        id_publisher = reader.GetInt32("id_publisher"),
                        publisher_name = reader.GetString("pname"),
                        img_url = reader.GetString("img_url"),
                        name = reader.GetString("gname"),
                        genre = reader.GetString("genre"),
                        platform = reader.GetString("platform"),
                        release_year = reader.GetInt32("release_year"),
                        state = reader.GetString("state")

                    });
                }

            }

        }
        return gamelist;

    }

    //########################################################--Warehouse

    //Devolve uma lista não filtrada de todos os Warehouses na base de dados. - Read
    public List<Warehouse> GetAllWarehouses()
    {
        List<Warehouse> warehouselist = new List<Warehouse>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM warehouse;", connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    warehouselist.Add(new Warehouse()
                    {
                        id_warehouse = reader.GetInt32("id_warehouse"),
                        location = reader.GetString("location"),
                        state = reader.GetString("state")

                    });
                }

            }

        }
        return warehouselist;

    }

    //Cria novo Warehouse na base de dades – Create
    public void CreateWarehouses(Warehouse warehouse)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("INSERT INTO warehouse (location,state)" +
                " VALUES (@location, @state);", conn);

            cmd.Parameters.AddWithValue("@location", warehouse.location);
            cmd.Parameters.AddWithValue("@state", warehouse.state);


            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }

    //Faz update a Warehouses presentes na base de dados – Update
    public void UpdateWarehouse(Warehouse warehouse)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("UPDATE warehouse SET location=@location, state=@state WHERE id_warehouse=@id_warehouse;", conn);

            cmd.Parameters.AddWithValue("@id_warehouse", warehouse.id_warehouse);
            cmd.Parameters.AddWithValue("@location", warehouse.location);
            cmd.Parameters.AddWithValue("@state", warehouse.state);

            cmd.ExecuteNonQuery();


            conn.Close();

        }
    }

    //Permite realizar pesquisas filtradas de Warehouses - Search 
    public List<Warehouse> SearchWarehouses(string searchoption, string searchtext)
    {
        List<Warehouse> warehouselist = new List<Warehouse>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM warehouse WHERE " + searchoption + " LIKE CONCAT('%',@searchtext,'%');", connection);
            cmd.Parameters.AddWithValue("searchtext", searchtext);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    warehouselist.Add(new Warehouse()
                    {
                        id_warehouse = reader.GetInt32("id_warehouse"),
                        location = reader.GetString("location"),
                        state = reader.GetString("state")

                    });
                }

            }

        }
        return warehouselist;
    }
    //########################################################--Inventory

    //Devolve uma lista não filtrada de todos os produtos em stock na base de dados. - Read
    public List<Inventory> GetAllInventory()
    {
        List<Inventory> inventorylist = new List<Inventory>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(
                
                
                "SELECT i.id_game, g.name, g.img_url, i.id_warehouse, w.location, i.quantity" +
                " FROM inventory AS i" +
                " JOIN game AS g" +
                " USING(id_game)" +
                " JOIN warehouse as w" +
                " USING(id_warehouse)"

                , connection);


            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    inventorylist.Add(new Inventory()
                    {
                        id_game = reader.GetInt32("id_game"),
                        game_name = reader.GetString("name"),
                        id_warehouse = reader.GetInt32("id_warehouse"),
                        warehouse_location = reader.GetString("location"),
                        quantity = reader.GetInt32("quantity"),
                        img_url = reader.GetString("img_url"),

                    });
                }

            }

        }
        return inventorylist;

    }

    //Cria nova entrada de stock na base de dades – Create
    public void CreateInventory(Inventory inventory)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("INSERT INTO inventory (id_game, id_warehouse, quantity)" +
                " VALUES (@id_game, @id_warehouse, @quantity);", conn);

            cmd.Parameters.AddWithValue("@id_game", inventory.id_game);
            cmd.Parameters.AddWithValue("@id_warehouse", inventory.id_warehouse);
            cmd.Parameters.AddWithValue("@quantity", inventory.quantity);


            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }

    //Faz update ao stock presente na base de dados – Update
    public void UpdateInventory(Inventory inventory)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("UPDATE inventory SET quantity=@quantity" +
                " WHERE id_game=@id_game AND id_warehouse=@id_warehouse;", conn);

            cmd.Parameters.AddWithValue("@id_game", inventory.id_game);
            cmd.Parameters.AddWithValue("@id_warehouse", inventory.id_warehouse);
            cmd.Parameters.AddWithValue("@quantity", inventory.quantity);

            cmd.ExecuteNonQuery();


            conn.Close();

        }
    }

    //Apaga registo de stock da base de dados - Delete
    public void DeleteInventory(Inventory inventory)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("DELETE FROM inventory WHERE id_game=@id_game AND id_warehouse=@id_warehouse;", conn);

            cmd.Parameters.AddWithValue("@id_game", inventory.id_game);
            cmd.Parameters.AddWithValue("@id_warehouse", inventory.id_warehouse);

            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }

    //Permite realizar pesquisas filtradas ao stock em base de dados - Search
    public List<Inventory> SearchInventory(string game_id, string warehouse_id)
    {
        List<Inventory> inventorylist = new List<Inventory>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            string sqlTmp = "SELECT i.id_game, g.name, g.img_url, i.id_warehouse, w.location, i.quantity" +
                " FROM inventory AS i" +
                " JOIN game AS g" +
                " USING(id_game)" +
                " JOIN warehouse as w" +
                " USING(id_warehouse)";

            //Construção dinâmica da Query de Mysql consoante as opções selecionadas pelo utilizador - Search

            if (game_id.Length>0 && warehouse_id.Length > 0) {

                sqlTmp += " WHERE i.id_game = " + game_id + " AND i.id_warehouse = " + warehouse_id;  

            }
            else if(game_id.Length > 0)
            {
                sqlTmp += " WHERE i.id_game = " + game_id;
            }
            else
            {
                sqlTmp += " WHERE i.id_warehouse = " + warehouse_id;
            }

            
            MySqlCommand cmd = new MySqlCommand(

                    sqlTmp

                , connection);

           
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    inventorylist.Add(new Inventory()
                    {
                        id_game = reader.GetInt32("id_game"),
                        game_name = reader.GetString("name"),
                        id_warehouse = reader.GetInt32("id_warehouse"),
                        warehouse_location = reader.GetString("location"),
                        quantity = reader.GetInt32("quantity"),
                        img_url = reader.GetString("img_url"),

                    });
                }

            }

        }
        return inventorylist;

    }

    //Devolve lista de stock sem incluir o armazém Transito  - Search
    public List<Inventory> GetInventoryNoTransit()
    {
        List<Inventory> inventorylist = new List<Inventory>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(


                "SELECT i.id_game, g.name, g.img_url, i.id_warehouse, w.location, i.quantity" +
                " FROM inventory AS i" +
                " JOIN game AS g" +
                " USING(id_game)" +
                " JOIN warehouse as w" +
                " USING(id_warehouse)" +
                " WHERE i.id_warehouse > 1"

                , connection);


            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    inventorylist.Add(new Inventory()
                    {
                        id_game = reader.GetInt32("id_game"),
                        game_name = reader.GetString("name"),
                        id_warehouse = reader.GetInt32("id_warehouse"),
                        warehouse_location = reader.GetString("location"),
                        quantity = reader.GetInt32("quantity"),
                        img_url = reader.GetString("img_url"),

                    });
                }

            }

        }
        return inventorylist;

    }



    //########################################################--Movement

    //Devolve uma lista não filtrada de todos os movimentos na base de dados. - Read
    public List<Movement> GetAllMovements()
    {
        List<Movement> movementlist = new List<Movement>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(


                "SELECT m.id_movement, m.id_game, g.name, m.id_warehouse, w.location, m.movement_type, m.quantity, m.movement_date" +
                " FROM movement AS m" +
                " JOIN game AS g" +
                " USING(id_game)" +
                " JOIN warehouse as w" +
                " USING(id_warehouse)" +
                " ORDER BY m.id_movement"

                , connection);


            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    movementlist.Add(new Movement()
                    {
                        id_movement = reader.GetInt32("id_movement"),
                        id_game = reader.GetInt32("id_game"),
                        game_name = reader.GetString("name"),
                        id_warehouse = reader.GetInt32("id_warehouse"),
                        warehouse_location = reader.GetString("location"),
                        movement_type = reader.GetString("movement_type"),
                        quantity = reader.GetInt32("quantity"),
                        movement_date = reader.GetDateTime("movement_date"),

                    });
                }

            }

        }
        return movementlist;

    }

    //Cria novo movimento na base de dades – Create
    public void CreateMovement(Movement movement)
    {

        using (MySqlConnection conn = GetConnection())
        {
            //Abrir a ligação
            conn.Open();

            //Query
            MySqlCommand cmd = new MySqlCommand("INSERT INTO movement (id_game, id_warehouse, movement_type, quantity)" +
                " VALUES (@id_game, @id_warehouse, @movement_type, @quantity);", conn);

            cmd.Parameters.AddWithValue("@id_game", movement.id_game);
            cmd.Parameters.AddWithValue("@id_warehouse", movement.id_warehouse);
            cmd.Parameters.AddWithValue("@movement_type", movement.movement_type);
            cmd.Parameters.AddWithValue("@quantity", movement.quantity);


            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }

    //Permite realizar pesquisas filtradas aos movimentos em base de dados - Search
    public List<Movement> SearchMovements(string game_id, string warehouse_id, string movement_type, string date)
    {
        List<Movement> movementlist = new List<Movement>();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            int count=0;

            string sqlTmp = "SELECT m.id_movement, m.id_game, g.name, m.id_warehouse, w.location, m.movement_type, m.quantity, m.movement_date" +
                " FROM movement AS m" +
                " JOIN game AS g" +
                " USING(id_game)" +
                " JOIN warehouse as w" +
                " USING(id_warehouse)" +
                " WHERE";


            //Construção dinâmica da Query de Mysql consoante as opções selecionadas pelo utilizador – Search

            if (game_id.Length > 0)
            {
               
                sqlTmp += " m.id_game = " + game_id;
                count++;

            }
            if (warehouse_id.Length > 0)
            {
                if (count>0)
                {
                    sqlTmp += " AND m.id_warehouse = " + warehouse_id;
                }
                else
                {
                    sqlTmp += " m.id_warehouse = " + warehouse_id;
                    count++;
                }
                

            }
            if (movement_type.Length > 0)
            {
                if (count > 0)
                {
                    sqlTmp += " AND m.movement_type = '" + movement_type + "'";
                }
                else
                {
                    sqlTmp += " m.movement_type = '" + movement_type + "'";
                    count++;
                }


            }
            if (date.Length > 0)
            {
                if (count > 0)
                {
                    sqlTmp += " AND DATE(m.movement_date) = DATE('"+ date+ "')";
                }
                else
                {
                    sqlTmp += " DATE(m.movement_date) = DATE('"+ date+ "')";


                }


            }


            MySqlCommand cmd = new MySqlCommand(

                    sqlTmp

                , connection);


            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    movementlist.Add(new Movement()
                    {
                        id_movement = reader.GetInt32("id_movement"),
                        id_game = reader.GetInt32("id_game"),
                        game_name = reader.GetString("name"),
                        id_warehouse = reader.GetInt32("id_warehouse"),
                        warehouse_location = reader.GetString("location"),
                        movement_type = reader.GetString("movement_type"),
                        quantity = reader.GetInt32("quantity"),
                        movement_date = reader.GetDateTime("movement_date"),

                    });
                }

            }

        }
        return movementlist;

    }

}

