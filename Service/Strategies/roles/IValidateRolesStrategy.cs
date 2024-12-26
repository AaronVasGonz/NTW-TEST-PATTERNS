using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Strategies.userRoles;

public interface IValidateRolesStrategy
    {
        bool Validate(List<int> roleIds);
    }

