using System.Data.SqlClient;
using Cwiczenia6.Model;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia6.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private readonly string _connectionString;

    public AnimalsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Failed to load the database connection string.");
        }
    }

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
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, con);
        // cmd.Parameters.AddWithValue() - do zapobiegania
        con.Open();

        var reader = cmd.ExecuteReader();
        List<Animal> animals = new List<Animal>();
        while (reader.Read())
        {
            var data = new Animal
            {
                IdAnimal = (int)reader["IdAnimal"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Category = reader["Category"].ToString(),
                Area = reader["Area"].ToString()
            };
            animals.Add(data);
        }


        return animals;
    }

    public int AddAnimal([FromBody] Animal animal)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
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
        string query = $"UPDATE Animal SET Name = '{newAnimal.Name}', " +
                       $"Description = '{newAnimal.Description}'," +
                       $"Category = '{newAnimal.Category}'," +
                       $"Area = '{newAnimal.Category}' " +
                       $"WHERE IdAnimal = {id}";

        using (SqlConnection connection = new SqlConnection(_connectionString))
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

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            int result = command.ExecuteNonQuery();

            return result;
        }
    }
}