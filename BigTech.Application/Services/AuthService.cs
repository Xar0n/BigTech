using AutoMapper;
using BigTech.Application.Resources;
using BigTech.Domain.Dto;
using BigTech.Domain.Dto.User;
using BigTech.Domain.Entity;
using BigTech.Domain.Enum;
using BigTech.Domain.Interfaces.Repositories;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BigTech.Application.Services;
public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserToken> _userTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public AuthService(IBaseRepository<User> userRepository,
        IBaseRepository<UserToken> userTokenRepository,
        ITokenService tokenService,
        ILogger logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _userTokenRepository = userTokenRepository;
        _tokenService = tokenService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
    {
        try
        {
            var users = await _userRepository.GetAll().ToListAsync();
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Login == dto.Login);
            if (user == null)
            {
                return new BaseResult<TokenDto>
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }
            if (!IsVerifyPassword(user.Password, dto.Password))
            {
                return new BaseResult<TokenDto>
                {
                    ErrorMessage = ErrorMessage.PasswordIsWrong,
                    ErrorCode = (int)ErrorCodes.PasswordIsWrong
                };
            }
            var userToken = await _userTokenRepository.GetAll()
                .FirstOrDefaultAsync(ut => ut.UserId == user.Id);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, "User")
            };
            var acessToken = _tokenService.GenerateAcessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            if (userToken == null)
            {
                userToken = new UserToken
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
                };
                await _userTokenRepository.CreateAsync(userToken);
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _userTokenRepository.UpdateAsync(userToken);
            }

            return new BaseResult<TokenDto>
            {
                Data = new TokenDto()
                {
                    AcessToken = acessToken,
                    RefreshToken = refreshToken
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return new BaseResult<TokenDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
    {
        if (dto.Password != dto.PasswordConfirm)
        {
            return new BaseResult<UserDto>()
            {
                ErrorMessage = ErrorMessage.PaswordNotEqulsPasswordConfirm,
                ErrorCode = (int)ErrorCodes.PaswordNotEqulsPasswordConfirm
            };
        }

        try
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Login == dto.Login);
            if (user != null)
            {
                return new BaseResult<UserDto>
                {
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                    ErrorCode = (int)ErrorCodes.UserAlreadyExists
                };
            }
            var hashUserPassword = HashPassword(dto.Password);
            user = new User()
            {
                Login = dto.Login,
                Password = hashUserPassword,
            };
            await _userRepository.CreateAsync(user);
            return new BaseResult<UserDto>
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return new BaseResult<UserDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool IsVerifyPassword(string userPasswordHash, string userPassword)
    {
        var hash = HashPassword(userPassword);
        return hash == userPasswordHash;
    }
}
