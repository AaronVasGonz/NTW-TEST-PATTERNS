using Models.DTOS;
using NTW_TEST_PATTERNS.Models.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Strategies.Login;
 public interface ILoginStrategy
{
    Task<string> LoginUser(UserLoginRequest userLoginRequest);
    bool ValidateUser(UserLoginRequest userLoginRequest);
    Task<string> LoginUserWithOAuth(OAuthRequest oAuthRequest);
}
