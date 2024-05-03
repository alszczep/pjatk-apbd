using Lab4.Model;

namespace Lab4.Services;

public interface IProductService
{
    Task<Product?> GetProductById(int id);
}
