using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTOS;
namespace Service.Mappers;

public interface IUserMapper
{
    User MapUserFromUserRegistrationData(UserRegistrationData userRegistrationData);
}

public class UserMapper : IUserMapper
{
    public User MapUserFromUserRegistrationData(UserRegistrationData userRegistrationData)
    {
        if (userRegistrationData == null)
        {
            throw new ArgumentNullException(nameof(userRegistrationData), "User registration data cannot be null");
        }

        return new User
        {
            Username = userRegistrationData.UserName,
            Email = userRegistrationData.Email,
            PasswordHash = userRegistrationData.Password,
            CreatedAt = DateTime.Now,
        };
    }
}

