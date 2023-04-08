using APICatalago.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
{
	public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
	{

	}

	public DbSet<Ministerio>? Ministerio { get; set; }
	public DbSet<Igreja>? Igreja { get; set; }

}
