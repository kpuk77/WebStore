using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _Db;
        private readonly ILogger<SqlProductData> _Logger;

        public SqlProductData(WebStoreDB db, ILogger<SqlProductData> logger)
        {
            _Db = db;
            _Logger = logger;
        }

        public IEnumerable<Section> GetSections() => _Db.Sections;

        public IEnumerable<Brand> GetBrands() => _Db.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _Db.Products;

            if (filter?.SectionId is { } sectionId)
                query = query.Where(p => p.SectionId == sectionId);

            if (filter?.BrandId is { } brandId)
                query = query.Where(p => p.BrandId == brandId);

            return query;
        }
    }
}
