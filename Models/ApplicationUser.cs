using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace APICatalago.Models;

public class ApplicationUser : IdentityUser<int>
{
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }

    public int MinisterioId { get; set; }
    public int IgrejaId { get; set; }

    [JsonIgnore]
    public Igreja? Igreja { get; set; }
}
