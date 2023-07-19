namespace APICatalago.DTOs;

public class UsuarioDTO
{
    public int Id { get; internal  set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public int MinisterioId { get; set; }
    public int IgrejaId { get; set; }
    public string? UserName { get;  set; }
    public string? NormalizedUserName { get;  set; }
    public string? PhoneNumber { get; set; }
    
}
