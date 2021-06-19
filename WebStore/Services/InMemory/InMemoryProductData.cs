using System.Collections.Generic;
using System.Linq;

using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            if (filter?.Ids.Length > 0)
                query = query.Where(p => filter.Ids.Contains(p.Id));
            else
            {
                if (filter?.SectionId is { } sectionId)
                    query = query.Where(p => p.SectionId == sectionId);

                if (filter?.BrandId is { } brandId)
                    query = query.Where(p => p.BrandId == brandId);
            }

            return query;
        }

        public Product GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id);
    }
}