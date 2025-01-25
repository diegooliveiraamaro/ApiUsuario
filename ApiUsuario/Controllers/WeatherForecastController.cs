using ApiUsuario.Data;
using ApiUsuario.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiUsuario.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    //public class WeatherForecastController : ControllerBase
    //{
    //    //private static readonly string[] Summaries = new[]
    //    //{
    //    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //    //};

    //    private readonly ILogger<WeatherForecastController> _logger;

    //    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    //    {
    //        _logger = logger;
    //    }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        [Route("api/[controller]")]
        [ApiController]
        public class UsuariosController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public UsuariosController(ApplicationDbContext context)
            {
                _context = context;
            }
            [HttpGet]
            public IActionResult GetUsuarios()
            {
                return Ok(new { mensagem = "Funcionando!" });
            }
            //[HttpGet]
            //public async Task<IActionResult> GetUsuarios()
            //{
            //    return Ok(await _context.Usuarios.ToListAsync());
            //}

            [HttpGet("{id}")]
            public async Task<IActionResult> GetUsuario(int id)
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return NotFound();

                return Ok(usuario);
            }

            [HttpPost]
            public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
                }
                catch (DbUpdateException)
                {
                    if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                        return Conflict(new { message = "Email já está em uso." });

                    throw;
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
            {
                if (id != usuario.Id) return BadRequest();

                _context.Entry(usuario).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(id)) return NotFound();
                    throw;
                }

                return NoContent();
            }

            [HttpPatch("{id}")]
            public async Task<IActionResult> PatchUsuario(int id, [FromBody] Usuario usuario)
            {
                var existingUsuario = await _context.Usuarios.FindAsync(id);
                if (existingUsuario == null) return NotFound();

                if (!string.IsNullOrEmpty(usuario.Nome)) existingUsuario.Nome = usuario.Nome;
                if (!string.IsNullOrEmpty(usuario.Email)) existingUsuario.Email = usuario.Email;

                await _context.SaveChangesAsync();
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUsuario(int id)
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) return NotFound();

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            private bool UsuarioExists(int id) => _context.Usuarios.Any(e => e.Id == id);
        }
   // }
}
