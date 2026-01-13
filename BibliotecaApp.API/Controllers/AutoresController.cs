using BibliotecaApp.API.Entities;
using BibliotecaApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<Autor>>> GetTodos()
    {
        return await _context.Autores.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Autor>> GetPorId(int id)
    {
        var autor = await _context.Autores.FindAsync(id);

        if (autor == null)
        {
            return NotFound("Autor não encontrado!");
        }

        return autor;
    }

    [HttpPost]
    public async Task<ActionResult<Autor>> CriarNovo(Autor novoCliente)
    {
        _context.Autores.Add(novoCliente);
        await _context.SaveChangesAsync(); // O "Commit" no banco

        return CreatedAtAction(nameof(GetPorId), new { id = novoCliente.Id }, novoCliente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Autor autorEditado)
    {
        if(id != autorEditado.Id)
        {
            return BadRequest();
        }

        _context.Entry(autorEditado).State = EntityState.Modified;

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
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null)
        {
            return NotFound();
        }

        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}