using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Modal;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("login")]
        public async Task<ActionResult<User>> login(string email, string password) 
        {
            var user =  await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

           if (user == null)
           {
               return NotFound();
           }

            string token = CreateRandomToken();
            user.actionToken = token;
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            Console.WriteLine(id);
            Console.WriteLine(user.Id);
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            Console.WriteLine(user.FirstName);
            Console.WriteLine(user.ContactDetails);
          if (_context.User == null)
          {
              return Problem("Entity set 'LegalGenContext.User'  is null.");
          }
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpGet("token")]

        public async Task<ActionResult<IEnumerable<User>>> GetUserByToken(string token)
        {
            var users = await _context.User.FirstOrDefaultAsync(u => u.actionToken == token);
            return Ok(users);
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
                    Body = $"Hello, {user.FirstName} {user.LastName}.\nreset link is http://localhost:4200/user/update-password/{token}.\nThank you",
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

         [HttpPost("change-password")]
        public async Task<ActionResult<User>> ChangePassword(string token, string password, string newPassword)
        {
           var user = await _context.User.FirstOrDefaultAsync(u => u.actionToken == token);
            
            if(user == null) 
            {
                return BadRequest();
            }

            if (user.Password != password)
            {
                return BadRequest();
            }

            user.Password = newPassword;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("logut")]

        public async Task<ActionResult<User>> logout(string token)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.actionToken == token);

            if(user == null)
            {
                return BadRequest();
            }

            user.actionToken = null;
            await _context.SaveChangesAsync();

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
