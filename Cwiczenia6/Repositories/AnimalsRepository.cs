using System.Data.SqlClient;
using Cwiczenia6.Model;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia6.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private const string ConnectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=bazaTestowa1234";

    public IEnumerable<Animal> GetAnimals(string orderBy = "name")
    {
        // Walidacja, zabezpieczająca przed SQL Injection.
        HashSet<string> validationSet = ["name", "description", "category", "area"];
        if (!validationSet.Contains(orderBy))
        {
            return new List<Animal>();
        }

        // Zapytanie, bez "ASC", bo jest ono natywnie przy "ORDER BY".
        string query = $"SELECT * FROM Animal ORDER BY {orderBy}";

        // Połączenie z bazą i czytanie do listy.
        List<Animal> animals = new List<Animal>();

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            using (SqlDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    animals.Add(
                        new Animal(
                            (int)dr["IdAnimal"],
                            dr["name"].ToString(),
                            dr["description"].ToString(),
                            dr["category"].ToString(),
                            dr["area"].ToString())
                    );
                }
            }
        }

        return animals;
    }

    public int AddAnimal([FromBody] Animal animal)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            string query =
                $"INSERT INTO Animal (Name, Description, Category, Area) " +
                "VALUES (@Name, @Description, @Category, @Area)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result;
            }
        }
    }

    public int UpdateAnimal(int id, Animal newAnimal)
    {
        string query = $"UPDATE Animals SET Name = '{newAnimal.Name}', " +
                       $"Description = '{newAnimal.Description}'," +
                       $"Category = '{newAnimal.Category}'," +
                       $"Area = '{newAnimal.Category}' " +
                       $"WHERE IdAnimal = {id}";

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            
            int result = command.ExecuteNonQuery();
            
            return result;
        }
    }

    public int DeleteAnimal(int id)
    {
        string query = $"DELETE FROM Animal WHERE IdAnimal = {id}";

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            
            int result = command.ExecuteNonQuery();
            
            return result;
        }
    }
}