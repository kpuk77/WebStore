using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

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
        
        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetId", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetName", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetName/{userName}", user, cancel);
            user.UserName = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetNormalizedName", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetNormalizedName/{normalizedName}", user, cancel);
            user.NormalizedUserName = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Create", user, cancel);
            var result = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            var response = await PutAsync($"{Address}/Update", user, cancel);
            var result = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Delete", user, cancel);
            var result = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/FindById/{userId}", cancel).ConfigureAwait(false);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/FindByName/{normalizedUserName}", cancel).ConfigureAwait(false);
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            await PostAsync($"{Address}/AddToRole/{roleName}", user, cancel).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            await PostAsync($"{Address}/RemoveFromRole/{roleName}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetRolesAsync", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<List<string>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/IsInRole/{roleName}", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{Address}/GetInRole/{roleName}", cancel).ConfigureAwait(false);
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPassword",
                new PasswordHashDTO { User = user, Hash = passwordHash }, cancel);

            user.PasswordHash = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPasswordHashAsync", user, cancel);

            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/HasPasswordAsync", user, cancel);

            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetEmail/{email}", user, cancel);
            user.Email = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetEmail/", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetEmailConfirmed", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetEmailConfirmed/{confirmed}", user, cancel);
            user.EmailConfirmed = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/FindByEmail/{normalizedEmail}", cancel).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetNormalizedEmail", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetNormalizedEmail/{normalizedEmail}", user, cancel);
            user.NormalizedEmail = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumber/{phoneNumber}", user, cancel);
            user.PhoneNumber = await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumber/", user, cancel);
            return await response.Content
                .ReadAsStringAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumberConfirmed", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumberConfirmed/{confirmed}", user, cancel);
            user.PhoneNumberConfirmed = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetTwoFactorEnabled/{enabled}", user, cancel);
            user.TwoFactorEnabled = await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetTwoFactorEnabled/", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{Address}/AddLogin",
                new AddLoginDTO { User = user, LoginInfo = login }, cancel).ConfigureAwait(false);
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancel)
        {
            await PostAsync($"{Address}/RemoveLoginAsync/{loginProvider}/{providerKey}", user, cancel)
                .ConfigureAwait(false);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetLogins", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<List<UserLoginInfo>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/FindByLogin/{loginProvider}/{providerKey}", cancel)
                .ConfigureAwait(false);
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetClaims", user, cancel);
            return await response.Content
                .ReadFromJsonAsync<List<Claim>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{Address}/AddClaims", new AddClaimDTO { User = user, Claims = claims },
                cancel).ConfigureAwait(false);
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancel)
        {
            await PostAsync($"{Address}/ReplaceClaim",
                new ReplaceClaimDTO { User = user, Claim = claim, NewClaim = newClaim }, cancel).ConfigureAwait(false);

        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{Address}/RemoveClaims", new RemoveClaimDTO { User = user, Claims = claims },
                    cancel).ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetUsersForClaim", claim, cancel);
            return await response.Content
                .ReadFromJsonAsync<List<User>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }
    }
}
