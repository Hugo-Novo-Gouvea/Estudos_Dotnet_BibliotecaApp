using BibliotecaApp.API.Entities;
using BibliotecaApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivrosController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<Livro>>> GetTodos()
    {
        return await _context.Livros.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Livro>> GetPorId(int id)
    {
        var livro = await _context.Livros.FindAsync(id);

        if (livro == null)
        {
            return NotFound("Livro não encontrado!");
        }

        return livro;
    }

    [HttpPost]
    public async Task<ActionResult<Livro>> CriarNovo(Livro novoLivro)
    {
        _context.Livros.Add(novoLivro);
        await _context.SaveChangesAsync(); // O "Commit" no banco

        return CreatedAtAction(nameof(GetPorId), new { id = novoLivro.Id }, novoLivro);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Livro livroEditado)
    {
        if(id != livroEditado.Id)
        {
            return BadRequest();
        }

        _context.Entry(livroEditado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if(!_context.Autores.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        if (livro == null)
        {
            return NotFound();
        }

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}