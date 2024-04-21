using Cwiczenia6.Model;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia6.Repositories;

public interface IAnimalsRepository
{
    IEnumerable<Animal> GetAnimals(string orderBy = "name");
    public int AddAnimal(Animal animal);
    public int UpdateAnimal(int id, Animal newAnimal);
    public int DeleteAnimal(int id);
}