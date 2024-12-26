using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOS;

public class UserRegistrationData
{   
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ? OAuthToken { get; set; } //If the user is registered with OAuth   
}
