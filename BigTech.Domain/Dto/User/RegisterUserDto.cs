

namespace BigTech.Domain.Dto.User;
public record RegisterUserDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}
