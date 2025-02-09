using BigTech.Domain.Dto.Role;
using BigTech.Domain.Dto.UserRole;
using BigTech.Domain.Entity;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BigTech.Api.Controllers;
[Consumes(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Создание роли
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    ///
    ///     POST
    ///     {
    ///        "name": "User",
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если роль создалась</response>
    /// <response code="400">Если роль не была создана</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<RoleDto>>> Create([FromBody] CreateRoleDto dto)
    {
        var response = await _roleService.CreateRoleAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Удаление роли с указанием идентификатора
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE
    ///     {
    ///        "id": "1",
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если роль удалилась</response>
    /// <response code="400">Если роль не был удалена</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<RoleDto>>> Delete(long id)
    {
        var response = await _roleService.DeleteRoleAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Обновление роли с указанием основных свойств
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT
    ///     {
    ///        "id": 1
    ///        "name": "Admin",
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если роль обновилась</response>
    /// <response code="400">Если роль не была обновлена</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<RoleDto>>> Update([FromBody] RoleDto dto)
    {
        var response = await _roleService.UpdateRoleAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Добавление роли пользователю
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for add role for user:
    ///
    ///     POST
    ///     {
    ///        "login": "User #1",
    ///        "roleName": "Admin"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если роль была добавлена</response>
    /// <response code="400">Если роль не была добавлена</response>
    [HttpPost("add-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> AddRoleForUser([FromBody] UserRoleDto dto)
    {
        var response = await _roleService.AddRoleForUserAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Удаление роли у пользователя
    /// </summary>
    [HttpDelete("delete-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> DeleteRoleForUser([FromBody] DeleteUserRoleDto dto)
    {
        var response = await _roleService.DeleteRoleForUserAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Удаление роли у пользователя
    /// </summary>
    [HttpPut("update-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> UpdateRoleForUser([FromBody] UpdateUserRoleDto dto)
    {
        var response = await _roleService.UpdateRoleForUserAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
