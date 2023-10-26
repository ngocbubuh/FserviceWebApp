using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Helper
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        //private readonly IStringLocalizer<LocalizedIdentityErrorDescriber> _localizer;

        //public LocalizedIdentityErrorDescriber(IStringLocalizer<LocalizedIdentityErrorDescriber> localizer)
        //{
        //    _localizer = localizer;
        //}

        public override IdentityError DefaultError() => new IdentityError
        {
            Code = nameof(DefaultError),
            Description = "Unknown Error!"
        };

        public override IdentityError ConcurrencyFailure() =>
            new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = "Concurrency Failure!"
            };

        public override IdentityError PasswordMismatch() => new IdentityError
        {
            Code = nameof(PasswordMismatch),
            Description = "Password mismatch!"
        };

        public override IdentityError InvalidToken() => new IdentityError
        {
            Code = nameof(InvalidToken),
            Description = "Invalid Token!"
        };

        public override IdentityError LoginAlreadyAssociated() => new IdentityError
        {
            Code = nameof(LoginAlreadyAssociated),
            Description = "You already login!"
        };

        public override IdentityError InvalidUserName(string userName) => new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = $"Invalid username: {userName}!"
        };

        public override IdentityError InvalidEmail(string email) => new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = $"Invalid email: {email}!"
        };

        public override IdentityError DuplicateUserName(string userName) => new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = $"Duplicate username: {userName}!"
        };

        public override IdentityError DuplicateEmail(string email) => new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = $"Duplicate email: {email}!"
        };

        public override IdentityError InvalidRoleName(string role) => new IdentityError
        {
            Code = nameof(InvalidRoleName),
            Description = $"Invalid role name: {role}!"
        };

        public override IdentityError DuplicateRoleName(string role) => new IdentityError
        {
            Code = nameof(DuplicateRoleName),
            Description = $"Duplicate role name: {role}!"
        };

        public override IdentityError UserAlreadyHasPassword() => new IdentityError
        {
            Code = nameof(UserAlreadyHasPassword),
            Description = "User already has password!"
        };

        public override IdentityError UserLockoutNotEnabled() => new IdentityError
        {
            Code = nameof(UserLockoutNotEnabled),
            Description = "User lockout not enabled!"
        };

        public override IdentityError UserAlreadyInRole(string role) => new IdentityError
        {
            Code = nameof(UserAlreadyInRole),
            Description = "User already in role!"
        };

        public override IdentityError UserNotInRole(string role) => new IdentityError
        {
            Code = nameof(UserNotInRole),
            Description = $"User not in role: {role}"
        };

        public override IdentityError PasswordTooShort(int length) => new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = "Password too short!"
        };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = "Password requires unique character!"
        };

        public override IdentityError PasswordRequiresDigit() => new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = "Password requires number character!"
        };

        public override IdentityError PasswordRequiresLower() => new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = "Password requires lowercase character!",
        };

        public override IdentityError PasswordRequiresUpper() => new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "Password requires uppercase character!",
        };
    }
}
