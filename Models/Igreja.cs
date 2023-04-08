using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalago.Models;

[Table("Igreja")]
public class Igreja
{

    public Igreja()
    {
        Membros = new Collection<ApplicationUser>();
    }


    [Key]
    public int IgrejaId { get; set; }

    [Required(ErrorMessage ="O nome é obrigatorio")]
    [StringLength(80, ErrorMessage ="O nome deve ter no maximo {1} e no minimo {2} caracteres",
        MinimumLength = 5)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter no maximo {1} caracteres")]
    public string? Descricao { get; set; }

    //[Required]
    //[DataType(DataType.Currency)]
    //[Column(TypeName = "decimal(8,2)")]
    //[Range(1,10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    //public decimal Preco { get; set; }

    public DateTime DataCadastro { get; set; }
    public int MinisterioId { get; set; }

    [JsonIgnore]
    public Ministerio? Ministerio { get; set; }

    public ICollection<ApplicationUser>? Membros { get; set; }
}
