using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.WebAPI.Controllers.Identity
{
    public class RolesAPIController
    {
        private readonly WebStoreDB _DB;
        private readonly RoleStore<Role> _RoleStore;
        public RolesAPIController(WebStoreDB db)
        {
            _DB = db;
            _RoleStore = new RoleStore<Role>(db);
        }

        [HttpGet("roles/all")]
        public async Task<IEnumerable<Role>> GetAllRoles() => await _RoleStore.Roles.ToArrayAsync();
    }
}
