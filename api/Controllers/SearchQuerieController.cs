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
    public class SearchQuerieController : ControllerBase
    {
        private readonly LegalGenContext _context;

        public SearchQuerieController(LegalGenContext context)
        {
            _context = context;
        }

        // GET: api/SearchQuerie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchQuery>>> GetSearchQuery()
        {
          if (_context.SearchQuery == null)
          {
              return NotFound();
          }
            return await _context.SearchQuery.ToListAsync();
        }

        // GET: api/SearchQuerie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SearchQuery>> GetSearchQuery(int id)
        {
          if (_context.SearchQuery == null)
          {
              return NotFound();
          }
            var searchQuery = await _context.SearchQuery.FindAsync(id);

            if (searchQuery == null)
            {
                return NotFound();
            }

            return searchQuery;
        }

        // PUT: api/SearchQuerie/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSearchQuery(int id, SearchQuery searchQuery)
        {
            if (id != searchQuery.Id)
            {
                return BadRequest();
            }

            _context.Entry(searchQuery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SearchQueryExists(id))
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

        // POST: api/SearchQuerie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SearchQuery>> PostSearchQuery(SearchQuery searchQuery)
        {
          if (_context.SearchQuery == null)
          {
              return Problem("Entity set 'LegalGenContext.SearchQuery'  is null.");
          }
            _context.SearchQuery.Add(searchQuery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSearchQuery", new { id = searchQuery.Id }, searchQuery);
        }

        // DELETE: api/SearchQuerie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchQuery(int id)
        {
            if (_context.SearchQuery == null)
            {
                return NotFound();
            }
            var searchQuery = await _context.SearchQuery.FindAsync(id);
            if (searchQuery == null)
            {
                return NotFound();
            }

            _context.SearchQuery.Remove(searchQuery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SearchQueryExists(int id)
        {
            return (_context.SearchQuery?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
