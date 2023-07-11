using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Modal;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Azure.Core;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LegalGenContext _context;

        public UserController(LegalGenContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            return await _context.User.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.User == null)
          {
              return Problem("Entity set 'LegalGenContext.User'  is null.");
          }
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("/forgot")] 
        public async Task<ActionResult<User>> ForgotPassword(string email) 
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            string token = CreateRandomToken();
            if (user == null)
            {
                return BadRequest("User not found.");
            } 
            else
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("chiraglabha05@gmail.com", "xmbmrinlzcavyppt")
                };

                var message = new MailMessage
                {
                    From = new MailAddress("chiraglabha05@gmail.com"),
                    Subject = "Forgot-password",
                    Body = $"reset link is `http://localhost:4200/user/update-password/{token}",
                    IsBodyHtml = true
                };
                message.To.Add(email);

                // Send the email
                smtpClient.Send(message);
                user.resetToken = token;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost("update-password")]

        public async Task<ActionResult<User>> UpdatePassword(string password, string token) 
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.resetToken == token);

            if(user == null) 
            {
                return BadRequest("User not found");
            } 
            else
            {
                user.Password = password;
                user.resetToken = null;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
