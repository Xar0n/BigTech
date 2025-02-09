using BigTech.Application.Services;
using BigTech.Domain.Dto;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace BigTech.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public  TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto dto)
    {
        var response = await _tokenService.RefreshToken(dto);
        if (response.IsSuccess)
            return Ok(response);
        return BadRequest(response);
    }
}
