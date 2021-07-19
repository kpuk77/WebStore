using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping.DTO.Products;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient: BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, APIAddress.PRODUCTS) { }

        public IEnumerable<Section> GetSections() => Get<IEnumerable<SectionDTO>>($"{Address}/sections").FromDTO();

        public Section GetSection(int id) => Get<SectionDTO>($"{Address}/sections/{id}").FromDTO();

        public IEnumerable<Brand> GetBrands() => Get<IEnumerable<BrandDTO>>($"{Address}/brands").FromDTO();

        public Brand GetBrand(int id) => Get<BrandDTO>($"{Address}/brands/{id}").FromDTO();

        public ProductsPage GetProducts(ProductFilter filter = null)
        {
            var response = Post(Address, filter ?? new ProductFilter());
            var products = response.Content.ReadFromJsonAsync<ProductsPageDTO>().Result;

            return products.FromDTO();
        }

        public Product GetProduct(int id) => Get<ProductDTO>($"{Address}/{id}").FromDTO();

        public int Add(Product product)
        {
            var response = Post($"{Address}/add", product);

            return GetProducts().Products.Select(s => s.Id).Max();
        }
        
        public bool Remove(int id) => Delete($"{Address}/{id}").IsSuccessStatusCode;

        public void Update(Product product) => Put($"{Address}", product);
    }
}