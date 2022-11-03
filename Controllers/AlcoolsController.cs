using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alcolik.Data;
using alcolik.Model;

namespace alcolik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlcoolsController : ControllerBase
    {
        private readonly AlcolikDbContext _context;

        public AlcoolsController(AlcolikDbContext context)
        {
            _context = context;
        }

        // GET: api/Alcools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alcool>>> GetAlcool()
        {
            return await _context.Alcool.ToListAsync();
        }

        // GET: api/Alcools/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alcool>> GetAlcool(int id)
        {
            var alcool = await _context.Alcool.FindAsync(id);

            if (alcool == null)
            {
                return NotFound();
            }

            return alcool;
        }

        // PUT: api/Alcools/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlcool(int id, Alcool alcool)
        {
            if (id != alcool.Id)
            {
                return BadRequest();
            }

            _context.Entry(alcool).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlcoolExists(id))
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

        // POST: api/Alcools
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Alcool>> PostAlcool(Alcool alcool)
        {
            _context.Alcool.Add(alcool);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlcool", new { id = alcool.Id }, alcool);
        }

        // DELETE: api/Alcools/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlcool(int id)
        {
            var alcool = await _context.Alcool.FindAsync(id);
            if (alcool == null)
            {
                return NotFound();
            }

            _context.Alcool.Remove(alcool);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlcoolExists(int id)
        {
            return _context.Alcool.Any(e => e.Id == id);
        }
    }
}
