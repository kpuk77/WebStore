﻿using Microsoft.AspNetCore.Identity;

using System;

using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.DTO.Identity
{
    public abstract class UserDTO
    {
        public User User { get; set; }
    }

    public class AddLoginDTO : UserDTO
    {
        public UserLoginInfo LoginInfo { get; set; }
    }

    public class PasswordHashDTO : UserDTO
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}