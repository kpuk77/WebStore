using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WebStore.DAL.Context;
using WebStore.Domain;
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

        public IEnumerable<Section> GetSections() => _Db.Sections;

        public IEnumerable<Brand> GetBrands() => _Db.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
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

            return query;
        }

        public Product GetProductById(int id) => _Db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .SingleOrDefault(p => p.Id == id);

        public int Add(Product product)
        {
            if (product is null)
            {
                _Logger.LogError("---> Ошибка создания товара");
                throw new NullReferenceException(nameof(product));
            }

            _Db.Entry(product).State = EntityState.Added;

            _Db.SaveChanges();

            _Logger.LogInformation($"---> Добавление товара: {product.Name}");

            return product.Id;
        }

        public bool Remove(Product product)
        {
            if (product is null)
            {
                _Logger.LogError("---> Ошибка удаления товара");
                throw new NullReferenceException(nameof(product));
            }

            if (!_Db.Products.Contains(product))
            {
                _Logger.LogWarning("---> Ошибка удаления товара");
                return false;
            }

            _Db.Remove(product).State = EntityState.Deleted;

            _Db.SaveChanges();

            _Logger.LogInformation($"---> Товар удален {product.Name}");

            return true;
        }

        public bool RemoveById(int id)
        {
            if (GetProductById(id) is not { } product)
            {
                _Logger.LogError("---> Ошибка удаления товара");
                return false;
            }

            _Db.Remove(product).State = EntityState.Deleted;

            _Db.SaveChanges();

            _Logger.LogInformation($"---> Товар удален {product.Name}");

            return true;
        }

        public void Update(Product product)
        {
            if (product is null)
            {
                _Logger.LogError($"---> Ошибка изменения товара {product.Name}");
                throw new ArgumentNullException(nameof(product));
            }

            if (!_Db.Products.Contains(product))
            {
                _Logger.LogWarning($"---> Ошибка изменения товара {product.Name}");
                return;
            }

            _Db.Update(product);

            _Db.SaveChanges();

            _Logger.LogInformation($"---> Товар {product.Name} изменен");
        }
    }
}
