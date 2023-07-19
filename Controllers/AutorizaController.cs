using APICatalago.DTOs;
using APICatalago.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IMapper _mapper;

    public AutorizaController(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<UsuarioDTO>> Get(int id)
    {
        var userupdate = await _userManager.FindByIdAsync(id.ToString());
        if (userupdate != null)
        {
            var UsuarioDto = _mapper.Map<UsuarioDTO>(userupdate);
            return Ok(UsuarioDto);
        }else
        {
            return BadRequest("não foi possivel trazer os usuarios");
        }    
    }

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("allUsers/{ministerioId:int:min(1)}")]
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetAllUsuarios(int ministerioId)
    {
        var Usuarios =  _userManager.Users;
        var UsuariosDto = _mapper.Map<List<UsuarioDTO>>(Usuarios);
        if (UsuariosDto != null)
        {
            return Ok(UsuariosDto);
        }
        else
        {
            return BadRequest("não foi possivel trazer os usuarios");
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        }
        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            MinisterioId = model.MinisterioId,
            IgrejaId = model.IgrejaId,
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
    [HttpPost("{id:int:min(1)}")]
    public async Task<ActionResult<UsuarioDTO>> UpdateUser(int id, [FromBody] UsuarioDTO user)
    {
        var userupdate = await _userManager.FindByIdAsync(id.ToString());
        if (userupdate != null)
        {

            if (!string.IsNullOrEmpty(user.Email))
                userupdate.Email = user.Email;

            if (!string.IsNullOrEmpty(user.UserName))
                userupdate.UserName = user.UserName;

            if (!string.IsNullOrEmpty(user.NormalizedUserName))
                userupdate.NormalizedUserName = user.NormalizedUserName;

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                userupdate.PhoneNumber = user.PhoneNumber;

            if (!string.IsNullOrEmpty(user.Password))
                userupdate.PasswordHash = _passwordHasher.HashPassword(userupdate, user.Password);

            if (!string.IsNullOrEmpty(userupdate.Email) && !string.IsNullOrEmpty(userupdate.PasswordHash))
            {
                IdentityResult result = await _userManager.UpdateAsync(userupdate);
            }
        }
        else
        {
            return NotFound("Membro não encontrado ..."); 
        }

        return Ok(userupdate);
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

    [HttpPut("Logout")]
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
