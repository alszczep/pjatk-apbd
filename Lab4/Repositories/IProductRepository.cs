using Lab4.Model;

namespace Lab4.Repositories;

public interface IProductRepository
{
    Task<Product?> GetProductById(int id);
}
