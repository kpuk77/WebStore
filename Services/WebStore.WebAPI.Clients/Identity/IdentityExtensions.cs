﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

using WebStore.Domain.Entities.Identity;

namespace WebStore.WebAPI.Clients.Identity
{
    public static class IdentityExtensions
    {
        private static IServiceCollection AddIdentityWebStoreAPIClients(this IServiceCollection services)
        {
            services.AddHttpClient("WebStoreAPI", (s, opt) => opt.BaseAddress = new Uri(s.GetRequiredService<IConfiguration>()["WebAPI"]))
                .AddTypedClient<IUserStore<User>, UsersClient>()
                .AddTypedClient<IUserRoleStore<User>, UsersClient>()
                .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
                .AddTypedClient<IUserEmailStore<User>, UsersClient>()
                .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
                .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
                .AddTypedClient<IUserClaimStore<User>, UsersClient>()
                .AddTypedClient<IUserLoginStore<User>, UsersClient>()
                .AddTypedClient<IRoleStore<Role>, RolesClient>();

            return services;
        }

        public static IdentityBuilder AddIdentityWebStoreAPIClients(this IdentityBuilder builder)
        {
            builder.Services.AddIdentityWebStoreAPIClients();

            return builder;
        }
    }
}
