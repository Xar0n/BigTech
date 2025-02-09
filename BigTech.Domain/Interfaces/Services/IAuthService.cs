using BigTech.Domain.Dto;
using BigTech.Domain.Dto.User;
using BigTech.Domain.Result;

namespace BigTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис предназначенный для авторизации/регистрации
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<BaseResult<UserDto>> Register(RegisterUserDto dto);

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<BaseResult<TokenDto>> Login(LoginUserDto dto);
}
