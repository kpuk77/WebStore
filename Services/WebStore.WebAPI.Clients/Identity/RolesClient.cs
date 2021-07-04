using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
    public class RolesClient : BaseClient, IRolesClient
    {
        public RolesClient(HttpClient client) : base(client, APIAddress.Identity.ROLES) { }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(Role role, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }
    }
}
