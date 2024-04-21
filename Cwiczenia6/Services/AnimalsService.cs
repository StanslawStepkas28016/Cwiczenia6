using Cwiczenia6.Model;
using Cwiczenia6.Repositories;

namespace Cwiczenia6.Services;

public class AnimalsService : IAnimalsService
{
    private readonly IAnimalsRepository _animalsRepository;

    public AnimalsService(IAnimalsRepository animalsRepository)
    {
        _animalsRepository = animalsRepository;
    }

    public IEnumerable<Animal> GetAnimals(string orderBy = "name")
    {
        return _animalsRepository.GetAnimals(orderBy);
    }

    public int AddAnimal(Animal animal)
    {
        return _animalsRepository.AddAnimal(animal);
    }

    public int UpdateAnimal(int id, Animal newAnimal)
    {
        return _animalsRepository.UpdateAnimal(id, newAnimal);
    }

    public int DeleteAnimal(int id)
    {
        return _animalsRepository.DeleteAnimal(id);
    }
}