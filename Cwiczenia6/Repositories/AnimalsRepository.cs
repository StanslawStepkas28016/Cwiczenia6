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

    public IEnumerable<Animal> GetAnimals(string query)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        
        using var cmd = new SqlCommand($"SELECT IdAnimal, Name, Description, Category, Area FROM Animal ORDER BY {query} ASC", con);
        
        var reader = cmd.ExecuteReader();
        var animals = new List<Animal>();
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
        string query = $"UPDATE Animals SET Name = '{newAnimal.Name}', " +
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