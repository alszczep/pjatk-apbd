using Lab4.Model;

namespace Lab4.Repositories;

public interface IProductRepository
{
    Product? GetProductById(int id);
}