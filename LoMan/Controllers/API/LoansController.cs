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
    public class LoansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            return await _context.Loans.OrderBy(l => l.Idate).ToListAsync();
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(string id)
        {
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // PUT: api/Loans/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(string id, Loan loan)
        {
            if (id != loan.Id)
            {
                return BadRequest();
            }
            if(loan.Rdate < DateTime.Today)              
                        {
                            loan.Status = "Pending";                        
                        } 
                        else
                        {
                            loan.Status = "Not Paid";
                        } 
            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
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

        // POST: api/Loans
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan(LoanApi loanApi)
        {
            
            try
            {
                if (loanApi.Type.Equals("Weekly"))
                {
                    float Principle = loanApi.loan.Principle / loanApi.loan.Times;
                    float Interest = ((loanApi.loan.Principle * loanApi.loan.Rate) / 100) / loanApi.loan.Times;
                    double Period = loanApi.loan.Period;
                    for (int i = 0; i < loanApi.loan.Times; i++)
                    {
                        loanApi.loan.Id = Guid.NewGuid().ToString();
                        loanApi.loan.Rdate = loanApi.loan.Idate.AddDays(Period);
                        loanApi.loan.Amount = Principle + Interest;
                        loanApi.loan.Interest = Interest;
                        loanApi.loan.Principle = Principle;
                        TimeSpan Diff = loanApi.loan.Rdate.Subtract(loanApi.loan.Idate);
                        loanApi.loan.Period = Diff.Days;
                        if(loanApi.loan.Rdate < DateTime.Today)              
                        {
                            loanApi.loan.Status = "Pending";                        
                        } 
                        else
                        {
                            loanApi.loan.Status = "Not Paid";
                        } 
                        _context.Add(loanApi.loan);
                        await _context.SaveChangesAsync();
                        loanApi.loan.Idate = loanApi.loan.Idate.AddDays(Period);
                    }
                }
                else
                {
                    loanApi.loan.Id = Guid.NewGuid().ToString();
                    loanApi.loan.Interest = (loanApi.loan.Principle * loanApi.loan.Rate) / 100;
                    loanApi.loan.Amount = loanApi.loan.Principle + loanApi.loan.Interest;
                    TimeSpan Period = loanApi.loan.Rdate.Subtract(loanApi.loan.Idate);
                    loanApi.loan.Period = Period.Days;
                    if(loanApi.loan.Rdate < DateTime.Today)              
                        {
                            loanApi.loan.Status = "Pending";                        
                        } 
                        else
                        {
                            loanApi.loan.Status = "Not Paid";
                        } 
                    loanApi.loan.Times = 1;
                    _context.Add(loanApi.loan);
                    await _context.SaveChangesAsync();
                }
                           
            }
            catch (DbUpdateException)
            {
                if (LoanExists(loanApi.loan.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLoan", new { id = loanApi.loan.Id }, loanApi.loan);
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Loan>> DeleteLoan(string id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        private bool LoanExists(string id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
