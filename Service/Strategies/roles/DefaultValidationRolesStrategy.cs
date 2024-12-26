
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategies.userRoles;

    public class DefaultValidationRolesStrategy: IValidateRolesStrategy
    {
        public bool Validate(List<int> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                return false;
            }
            return true;
        }
    }

