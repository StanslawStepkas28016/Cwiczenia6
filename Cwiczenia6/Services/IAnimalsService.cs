using Cwiczenia6.Model;

namespace Cwiczenia6.Services;

public interface IAnimalsService
{
    IEnumerable<Animal> GetAnimals(string orderBy = "name");
    public int AddAnimal(Animal animal);
    public int UpdateAnimal(int id, Animal newAnimal);
    public int DeleteAnimal(int id);
}