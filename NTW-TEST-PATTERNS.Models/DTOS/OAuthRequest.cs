using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTW_TEST_PATTERNS.Models.DTOS
{
    public class OAuthRequest
    {
        public string? Token { get; set; }
        public string? Code { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string?  Provider { get; set; }
    }
}
