using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace Strategies.userRoles;

public interface IAssignRolesStrategy
    {
       User AssignRolesToUser(User user, IEnumerable<Role> roles, List<int> roleIds);
    }

