namespace APICatalago.DTOs;

public class MinisterioDTO
{
    public int MinisterioId { get; set; }
    public string? Nome { get; set; }
    public ICollection<IgrejaDTO>? Igrejas { get; set; }
}
