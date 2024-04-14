using Api.Model;
using Api.Repositories;

namespace Api.Services;

public class AnimalsService : IAnimalsService
{
    private readonly IAnimalsRepository animalsRepository;
    
    public AnimalsService(IAnimalsRepository animalsRepository)
    {
        this.animalsRepository = animalsRepository;
    }
    
    public IEnumerable<Animal> GetAnimals(string? orderBy)
    {
        return animalsRepository.GetAnimals(orderBy);
    }
    
    public int CreateAnimal(Animal animal)
    {
        return animalsRepository.CreateAnimal(animal);
    }

    public int UpdateAnimal(Animal animal)
    {
        return animalsRepository.UpdateAnimal(animal);
    }

    public int DeleteAnimal(int animalId)
    {
        return animalsRepository.DeleteAnimal(animalId);
    }
}