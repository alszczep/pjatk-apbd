using Lab4.Model;

namespace Lab4.Services;

public interface IProductService
{
    Product? GetProductById(int id);
}