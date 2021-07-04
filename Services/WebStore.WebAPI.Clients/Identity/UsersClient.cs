using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(HttpClient client) : base(client, APIAddress.Identity.USERS) { }

        public void Dispose()
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/UserId", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/UserName", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/UserName/{userName}", user, cancel);
            user.UserName = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Normal", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Normal/{normalizedName}", user, cancel);
            user.NormalizedUserName = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User", user, cancel);
            var result = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            var response = await PutAsync($"{Address}/User", user, cancel);
            var result = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Delete", user, cancel);
            var result = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/User/{userId}", cancel).ConfigureAwait(false);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/User/{normalizedUserName}", cancel).ConfigureAwait(false);
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            await PostAsync($"{Address}/User/Role/{roleName}", user, cancel).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            await PostAsync($"{Address}/User/Role/Delete/{roleName}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Role/GetAll", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<List<string>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/In/{roleName}", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{Address}/Role/{roleName}/Users", cancel).ConfigureAwait(false);
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Hash",
                new PasswordHashDTO { User = user, Hash = passwordHash }, cancel);

            user.PasswordHash = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/HashGet", user, cancel);

            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Has/", user, cancel);

            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Email/{email}", user, cancel);
            user.Email = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Email", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/ConfirmedEmail", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Set/{confirmed}", user, cancel);
            user.EmailConfirmed = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/UserByEmail/{normalizedEmail}", cancel).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/NormalEmail", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Email/Set/{normalizedEmail}", user, cancel);
            user.NormalizedEmail = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Phone/{phoneNumber}", user, cancel);
            user.PhoneNumber = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Phone/", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/ConfirmedPhone", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/Phone/{confirmed}", user, cancel);
            user.PhoneNumberConfirmed = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/User/SetTwoFactorAuth/{enabled}", user, cancel);
            user.TwoFactorEnabled = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/")
        }
    }
}
