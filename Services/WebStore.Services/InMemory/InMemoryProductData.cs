using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.InMemory
{
    [Obsolete("Поддержка прекращена", true)]
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public Section GetSection(int id) => GetSections().FirstOrDefault(s => s.Id == id);

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public Brand GetBrand(int id) => GetBrands().FirstOrDefault(b => b.Id == id);

        public ProductsPage GetProducts(ProductFilter filter = null)
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

            var totalCount = query.Count();

            return new ProductsPage(query, totalCount);
        }

        public Product GetProduct(int id) => TestData.Products.FirstOrDefault(p => p.Id == id);

        public int Add(Product product)
        {
            var maxId = TestData.Products.Max(p => p.Id);

            if (product is null)
                throw new ArgumentNullException();

            if (product.Id <= 0)
                product.Id = ++maxId;

            TestData.Products.Add(product);

            return product.Id;
        }

        public bool Remove(Product product)
        {
            if (product is null)
                throw new ArgumentNullException();

            if (!TestData.Products.Contains(product))
                return false;

            return TestData.Products.Remove(product);
        }

        public bool Remove(int id)
        {
            var product = GetProduct(id);

            return Remove(product);
        }

        public void Update(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}