using Data.Repository;
using Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IUserRoleService
{
    Task<IEnumerable<UserRole>> GetUserRoles();
    Task<UserRole> SaveUserRole(int userId, int roleId);
}

public class UserRoleService : IUserRoleService

{
    private readonly IUserRoleRepository _userRoleRepository;
    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }

    public async Task<IEnumerable<UserRole>> GetUserRoles()
    {
        return await _userRoleRepository.GetUserRolesAsync();
    }

    public async Task<UserRole> SaveUserRole(int userId, int roleId)
    {
        return await _userRoleRepository.SaveUserRoleAsync(userId, roleId);
    }
}
