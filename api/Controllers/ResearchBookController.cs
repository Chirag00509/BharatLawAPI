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
    public class ResearchBookController : ControllerBase
    {
        private readonly LegalGenContext _context;

        public ResearchBookController(LegalGenContext context)
        {
            _context = context;
        }

        // GET: api/ResearchBook
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResearchBook>>> GetResearchBook()
        {
          if (_context.ResearchBook == null)
          {
              return NotFound();
          }
            return await _context.ResearchBook.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ResearchBook>>> GetResearchBookByToken(int id)
        {
           var book = await _context.ResearchBook.Where(u => u.UserId == id).ToListAsync();

            if (book == null) 
            {
                return NotFound();
            }

            return book;
        }

        //// GET: api/ResearchBook/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ResearchBook>> GetResearchBook(int id)
        //{
        //  if (_context.ResearchBook == null)
        //  {
        //      return NotFound();
        //  }
        //    var researchBook = await _context.ResearchBook.FindAsync(id);

        //    if (researchBook == null)
        //    {
        //        return NotFound();
        //    }

        //    return researchBook;
        //}

        // PUT: api/ResearchBook/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResearchBook(int id, ResearchBook researchBook)
        {
            if (id != researchBook.Id)
            {
                return BadRequest();
            }

            _context.Entry(researchBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResearchBookExists(id))
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

        // POST: api/ResearchBook
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResearchBook>> PostResearchBook(ResearchBook researchBook)
        {
          if (_context.ResearchBook == null)
          {
              return Problem("Entity set 'LegalGenContext.ResearchBook'  is null.");
          }
            _context.ResearchBook.Add(researchBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResearchBook", new { id = researchBook.Id }, researchBook);
        }

        // DELETE: api/ResearchBook/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResearchBook(int id)
        {
            if (_context.ResearchBook == null)
            {
                return NotFound();
            }
            var researchBook = await _context.ResearchBook.FindAsync(id);
            if (researchBook == null)
            {
                return NotFound();
            }

            _context.ResearchBook.Remove(researchBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResearchBookExists(int id)
        {
            return (_context.ResearchBook?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
