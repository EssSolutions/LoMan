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
    public class RecoveriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecoveriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recoveries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recoveries.OrderBy(r => r.Date).ToListAsync());
        }

        // GET: Recoveries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recoveries = await _context.Recoveries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recoveries == null)
            {
                return NotFound();
            }

            return View(recoveries);
        }

        

        // GET: Recoveries/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recoveries = await _context.Recoveries.FindAsync(id);
            if (recoveries == null)
            {
                return NotFound();
            }
            return View(recoveries);
        }

        // POST: Recoveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Date,Principle,Interest")] Recoveries recoveries)
        {
            if (id != recoveries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recoveries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecoveriesExists(recoveries.Id))
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
            return View(recoveries);
        }

        // GET: Recoveries/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recoveries = await _context.Recoveries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recoveries == null)
            {
                return NotFound();
            }

            return View(recoveries);
        }

        // POST: Recoveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var recoveries = await _context.Recoveries.FindAsync(id);
            _context.Recoveries.Remove(recoveries);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecoveriesExists(string id)
        {
            return _context.Recoveries.Any(e => e.Id == id);
        }
    }
}
