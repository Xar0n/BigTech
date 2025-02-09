namespace BigTech.Domain.Dto.User;
public record LoginUserDto
{
    public string Login {  get; set; }
    public string Password { get; set; }
}
