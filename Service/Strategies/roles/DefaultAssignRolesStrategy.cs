using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategies.userRoles;

public class DefaultAssignRolesStrategy : IAssignRolesStrategy
{
    public User AssignRolesToUser(User user, IEnumerable<Role> roles, List<int> roleIds)
    {
        var selectedRoles = roles.Where(r => roleIds.Contains(r.RoleId)).ToList();
        user.Roles = selectedRoles;
        return user;
    }
}
