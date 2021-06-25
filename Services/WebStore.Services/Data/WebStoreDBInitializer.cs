using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _Db;
        private readonly ILogger<WebStoreDBInitializer> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;

        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer> logger,
            UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _Db = db;
            _Logger = logger;
            _UserManager = userManager;
            _RoleManager = roleManager;
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

            try
            {
                InitializeIdentityAsync().Wait();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "---> Ошибка инициализации системы Identity");
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

        private async Task InitializeIdentityAsync()
        {
            async Task CheckRole(string role)
            {
                if (!await _RoleManager.RoleExistsAsync(role))
                {
                    _Logger.LogWarning($"Роль {role} отсутствует.");
                    await _RoleManager.CreateAsync(new Role { Name = role });
                    _Logger.LogInformation($"Роль {role} успешно создана.");
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogWarning($"Пользователь {User.Administrator} не найден.");

                var admin = new User { UserName = User.Administrator };

                var result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);

                if (result.Succeeded)
                {
                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);
                    _Logger.LogInformation($"Пользователь {admin.UserName} успешно создан и наделен правами {Role.Administrators}");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors);

                    _Logger.LogError(errors);
                    throw new InvalidOperationException(errors);
                }
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
