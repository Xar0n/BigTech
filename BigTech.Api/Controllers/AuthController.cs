using BigTech.Application.Services;
using BigTech.Domain.Dto;
using BigTech.Domain.Dto.User;
using BigTech.Domain.Entity;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigTech.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<BaseResult>> Register([FromBody] RegisterUserDto dto)
    {
        var response = await _authService.Register(dto);
        if (response.IsSuccess)
            return Ok(response);
        return BadRequest(response);
    }

    /// <summary>
    /// Логин пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto dto)
    {
        var response = await _authService.Login(dto);
        if (response.IsSuccess)
            return Ok(response);
        return BadRequest(response);
    }
}
