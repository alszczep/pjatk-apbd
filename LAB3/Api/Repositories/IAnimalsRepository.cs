using Api.Model;

namespace Api.Repositories;

public interface IAnimalsRepository
{
    IEnumerable<Animal> GetAnimals(string? orderBy);
    int CreateAnimal(Animal animal);
    int UpdateAnimal(Animal animal);
    int DeleteAnimal(int animalId);
}