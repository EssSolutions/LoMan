using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoMan.Data;
using LoMan.Models;
using LoMan.ViewModels;

namespace LoMan.Controllers
{
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Loans.OrderBy(l => l.Idate).ToListAsync());
        }

        // GET: Loans/Pending
        public async Task<IActionResult> Pending()
        {
            var PList = _context.Loans.Where(l => l.Status.Equals("Pending")).OrderBy(l=> l.Idate);
            return View(await PList.ToListAsync());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/CreateWeekly
        public IActionResult CreateWeekly()
        {
            return View();
        }

        // POST: Loans/CreateWeekly
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWeekly([Bind("Id,Name,Address,Phone,Asset,Principle,Interest,Rate,Amount,Idate,Rdate,Penalty,Times,Period,Status")] Loan loan)
        {
            if (ModelState.IsValid)
            {

                float Principle = loan.Principle / loan.Times;
                float Interest = ((loan.Principle * loan.Rate) / 100) / loan.Times;
                double Period = loan.Period;
                for (int i = 0; i < loan.Times; i++)
                {
                    loan.Id = Guid.NewGuid().ToString();
                    loan.Rdate = loan.Idate.AddDays(Period);
                    loan.Amount = Principle + Interest;
                    loan.Interest = Interest;
                    loan.Principle = Principle;
                    TimeSpan Diff = loan.Rdate.Subtract(loan.Idate);
                    loan.Period = Diff.Days;
                    if (loan.Rdate < DateTime.Today)              
                    {
                        loan.Status = "Pending";                        
                    } 
                    else
                    {
                        loan.Status = "Not Paid";
                    }                                 
                    _context.Add(loan);
                    await _context.SaveChangesAsync();
                    loan.Idate = loan.Idate.AddDays(Period);
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Phone,Asset,Principle,Interest,Rate,Amount,Idate,Rdate,Penalty,Times,Period,Status")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                loan.Id = Guid.NewGuid().ToString();
                loan.Interest = (loan.Principle * loan.Rate) / 100;
                loan.Amount = loan.Principle + loan.Interest;
                TimeSpan Period = loan.Rdate.Subtract(loan.Idate);
                loan.Period = Period.Days;
                if(loan.Rdate < DateTime.Today)              
                    {
                        loan.Status = "Pending";                        
                    } 
                    else
                    {
                        loan.Status = "Not Paid";
                    }    
                loan.Times = 1;
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Address,Phone,Asset,Principle,Interest,Rate,Amount,Idate,Rdate,Penalty,Times,Period,Status")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        // GET: Loans/Recover/5
        public async Task<IActionResult> Recover(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RecoveryVM recoveryVM = new RecoveryVM
            {
                loan = await _context.Loans.FindAsync(id)
            };
            if (recoveryVM.loan == null)
            {
                return NotFound();
            }
            return View(recoveryVM);
        }

        // POST: Loans/Recover/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recover(string id, [Bind("Id,Name,Address,Phone,Asset,Principle,Interest,Rate,Amount,Idate,Rdate,Penalty,Times,Period,Status")] Loan loan,string Type,float Penalty)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    loan = await _context.Loans.FindAsync(id);
                    Recoveries recovery = new Recoveries
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Today,
                        Name = loan.Name
                    };
                    loan.Penalty = Penalty;
                    if (Type.Equals("Interest"))
                    {
                        recovery.Interest = loan.Interest + loan.Penalty;
                        loan.Rdate = loan.Rdate.AddDays(loan.Period);
                        loan.Status = "Pending";

                    }
                    else if (Type.Equals("Principle"))
                    {
                        recovery.Principle = loan.Principle + loan.Penalty;
                        loan.Status = "Principle Paid";
                    }
                    else if(Type.Equals("Complete"))
                    {
                        recovery.Interest = loan.Interest + loan.Penalty;
                        recovery.Principle = loan.Principle;
                        loan.Status = "Paid";
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid Recovery Type");
                    }
                    _context.Add(recovery);
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var loan = await _context.Loans.FindAsync(id);
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(string id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
