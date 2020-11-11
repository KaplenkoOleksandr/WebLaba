using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLaba.Models;

namespace WebLaba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Corporations1Controller : ControllerBase
    {
        private readonly WebLabaContext _context;

        public Corporations1Controller(WebLabaContext context)
        {
            _context = context;
        }

        // GET: api/Corporations1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Corporations>>> GetCorporations()
        {
            return await _context.Corporations.ToListAsync();
        }

        // GET: api/Corporations1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Corporations>> GetCorporations(int id)
        {
            var corporations = await _context.Corporations.FindAsync(id);

            if (corporations == null)
            {
                return NotFound();
            }

            return corporations;
        }

        // PUT: api/Corporations1/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCorporations(int id, Corporations corporations)
        {
            if (id != corporations.Id)
            {
                return BadRequest();
            }

            _context.Entry(corporations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorporationsExists(id))
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

        // POST: api/Corporations1
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Corporations>> PostCorporations(Corporations corporations)
        {
            _context.Corporations.Add(corporations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCorporations", new { id = corporations.Id }, corporations);
        }

        // DELETE: api/Corporations1/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Corporations>> DeleteCorporations(int id)
        {
            var corporations = await _context.Corporations.FindAsync(id);
            if (corporations == null)
            {
                return NotFound();
            }

            _context.Corporations.Remove(corporations);
            await _context.SaveChangesAsync();

            return corporations;
        }

        private bool CorporationsExists(int id)
        {
            return _context.Corporations.Any(e => e.Id == id);
        }
    }
}
