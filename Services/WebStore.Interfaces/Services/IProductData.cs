using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        Section GetSection(int id);
        
        IEnumerable<Brand> GetBrands();

        Brand GetBrand(int id);

        IEnumerable<Product> GetProducts(ProductFilter filter = null);

        Product GetProductById(int id);

        int Add(Product product);

        bool Remove(Product product);

        bool RemoveById(int id);

        void Update(Product product);
    }
}
