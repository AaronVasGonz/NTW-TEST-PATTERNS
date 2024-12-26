using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategies.userRoles;

public interface IRoleAssigmentContext
{
    User AssignRolesToUser(User user, IEnumerable<Role> roles, List<int> roleIds);
}

//Context class that uses the strategy
public class RoleAssigmentContext : IRoleAssigmentContext
{
    private readonly IAssignRolesStrategy _assignRolesStrategy;

    //Contructor acepts a strategy as a parameter or uses the default strategy
    public RoleAssigmentContext(IAssignRolesStrategy? assignRolesStrategy = null)
    {
        _assignRolesStrategy = assignRolesStrategy ?? new DefaultAssignRolesStrategy();
    }

    public User AssignRolesToUser(User user, IEnumerable<Role> roles, List<int> roleIds)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (roles == null) throw new ArgumentNullException(nameof(roles));
        if (roleIds == null || roleIds.Count == 0) throw new ArgumentException("No role IDs provided.", nameof(roleIds));

        return _assignRolesStrategy.AssignRolesToUser(user, roles, roleIds);
    }
}


