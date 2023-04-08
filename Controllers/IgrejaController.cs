using APICatalago.DTOs;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalago.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class IgrejaController : Controller
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mappper;
    
    public IgrejaController(IUnitOfWork context, IMapper mapper)
    {
        _uof = context;
        _mappper = mapper;
    }

    //[HttpGet("menorpreco")]
    //public async Task<ActionResult<IEnumerable<IgrejaDTO>>> GetProdutoPrecos()
    //{
    //    try
    //    {
    //        var produtos = await _uof.IgrejaRepository.GetProdutosPorPreco();
    //        var produtosDto = _mappper.Map<List<IgrejaDTO>>(produtos);
    //        return produtosDto;
    //    }
    //    catch (Exception)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError,
    //            "Ocorreu um problema ao tratar a sua solicitação.");
    //    }
        
    //}
    //Produtos?pageNumber=1&PageSize=2
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IgrejaDTO>>> Get([FromQuery] IgrejaParameters igrejaParameters)
    {
        try
        {
            var igrejas = await _uof.IgrejaRepository.GetIgreja(igrejaParameters);

            var metadata = new
            {
                igrejas.TotalCount,
                igrejas.PageSize,
                igrejas.CurrentPage,
                igrejas.TotalPages,
                igrejas.hasNext,
                igrejas.hasPrevius
            };

            Response.Headers.Add("x-Pagination", JsonConvert.SerializeObject(metadata));

            var igrejasDto = _mappper.Map<List<IgrejaDTO>>(igrejas);

            if (igrejasDto is null)
                return NotFound("Produtos não encontrados...");

            return igrejasDto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }

    [HttpGet("{id:int:min(1)}", Name = "ObterIgreja")]
    public async Task<ActionResult<IgrejaDTO>> Get(int id)
    {
        try
        {
            var igreja = await _uof.IgrejaRepository.GetbyId(p => p.IgrejaId == id);

            if (igreja is null)
                return NotFound("Produtos não encontrados...");

            var igrejaDto = _mappper.Map<IgrejaDTO>(igreja);
            return igrejaDto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }

    [HttpPost]
    public async  Task<ActionResult> Post([FromBody]IgrejaDTO igrejaDto)
    {
        try
        {
            var igreja = _mappper.Map<Igreja>(igrejaDto);
            _uof.IgrejaRepository.Add(igreja);
            await _uof.Commit();

            var igrejaDTO = _mappper.Map<IgrejaDTO>(igreja);

            return new CreatedAtRouteResult("ObterIgreja",
                new { id = igreja.IgrejaId }, igrejaDTO);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> Put(int id,[FromBody] IgrejaDTO igrejaDto) 
    {
        try
        {
            if (id != igrejaDto.IgrejaId)
            {
                return BadRequest();
            }
            var igreja = _mappper.Map<Igreja>(igrejaDto);
            _uof.IgrejaRepository.Update(igreja);
            await _uof.Commit();

            return Ok(igreja);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<IgrejaDTO>> Delete(int id) 
    {
        try
        {
            var igreja = await _uof.IgrejaRepository.GetbyId(p => p.IgrejaId == id);

            if (igreja is null)
                return NotFound("Igreja não encontrado ...");

            _uof.IgrejaRepository.Delete(igreja);
            await _uof.Commit();

            var igrejaDto = _mappper.Map<IgrejaDTO>(igreja);

            return Ok(igrejaDto);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }
}

