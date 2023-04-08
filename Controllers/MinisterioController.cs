using APICatalago.Context;
using APICatalago.DTOs;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalago.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
[ApiController]
public class MinisterioController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper; 

    public MinisterioController(IUnitOfWork context, IMapper mapper)
    {
        _uof = context;
        _mapper = mapper;
    }

    [HttpGet("Igrejas")]
    public async  Task<ActionResult<IEnumerable<MinisterioDTO>>> GetGetMinisterioIgrejas()
    {
        try
        {
            
            var ministerios = await _uof.MinisterioRepository.GetGetMinisterioIgrejas();
            var ministeriosDto = _mapper.Map<List<MinisterioDTO>>(ministerios);
            return ministeriosDto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }
    //Categorias?pageNumber=1&PageSize=2
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MinisterioDTO>>> Get([FromQuery] MinisterioParameters ministerioParameters)
    {
        try
        {
            var ministerios = await _uof.MinisterioRepository.GetMinisterioPaginado(ministerioParameters);

            var metadata = new
            {
                ministerios.TotalCount,
                ministerios.PageSize,
                ministerios.CurrentPage,
                ministerios.TotalPages,
                ministerios.hasNext,
                ministerios.hasPrevius
            };

            Response.Headers.Add("x-Pagination", JsonConvert.SerializeObject(metadata));

            var ministeriosDto = _mapper.Map<List<MinisterioDTO>>(ministerios);
            return ministeriosDto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterMinisterios")]
    public async Task<ActionResult<MinisterioDTO>> Get(int id)
    {
        try
        {
            var ministerio = await _uof.MinisterioRepository.GetbyId(p => p.MinisterioId == id);

            if (ministerio is null)
                return NotFound("Ministerio não encontrado...");

            var ministerioDto = _mapper.Map<MinisterioDTO>(ministerio);

            return Ok(ministerioDto);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] MinisterioDTO ministerioDTO)
    {
        try
        {
            var ministerio = _mapper.Map<Ministerio>(ministerioDTO);
            _uof.MinisterioRepository.Add(ministerio);
           await _uof.Commit();

            

            return new CreatedAtRouteResult("ObterMinisterios",
                new { id = ministerio.MinisterioId }, ministerioDTO);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> Put(int id,[FromBody] MinisterioDTO ministerioDTO)
    {
        try
        {
            if (id != ministerioDTO.MinisterioId)
                return BadRequest("ministerio não encontrado...");

            var ministerio = _mapper.Map<Ministerio>(ministerioDTO);

            _uof.MinisterioRepository.Update(ministerio);
            await _uof.Commit();

            return Ok(ministerio);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<MinisterioDTO>> Delete(int id)
    {
        try
        {
            var ministerio = await _uof.MinisterioRepository.GetbyId(p => p.MinisterioId == id);

            if (ministerio is null)
                return NotFound("ministerio não encontrado ...");

            _uof.MinisterioRepository.Delete(ministerio);
            await _uof.Commit();
            var ministerioDto = _mapper.Map<MinisterioDTO>(ministerio);

            return Ok(ministerioDto);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }

    }
}
