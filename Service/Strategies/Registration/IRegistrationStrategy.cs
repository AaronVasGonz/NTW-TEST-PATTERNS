using Models;
using Models.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Strategies.Authentication;
public interface IRegistrationStrategy
{
    // This method could be for the normal registration flow
    Task<User> RegisterUser(UserRegistrationData userRegistrationData);

    // Method to validate registration data (such as email verification or format)
    bool ValidateUser(UserRegistrationData userRegistrationData);

    // Method to handle the registration flow through OAuth
    Task<User> RegisterUserWithOAuth(string oauthToken);
}

