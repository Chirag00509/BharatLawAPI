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
            Console.WriteLine("Hello");
            Console.WriteLine(email);
            if (email != null)
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
                    Body = "reset link is http://localhost:4200/update-password",
                    IsBodyHtml = true
                };
                message.To.Add(email);

                // Send the email
                smtpClient.Send(message);
            }
            return Ok();
        }

        //[HttpGet("/email")]
        //public IActionResult Email(string email, string Body)
        //{
        //    string recipient = email;
        //    string subject = "Task";
        //    string body = Body;

        //    SendEmail(recipient, subject, body);

        //    return Ok();
        //}

        //[HttpPost("")]
        //public void SendEmail(string recipient, string subject, string body)
        //{
        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        Credentials = new NetworkCredential("chiraglabha05@gmail.com", "xmbmrinlzcavyppt")
        //    };

        //    var message = new MailMessage
        //    {
        //        From = new MailAddress("chiraglabha05@gmail.com"),
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true
        //    };
        //    message.To.Add(recipient);

        //    // Send the email
        //    smtpClient.Send(message);
        //}

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
