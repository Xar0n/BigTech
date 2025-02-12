using AutoMapper;
using BigTech.Application.Resources;
using BigTech.Domain.Dto;
using BigTech.Domain.Dto.User;
using BigTech.Domain.Entity;
using BigTech.Domain.Enum;
using BigTech.Domain.Interfaces.Databases;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserToken> _userTokenRepository;
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public AuthService(
        IUnitOfWork unitOfWork,
        IBaseRepository<User> userRepository,
        IBaseRepository<UserToken> userTokenRepository,
        IBaseRepository<Role> roleRepository,
        IBaseRepository<UserRole> userRoleRepository,
        ITokenService tokenService,
        ILogger logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userTokenRepository = userTokenRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _tokenService = tokenService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
    {

        var user = await _userRepository.GetAll()
            .Include(u => u.Roles)
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


        var userRoles = user.Roles;
        var claims = userRoles?.Select(ur => new Claim(ClaimTypes.Role, ur.Name)).ToList();
        claims.Add(new Claim(ClaimTypes.Name, user.Login));
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

            _userTokenRepository.Update(userToken);
        }

        await _unitOfWork.SaveChangesAsync();

        return new BaseResult<TokenDto>
        {
            Data = new TokenDto()
            {
                AcessToken = acessToken,
                RefreshToken = refreshToken
            }
        };
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

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    user = new User()
                    {
                        Login = dto.Login,
                        Password = hashUserPassword,
                    };
                    await _unitOfWork.Users.CreateAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    var role = await _unitOfWork.Roles.GetAll().FirstOrDefaultAsync(r => r.Name == nameof(Roles.User));
                    if (role == null)
                    {
                        return new BaseResult<UserDto>
                        {
                            ErrorMessage = ErrorMessage.RoleNotFound,
                            ErrorCode = (int)ErrorCodes.RoleNotFound
                        };
                    }

                    UserRole userRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                    };

                    await _unitOfWork.UserRoles.CreateAsync(userRole);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }


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
