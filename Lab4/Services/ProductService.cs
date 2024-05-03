using Lab4.Model;
using Lab4.Repositories;

namespace Lab4.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;

    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await this.productRepository.GetProductById(id);
    }
}
