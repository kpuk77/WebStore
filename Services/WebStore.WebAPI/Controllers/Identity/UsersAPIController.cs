using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using WebStore.DAL.Context;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(APIAddress.Identity.USERS)]
    public class UsersAPIController : ControllerBase
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

        [HttpPost("GetId")]
        public async Task<string> GetUserIdAsync([FromBody] User user) =>
            await _UserStore.GetUserIdAsync(user);

        [HttpPost("GetName")]
        public async Task<string> GetUserNameAsync([FromBody] User user) => await _UserStore.GetUserNameAsync(user);

        [HttpPost("SetName/{userName}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string userName)
        {
            await _UserStore.SetUserNameAsync(user, userName);
            await _UserStore.UpdateAsync(user);
            return user.UserName;
        }

        [HttpPost("GetNormalizedName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] User user) =>
            await _UserStore.GetNormalizedUserNameAsync(user);

        [HttpPost("SetNormalizedName/{normalizedName}")]
        public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string normalizedName)
        {
            await _UserStore.SetNormalizedUserNameAsync(user, normalizedName);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedUserName;
        }

        [HttpPost("Create")]
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            var result = await _UserStore.CreateAsync(user);
            return result.Succeeded;
        }

        [HttpPut("Update")]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var result = await _UserStore.UpdateAsync(user);
            return result.Succeeded;
        }

        [HttpPost("Delete")]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            var result = await _UserStore.DeleteAsync(user);
            return result.Succeeded;
        }

        [HttpGet("FindById/{userId}")]
        public async Task<User> FindByIdAsync(string userId) => await _UserStore.FindByIdAsync(userId);

        [HttpGet("FindByName/{normalizedUserName}")]
        public async Task<User> FindByNameAsync(string normalizedUserName) =>
            await _UserStore.FindByNameAsync(normalizedUserName);

        [HttpPost("AddToRole/{roleName}")]
        public async Task AddToRoleAsync([FromBody] User user, string roleName)
        {
            await _UserStore.AddToRoleAsync(user, roleName);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("RemoveFromRole/{roleName}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string roleName)
        {
            await _UserStore.RemoveFromRoleAsync(user, roleName);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("GetRolesAsync")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user) => await _UserStore.GetRolesAsync(user);

        [HttpPost("IsInRole/{roleName}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string roleName) =>
            await _UserStore.IsInRoleAsync(user, roleName);

        [HttpGet("GetInRole/{roleName}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string roleName) =>
            await _UserStore.GetUsersInRoleAsync(roleName);

        [HttpPost("SetPassword")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO passwordHash)
        {
            await _UserStore.SetPasswordHashAsync(passwordHash.User, passwordHash.Hash);
            await _UserStore.UpdateAsync(passwordHash.User);
            return passwordHash.User.PasswordHash;
        }

        [HttpPost("GetPasswordHashAsync")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user) =>
            await _UserStore.GetPasswordHashAsync(user);

        [HttpPost("HasPasswordAsync")]
        public async Task<bool> HasPasswordAsync([FromBody] User user) => await _UserStore.HasPasswordAsync(user);

        [HttpPost("SetEmail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _UserStore.SetEmailAsync(user, email);
            await _UserStore.UpdateAsync(user);
            return user.Email;
        }

        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] User user) => await _UserStore.GetEmailAsync(user);

        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user) =>
            await _UserStore.GetEmailConfirmedAsync(user);

        [HttpPost("SetEmailConfirmed/{confirmed}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _UserStore.SetEmailConfirmedAsync(user, confirmed);
            await _UserStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        [HttpPost("FindByEmail/{normalizedEmail}")]
        public async Task<User> FindByEmailAsync(string normalizedEmail) =>
            await _UserStore.FindByEmailAsync(normalizedEmail);

        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user) =>
            await _UserStore.GetNormalizedEmailAsync(user);

        [HttpPost("SetNormalizedEmail/{normalizedEmail}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string normalizedEmail)
        {
            await _UserStore.SetNormalizedEmailAsync(user, normalizedEmail);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        [HttpPost("SetPhoneNumber/{phoneNumber}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phoneNumber)
        {
            await _UserStore.SetPhoneNumberAsync(user, phoneNumber);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user) =>
            await _UserStore.GetPhoneNumberAsync(user);

        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user) =>
            await _UserStore.GetPhoneNumberConfirmedAsync(user);

        [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _UserStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        [HttpPost("SetTwoFactorEnabled/{enabled}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enabled)
        {
            await _UserStore.SetTwoFactorEnabledAsync(user, enabled);
            await _UserStore.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user) =>
            await _UserStore.GetTwoFactorEnabledAsync(user);

        [HttpPost("AddLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO login)
        {
            await _UserStore.AddLoginAsync(login.User, login.LoginInfo);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("RemoveLoginAsync/{loginProvider}")]
        public async Task RemoveLoginAsync([FromBody] User user, string loginProvider, string providerKey)
        {
            await _UserStore.RemoveLoginAsync(user, loginProvider, providerKey);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("GetLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user) =>
            await _UserStore.GetLoginsAsync(user);

        [HttpGet("FindByLogin/{loginProvider}/{providerKey}")]
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey) =>
            await _UserStore.FindByLoginAsync(loginProvider, providerKey);

        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user) => await _UserStore.GetClaimsAsync(user);

        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO addClaim)
        {
            await _UserStore.AddClaimsAsync(addClaim.User, addClaim.Claims);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO replaceClaim)
        {
            await _UserStore.ReplaceClaimAsync(replaceClaim.User, replaceClaim.Claim, replaceClaim.NewClaim);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("RemoveClaims")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO removeClaim)
        {
            await _UserStore.RemoveClaimsAsync(removeClaim.User, removeClaim.Claims);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("GetUsersForClaim")]
        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim) =>
            await _UserStore.GetUsersForClaimAsync(claim);
    }
}