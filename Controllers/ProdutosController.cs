using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly APICatalogoContext _context;

    public ProdutosController(APICatalogoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Index()
    {
        try
        {
            if (_context.Produtos == null) return NotFound();
            var produtos = _context.Produtos.AsNoTracking().ToList();
            if (produtos is null) return NotFound();
            return produtos;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Show(int id)
    {
        try
        {
            if (_context.Produtos == null) return NotFound();
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null) return NotFound("Produto não encontrado...");
            return produto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpPost]
    public ActionResult Create(Produto produto)
    {
        try
        {
            if (_context.Produtos == null) return BadRequest("Produtos não existe no contexto.");
            if (produto is null) return BadRequest("Produto não existe no corpo da requisição.");
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult Update(int id, Produto produto)
    {
        try
        {
            if (_context.Produtos == null) return BadRequest("Produtos não existe no contexto.");
            if (produto is null) return BadRequest("Produto não existe no corpo da requisição.");
            if (produto.Id != id) return BadRequest("Produto não possui mesmo id da rota.");

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
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
            if (_context.Produtos == null) return NotFound();
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null) return NotFound("Produto não encontrado...");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Falha do servidor ao processar a requisição");
        }
    }
}