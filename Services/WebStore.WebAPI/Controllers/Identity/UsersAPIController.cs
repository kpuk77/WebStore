using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.WebAPI.Controllers.Identity
{
    public class UsersAPIController
    {
        private readonly WebStoreDB _DB;
        private readonly UserStore<User, Role, WebStoreDB> _UserStore;

        public UsersAPIController(WebStoreDB db)
        {
            _DB = db;
            _UserStore = new UserStore<User, Role, WebStoreDB>(db);
        }

        [HttpGet("users/all")]
        public async Task<IEnumerable<User>> GetAllUsers() => await _UserStore.Users.ToArrayAsync();
    }
}
