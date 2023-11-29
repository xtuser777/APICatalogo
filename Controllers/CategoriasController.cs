using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly APICatalogoContext _context;

    public CategoriasController(APICatalogoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Index()
    {
        try
        {
            if (_context.Categorias == null) return NotFound();
            var categorias = _context.Categorias.AsNoTracking().ToList();
            return categorias;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> IndexProdutos()
    {
        try
        {
            if (_context.Categorias == null) return NotFound();
            var categorias = _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
            return categorias;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Show(int id)
    {
        try
        {
            if (_context.Categorias == null) return NotFound();
            var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);
            if (categoria is null) return NotFound("categoria não encontrada...");
            return categoria;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpPost]
    public ActionResult Create(Categoria categoria)
    {
        try
        {
            if (_context.Categorias == null) return BadRequest("Produtos não existe no contexto.");
            if (categoria is null) return BadRequest("Produto não existe no corpo da requisição.");
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult Update(int id, Categoria categoria)
    {
        try
        {
            if (_context.Categorias == null) return BadRequest("categorias não existe no contexto.");
            if (categoria is null) return BadRequest("categoria não existe no corpo da requisição.");
            if (categoria.Id != id) return BadRequest("categoria não possui mesmo id da rota.");

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        try
        {
            if (_context.Categorias == null) return NotFound();
            var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);
            if (categoria is null) return NotFound("categoria não encontrada...");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }
}