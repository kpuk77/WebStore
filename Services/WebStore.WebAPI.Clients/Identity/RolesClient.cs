using System.Net.Http;
using System.Net.Http.Json;
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

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync(Address, role, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancel)
        {
            var response = await PutAsync(Address, role, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Delete", role, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetId", role, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetName", role, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetName/{roleName}", role, cancel);
            role.Name = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetNormalizedName", role, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetNormalizedName/{normalizedName}", role, cancel);
            role.NormalizedName = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancel)
        {
            return await GetAsync<Role>($"{Address}/FindById/{roleId}", cancel).ConfigureAwait(false);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancel)
        {
            return await GetAsync<Role>($"{Address}/FindByName/{normalizedRoleName}", cancel).ConfigureAwait(false);
        }
    }
}