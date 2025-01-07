using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTW_TEST_PATTERNS.Models.DTOS
{
    public class GithubUserInfo
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Avatar_Url { get; set; }
        public string Email { get; set; }
    }
}
