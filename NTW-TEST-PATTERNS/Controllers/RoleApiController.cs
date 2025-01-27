using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using Service.Services;
using Service.Services.validations.Roles;
namespace NTW_TEST_PATTERNS.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoleApiController(IRoleService roleService, IValidateRoleService validateRoleService) : Controller
{
    private readonly IRoleService _roleService = roleService;
    private readonly IValidateRoleService _validateRoleService = validateRoleService;

    [HttpGet("all")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleService.GetRolesAsync();
        return roles == null ? throw new KeyNotFoundException("Roles not found") : (IActionResult)Ok(roles);
    }

    [HttpGet("role/{id}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        return role == null ? throw new KeyNotFoundException("Role not found") : (IActionResult)Ok(role);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveRole(RoleRequest roleRequest)
    {
        _validateRoleService.ValidateRoleRequest(roleRequest);

        //create new Role Instance
        var role = new Role
        {
            RoleName = roleRequest.RoleName,
            Description = roleRequest.Description,
        };

        var savedRole = await _roleService.SaveRoleAsync(role);

        return savedRole == null ? throw new Exception("Error ocurred while saving the role") : (IActionResult)Ok(savedRole);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateRole(RoleRequest roleRequest)
    {
        if (roleRequest.Id == null)
            throw new KeyNotFoundException("Role Id is required");

        _validateRoleService.ValidateRoleRequest(roleRequest);
        var role = await _roleService.GetRoleByIdAsync(roleRequest.Id ?? 0);
        if (role == null)
            throw new KeyNotFoundException("Role not found");

        role.RoleName = roleRequest.RoleName;
        role.Description = roleRequest.Description;

        var updatedRole = await _roleService.SaveRoleAsync(role);
        return updatedRole == null ? throw new Exception("Error ocurred while updating the role") : (IActionResult)Ok(updatedRole);
    }
}
