using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _Db;
        private readonly ILogger<WebStoreDBInitializer> _Logger;

        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer> logger)
        {
            _Db = db;
            _Logger = logger;
        }

        public void Initialize()
        {
            //_Db.Database.EnsureDeleted();

            _Logger.LogInformation("---> Инициализация БД...");
            var timer = Stopwatch.StartNew();


            if (_Db.Database.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("---> Миграция БД...");
                _Db.Database.Migrate();
                _Logger.LogInformation($"---> Миграция БД выполнена за {timer.Elapsed.TotalSeconds} c.");
            }
            else
                _Logger.LogInformation("---> БД в актуальном состоянии.");

            try
            {
                InitializeProducts();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "---> Ошибка инициализации товаров в БД.");
                throw;
            }

            _Logger.LogInformation($"---> Инициализация БД завершена за {timer.Elapsed.TotalSeconds} c.");
        }

        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();
            if (_Db.Products.Any())
            {
                _Logger.LogInformation("---> Инициализация товаров не требуется.");
                return;
            }

            _Logger.LogInformation("---> Инициализация товаров...");

            ConvertData();

            using (_Db.Database.BeginTransaction())
            {
                _Logger.LogInformation("---> Добавление данных в БД...");
                _Db.Products.AddRange(TestData.Products);
                _Db.Brands.AddRange(TestData.Brands);
                _Db.Sections.AddRange(TestData.Sections);

                _Db.SaveChanges();

                _Db.Database.CommitTransaction();
                _Logger.LogInformation($"---> Добавление данных в БД завершено за {timer.Elapsed.TotalSeconds} c.");
            }
        }

        private void ConvertData()
        {
            _Logger.LogInformation("---> Исправление данных...");

            var timer = Stopwatch.StartNew();

            var sections = TestData.Sections.ToDictionary(s => s.Id);
            var brands = TestData.Brands.ToDictionary(b => b.Id);
            
            foreach (var section in TestData.Sections)
            {
                section.Id = 0;

                if (section.ParentId is { } parentId)
                {
                    section.Parent = sections[parentId];
                    section.ParentId = null;
                }
            }
            
            foreach (var product in TestData.Products)
            {
                product.Id = 0;
                product.Section = sections[product.SectionId];
                product.SectionId = 0;

                if (product.BrandId is { } brandId)
                {
                    product.Brand = brands[brandId];
                    product.BrandId = null;
                }
            }
            
            foreach (var brand in TestData.Brands) 
                brand.Id = 0;
            
            _Logger.LogInformation($"---> Исправление данных завершено за {timer.Elapsed.TotalSeconds} c.");
        }
    }
}
