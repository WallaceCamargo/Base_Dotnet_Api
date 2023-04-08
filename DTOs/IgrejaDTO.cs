using APICatalago.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalago.DTOs;

public class IgrejaDTO
{
    public int IgrejaId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }

    //public decimal Preco { get; set; }
    //public string? ImagemUrl { get; set; }

    public int MinisterioId { get; set; }
}
