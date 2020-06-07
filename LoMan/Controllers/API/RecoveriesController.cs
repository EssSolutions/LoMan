using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoMan.Data;
using LoMan.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoMan.Controllers.API
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecoveriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Recoveries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recoveries>>> GetRecoveries()
        {
            return await _context.Recoveries.ToListAsync();
        }

        // GET: api/Recoveries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recoveries>> GetRecoveries(string id)
        {
            var recoveries = await _context.Recoveries.FindAsync(id);

            if (recoveries == null)
            {
                return NotFound();
            }

            return recoveries;
        }

        // PUT: api/Recoveries/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecoveries(string id, Recoveries recoveries)
        {
            if (id != recoveries.Id)
            {
                return BadRequest();
            }

            _context.Entry(recoveries).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecoveriesExists(id))
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

        // POST: api/Recoveries
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Recoveries>> PostRecoveries(RecoveryApi recoveryApi)
        {
            Recoveries recovery = new Recoveries();
            recovery.Id = Guid.NewGuid().ToString();
            if (recoveryApi.Id != recoveryApi.loan.Id)
            {
                return NotFound();
            }
            try
            {
                recoveryApi.loan = await _context.Loans.FindAsync(recoveryApi.Id);
                
                recovery.Date = DateTime.Today;
                recovery.Name = recoveryApi.loan.Name;
                if (recoveryApi.Type.Equals("Interest"))
                {
                    recovery.Interest = recoveryApi.loan.Interest;
                    recoveryApi.loan.Status = "Interest Paid";

                }
                else if (recoveryApi.Type.Equals("Principle"))
                {
                    recovery.Principle = recoveryApi.loan.Principle;
                    recoveryApi.loan.Status = "Principle Paid";
                }
                else if (recoveryApi.Type.Equals("Complete"))
                {
                    recovery.Interest = recoveryApi.loan.Interest;
                    recovery.Principle = recoveryApi.loan.Principle;
                    recoveryApi.loan.Status = "Paid";
                }
                else
                {
                    throw new InvalidOperationException("Invalid Recovery Type");
                }
                _context.Add(recovery);
                _context.Update(recoveryApi.loan);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecoveriesExists(recoveryApi.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetRecoveries", new { id = recovery.Id }, recovery);
        }

        // DELETE: api/Recoveries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Recoveries>> DeleteRecoveries(string id)
        {
            var recoveries = await _context.Recoveries.FindAsync(id);
            if (recoveries == null)
            {
                return NotFound();
            }

            _context.Recoveries.Remove(recoveries);
            await _context.SaveChangesAsync();

            return recoveries;
        }

        private bool RecoveriesExists(string id)
        {
            return _context.Recoveries.Any(e => e.Id == id);
        }
    }
}
