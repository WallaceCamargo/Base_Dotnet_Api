using APICatalago.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICatalago.Controllers;

[Route("[Controller]")]
[ApiController]
public class AutorizaController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return "AutorizaController :: Acessado em : "
            + DateTime.Now.ToLongTimeString();
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        }
        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _signInManager.SignInAsync(user, false);
        return Ok(GeraToken(model));
    }

    [HttpPost("Login")]  
    public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
    {
        //verifica se model e valido
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        }

        //verifica as credenciais do usuário e retorna um valor 
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
            userInfo.Password, isPersistent: false, lockoutOnFailure: false);

        if(result.Succeeded)
        {
            return Ok(GeraToken(userInfo));
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Login Inválido...");
            return BadRequest(ModelState);
        }
    }

    [HttpPut("Login")]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    private UsuarioToken GeraToken(UsuarioDTO userInfo) 
    {
        //define declarações do usuário
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
            new Claim("meuPet", "pipoca"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        //gera uma chave com base em um algoritmo simetrico
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //gera a assinatura digital do token usando o algoritmo hmac e a chave privada
        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //Tempo de expiração do Token.
        var expiracao = _configuration["TokenConfiguration:ExpireHours"];
        var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

        //classe que representa um token JWT e gera o token
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["TokenConfiguration:Issuer"],
            audience: _configuration["TokenConfiguration:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credenciais
            );

        return new UsuarioToken()
        {
            Authenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Message = "Token JWT OK"
        };
    }
}
