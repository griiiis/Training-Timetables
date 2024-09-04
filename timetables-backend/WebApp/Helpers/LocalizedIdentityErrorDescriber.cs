using Microsoft.AspNetCore.Identity;

namespace WebApp.Helpers;

public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError()
    {
        return new IdentityError()
        {
            Code = nameof(DefaultError),
            Description = Base.Resources.Identity.DefaultError
        };
    }

    public override IdentityError ConcurrencyFailure()
    {
        return new IdentityError
        {
            Code = nameof(ConcurrencyFailure),
            Description = Base.Resources.Identity.ConcurrencyFailure
        };
    }

    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = nameof(PasswordMismatch),
            Description = Base.Resources.Identity.PasswordMismatch
        };
    }

    public override IdentityError InvalidToken()
    {
        return new IdentityError
        {
            Code = nameof(InvalidToken),
            Description = Base.Resources.Identity.InvalidToken
        };
    }

    public override IdentityError LoginAlreadyAssociated()
    {
        return new IdentityError
        {
            Code = nameof(LoginAlreadyAssociated),
            Description = Base.Resources.Identity.LoginAlreadyAssociated
        };
    }

    public override IdentityError InvalidUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = String.Format(Base.Resources.Identity.InvalidUserName, userName)
        };
    }

    public override IdentityError InvalidEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = String.Format(Base.Resources.Identity.InvalidEmail, email)
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = String.Format(Base.Resources.Identity.DuplicateUserName, userName)
        };
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateEmail), 
            Description = String.Format(Base.Resources.Identity.DuplicateEmail, email)
        };
    }

    public override IdentityError InvalidRoleName(string role)
    {
        return new IdentityError
        {
            Code = nameof(InvalidRoleName), 
            Description = String.Format(Base.Resources.Identity.InvalidRoleName, role)
        };
    }

    public override IdentityError DuplicateRoleName(string role)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateRoleName), 
            Description = String.Format(Base.Resources.Identity.DuplicateRoleName, role)
        };
    }

    public override IdentityError UserAlreadyHasPassword()
    {
        return new IdentityError
        {
            Code = nameof(UserAlreadyHasPassword), 
            Description = Base.Resources.Identity.UserAlreadyHasPassword
        };
    }

    public override IdentityError UserLockoutNotEnabled()
    {
        return new IdentityError
        {
            Code = nameof(UserLockoutNotEnabled), 
            Description = Base.Resources.Identity.UserLockoutNotEnabled
        };
    }

    public override IdentityError UserAlreadyInRole(string role)
    {
        return new IdentityError
        {
            Code = nameof(UserAlreadyInRole), 
            Description = String.Format(Base.Resources.Identity.UserAlreadyInRole, role)
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(PasswordTooShort), 
            Description = String.Format(Base.Resources.Identity.UserAlreadyInRole, length)
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = Base.Resources.Identity.PasswordRequiresNonAlphanumeric
        };
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresDigit), 
            Description = Base.Resources.Identity.PasswordRequiresDigit
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresLower), 
            Description = Base.Resources.Identity.PasswordRequiresLower
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUpper), 
            Description = Base.Resources.Identity.PasswordRequiresUpper
        };
    }
}