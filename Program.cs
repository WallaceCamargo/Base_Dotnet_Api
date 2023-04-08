
using APICatalago.Context;
using APICatalago.DTOs.Mappings;
using APICatalago.Models;
using APICatalago.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace APICatalago;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(op => 
        op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",new OpenApiInfo { Title = "APICatalogo", Version= "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat= "JWT",
                In = ParameterLocation.Header,
                Description = "Header de autorização JWT usando o esquema Bearer. \r\n\r\nInforme 'Bearer'[espaço] e o seu token. \r\n\r\nExemplo: \'Bearer 12345abcdef\'",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        
        });
        
        builder.Services.AddCors();

        string? mysqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
        
        builder.Services.AddDbContext<AppDbContext>(options =>
                            options.UseMySql(mysqlConnection, 
                            ServerVersion.AutoDetect(mysqlConnection)));

        //identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        //JWT
        //adiciona o manipulador de autenticacao e define o 
        //esquema de autenticacao usado : Bearer
        //valida o emissor, a audiencia e a chave
        //usando a chave secreta valida a assinatura
        builder.Services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
                ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            });

        builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

        //add AUTOMAPPER
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        
        IMapper mapper = mappingConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(opt => opt.AllowAnyOrigin());
        
        app.UseRouting();
        app.UseAuthentication();//identity
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}