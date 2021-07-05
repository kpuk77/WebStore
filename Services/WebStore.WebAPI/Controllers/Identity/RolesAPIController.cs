using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(APIAddress.Identity.ROLES)]
    public class RolesAPIController : ControllerBase
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

        [HttpPost]
        public async Task<IdentityResult> CreateAsync(Role role) =>
            await _RoleStore.CreateAsync(role);

        [HttpPut]
        public async Task<IdentityResult> UpdateAsync(Role role) =>
            await _RoleStore.UpdateAsync(role);

        [HttpPost("Delete")]
        public async Task<bool> DeleteAsync(Role role)
        {
            var result = await _RoleStore.DeleteAsync(role);
            return result.Succeeded;
        }

        [HttpPost("GetId")]
        public async Task<string> GetRoleIdAsync(Role role) =>
            await _RoleStore.GetRoleIdAsync(role);

        [HttpPost("GetName")]
        public async Task<string> GetRoleNameAsync(Role role) =>
            await _RoleStore.GetRoleNameAsync(role);

        [HttpPost("SetName/{roleName}")]
        public async Task<string> SetRoleNameAsync(Role role, string roleName)
        {
            await _RoleStore.SetRoleNameAsync(role, roleName);
            await _RoleStore.UpdateAsync(role);
            return role.Name;
        }

        [HttpPost("GetNormalizedName")]
        public async Task<string> GetNormalizedRoleNameAsync(Role role) =>
            await _RoleStore.GetNormalizedRoleNameAsync(role);

        [HttpPost("SetNormalizedName/{normalizedName}")]
        public async Task<string> SetNormalizedRoleNameAsync(Role role, string normalizedName)
        {
            await _RoleStore.SetNormalizedRoleNameAsync(role, normalizedName);
            await _RoleStore.UpdateAsync(role);
            return role.NormalizedName;

        }

        [HttpGet("FindById/{roleId}")]
        public async Task<Role> FindByIdAsync(string roleId) =>
            await _RoleStore.FindByIdAsync(roleId);

        [HttpGet("FindByName/{normalizedRoleName}")]
        public async Task<Role> FindByNameAsync(string normalizedRoleName) =>
            await _RoleStore.FindByNameAsync(normalizedRoleName);
    }
}