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
using System.Globalization;

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
        public async Task<IActionResult> Index(DateTime MonthYear, string SearchType, string SearchInput)
        {
            var List = from l in _context.Loans                        
                        select l;
            DateTime Test = new DateTime();
            if (MonthYear != Test)
            {
                List = List.Where(l => l.Idate.Month == MonthYear.Month && l.Idate.Year == MonthYear.Year);

            }
            else if (!string.IsNullOrEmpty(SearchType))
            {
                if (SearchType.Equals("name"))
                {
                    List = List.Where(l => l.Name.Contains(SearchInput));

                }
                else if (SearchType.Equals("idate"))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime SearchDate = DateTime.ParseExact(SearchInput, "dd-MM-yyyy", provider);
                    List = List.Where(l => l.Idate == SearchDate);

                }
                else if (SearchType.Equals("rdate"))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime SearchDate = DateTime.ParseExact(SearchInput, "dd-MM-yyyy", provider);
                    List = List.Where(l => l.Rdate == SearchDate);

                }
            }

            return View(await List.OrderBy(l => l.Idate).ToListAsync());
        }

        // GET: Loans/Pending
        public async Task<IActionResult> Pending(DateTime MonthYear, string SearchType, string SearchInput)
        {
            var PList = from l in _context.Loans
                        where l.Status.Equals("Pending")
                        select l;
            DateTime Test = new DateTime();
            if (MonthYear != Test)
            {
                PList = PList.Where(l => l.Idate.Month == MonthYear.Month && l.Idate.Year == MonthYear.Year);
                
            }
            else if (!string.IsNullOrEmpty(SearchType))
            {
                if (SearchType.Equals("name"))
                {
                    PList = PList.Where(l => l.Name.Contains(SearchInput));
                   
                }
                else if (SearchType.Equals("idate"))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime SearchDate = DateTime.ParseExact(SearchInput, "dd-MM-yyyy", provider);
                    PList = PList.Where(l => l.Idate == SearchDate);
                    
                }
                else if (SearchType.Equals("rdate"))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime SearchDate = DateTime.ParseExact(SearchInput, "dd-MM-yyyy", provider);
                    PList = PList.Where(l => l.Rdate == SearchDate);
                   
                }               
            }

            return View(await PList.OrderBy(l => l.Idate).ToListAsync());


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
                }

                return RedirectToPage("Index","Home");
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
                if (loan.Rdate < DateTime.Today)
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
                var referer = Request.Headers["Referer"].ToString();
                ViewBag.Referrer = referer;
                return View();
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
            LoanVM loanVM = new LoanVM();
            loanVM.loan = await _context.Loans.FindAsync(id);
            if (loanVM.loan == null)
            {
                return NotFound();
            }
            loanVM.PreviousUrl = Request.Headers["Referer"].ToString();             
            return View(loanVM);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Address,Phone,Asset,Principle,Interest,Rate,Amount,Idate,Rdate,Penalty,Times,Period,Status")] Loan loan,string PreviousUrl)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (loan.Rdate < DateTime.Today)
                    {
                        loan.Status = "Pending";
                    }
                    else
                    {
                        loan.Status = "Not Paid";
                    }
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
                
                return Redirect(PreviousUrl);
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
            recoveryVM.PreviousUrl = Request.Headers["Referer"].ToString();
            return View(recoveryVM);
        }

        // POST: Loans/Recover/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recover(string id, [Bind("Id,Name,Address,Phone,Asset,Principle,Interest,Rate,Amount,Idate,Rdate,Penalty,Times,Period,Status")] Loan loan, string Type, float Penalty,string PreviousUrl)
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
                        loan.Status = "Not Paid";

                    }
                    else if (Type.Equals("Principle"))
                    {
                        recovery.Principle = loan.Principle + loan.Penalty;
                        loan.Status = "Principle Paid";
                    }
                    else if (Type.Equals("Complete"))
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
                return Redirect(PreviousUrl);
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
            LoanVM loanVM = new LoanVM();
            loanVM.loan = await _context.Loans
                .FirstOrDefaultAsync(m => m.Id == id);
            loanVM.PreviousUrl = Request.Headers["Referer"].ToString();
            if (loanVM.loan == null)
            {
                return NotFound();
            }

            return View(loanVM);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id,string PreviousUrl)
        {
            var loan = await _context.Loans.FindAsync(id);
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();            
            return Redirect(PreviousUrl);
        }

        private bool LoanExists(string id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
