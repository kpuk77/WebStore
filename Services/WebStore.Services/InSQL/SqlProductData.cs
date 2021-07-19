using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

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

        public IEnumerable<Section> GetSections() => _Db.Sections.Include(s => s.Products);
        public Section GetSection(int id) => GetSections().FirstOrDefault(s => s.Id == id);

        public IEnumerable<Brand> GetBrands() => _Db.Brands;
        public Brand GetBrand(int id) => GetBrands().FirstOrDefault(b => b.Id == id);

        public ProductsPage GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _Db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (filter?.Ids?.Length > 0)
                query = query.Where(p => filter.Ids.Contains(p.Id));
            else
            {
                if (filter?.SectionId is { } sectionId)
                    query = query.Where(p => p.SectionId == sectionId);

                if (filter?.BrandId is { } brandId)
                    query = query.Where(p => p.BrandId == brandId);
            }

            var totalCount = query.Count();

            if (filter is { PageSize: > 0 and var pageSize, Page: > 0 and var pageNumber })
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

            return new ProductsPage(query.AsEnumerable(), totalCount);
        }

        public Product GetProduct(int id) => _Db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .SingleOrDefault(p => p.Id == id);

        public int Add(Product product)
        {
            if (product is null)
            {
                _Logger.LogError("Ошибка создания товара");
                throw new NullReferenceException(nameof(product));
            }

            _Db.Add(product);

            _Db.SaveChanges();

            _Logger.LogInformation($"Добавление товара: {product.Name} id: {product.Id}");

            return product.Id;
        }

        public bool Remove(Product product)
        {
            if (product is null)
            {
                _Logger.LogError("Ошибка удаления товара");
                throw new NullReferenceException(nameof(product));
            }

            if (!_Db.Products.Contains(product))
            {
                _Logger.LogWarning("Ошибка удаления товара");
                return false;
            }

            _Db.Remove(product).State = EntityState.Deleted;

            _Db.SaveChanges();

            _Logger.LogInformation($"Товар удален {product.Name} id: {product.Id}");

            return true;
        }

        public bool Remove(int id)
        {
            if (GetProduct(id) is not { } product)
            {
                _Logger.LogError("Ошибка удаления товара");
                return false;
            }

            _Db.Remove(product).State = EntityState.Deleted;

            _Db.SaveChanges();

            _Logger.LogInformation($"Товар удален {product.Name} id:{id}");

            return true;
        }

        public void Update(Product product)
        {
            if (product is null)
            {
                _Logger.LogError($"Ошибка изменения товара {product.Name}");
                throw new ArgumentNullException(nameof(product));
            }

            if (!_Db.Products.Contains(product))
            {
                _Logger.LogWarning($"Ошибка изменения товара {product.Name}");
                return;
            }

            _Db.Update(product);

            _Db.SaveChanges();

            _Logger.LogInformation($"Товар {product.Name} id: {product.Id} изменен");
        }
    }
}