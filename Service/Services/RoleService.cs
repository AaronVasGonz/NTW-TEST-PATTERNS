using Data.Repository;
using Models;

namespace Service.Services;

public interface IRoleService
{
    Task<bool> DeleteRoleAsync(int id);
    Task<Role> GetRoleByIdAsync(int id);
    Task<IEnumerable<Role>> GetRolesAsync();
    Task<Role> SaveRoleAsync(Role role);
}

public class RoleService : IRoleService
{

    private readonly IRoleRepository _roleRepository;
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<Role>> GetRolesAsync()
    {
        return await _roleRepository.GetRolesAsync();
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        return await _roleRepository.GetRoleByIdAsync(id);
    }

    public async Task<Role> SaveRoleAsync(Role role)
    {
        return await _roleRepository.SaveRoleAsync(role);
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        return await _roleRepository.DeleteRoleAsync(id);
    }
}

