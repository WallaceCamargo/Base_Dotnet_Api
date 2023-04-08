using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalago.Models;

[Table("Ministerio")]
public class Ministerio
{
    public Ministerio()
    {

        Igrejas = new Collection<Igreja>();
    }

    [Key]
    public int MinisterioId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }


    public ICollection<Igreja>? Igrejas { get; set; }
}

