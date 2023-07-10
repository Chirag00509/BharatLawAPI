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
    public class ChatInteractionController : ControllerBase
    {
        private readonly LegalGenContext _context;

        public ChatInteractionController(LegalGenContext context)
        {
            _context = context;
        }

        // GET: api/ChatInteraction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatInteraction>>> GetChatInteraction()
        {
          if (_context.ChatInteraction == null)
          {
              return NotFound();
          }
            return await _context.ChatInteraction.ToListAsync();
        }

        // GET: api/ChatInteraction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatInteraction>> GetChatInteraction(int id)
        {
          if (_context.ChatInteraction == null)
          {
              return NotFound();
          }
            var chatInteraction = await _context.ChatInteraction.FindAsync(id);

            if (chatInteraction == null)
            {
                return NotFound();
            }

            return chatInteraction;
        }

        // PUT: api/ChatInteraction/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatInteraction(int id, ChatInteraction chatInteraction)
        {
            if (id != chatInteraction.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatInteraction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatInteractionExists(id))
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

        // POST: api/ChatInteraction
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChatInteraction>> PostChatInteraction(ChatInteraction chatInteraction)
        {
          if (_context.ChatInteraction == null)
          {
              return Problem("Entity set 'LegalGenContext.ChatInteraction'  is null.");
          }
            _context.ChatInteraction.Add(chatInteraction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatInteraction", new { id = chatInteraction.Id }, chatInteraction);
        }

        // DELETE: api/ChatInteraction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatInteraction(int id)
        {
            if (_context.ChatInteraction == null)
            {
                return NotFound();
            }
            var chatInteraction = await _context.ChatInteraction.FindAsync(id);
            if (chatInteraction == null)
            {
                return NotFound();
            }

            _context.ChatInteraction.Remove(chatInteraction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChatInteractionExists(int id)
        {
            return (_context.ChatInteraction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
