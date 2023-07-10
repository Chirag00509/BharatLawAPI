using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Modal;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalInformationController : ControllerBase
    {
        private readonly LegalGenContext _context;

        public LegalInformationController(LegalGenContext context)
        {
            _context = context;
        }

        // GET: api/LegalInformation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalInformation>>> GetLegalInformation()
        {
          if (_context.LegalInformation == null)
          {
              return NotFound();
          }
            return await _context.LegalInformation.ToListAsync();
        }

        // GET: api/LegalInformation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LegalInformation>> GetLegalInformation(int id)
        {
          if (_context.LegalInformation == null)
          {
              return NotFound();
          }
            var legalInformation = await _context.LegalInformation.FindAsync(id);

            if (legalInformation == null)
            {
                return NotFound();
            }

            return legalInformation;
        }

        // PUT: api/LegalInformation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLegalInformation(int id, LegalInformation legalInformation)
        {
            if (id != legalInformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(legalInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LegalInformationExists(id))
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

        // POST: api/LegalInformation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LegalInformation>> PostLegalInformation(LegalInformation legalInformation)
        {
          if (_context.LegalInformation == null)
          {
              return Problem("Entity set 'LegalGenContext.LegalInformation'  is null.");
          }
            _context.LegalInformation.Add(legalInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLegalInformation", new { id = legalInformation.Id }, legalInformation);
        }

        // DELETE: api/LegalInformation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLegalInformation(int id)
        {
            if (_context.LegalInformation == null)
            {
                return NotFound();
            }
            var legalInformation = await _context.LegalInformation.FindAsync(id);
            if (legalInformation == null)
            {
                return NotFound();
            }

            _context.LegalInformation.Remove(legalInformation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LegalInformationExists(int id)
        {
            return (_context.LegalInformation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
