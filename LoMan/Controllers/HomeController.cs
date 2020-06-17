using LoMan.Data;
using LoMan.Models;
using LoMan.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace LoMan.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            
            _db = db;
        }

        public IActionResult Index()
        {
            _ = _db.Database.ExecuteSqlRaw("EXEC Set_Dashboard");
            foreach (Loan item in _db.Loans)
            {
                if (item.Rdate < DateTime.Today)
                {
                    if (item.Status.Equals("Not Paid"))
                    {
                        item.Status = "Pending";                        
                    }
                    
                }
            }
            _db.SaveChanges();
            DashboardVM dashboardVM = new DashboardVM
            {
                TdLoans = _db.Loans.Where(l => l.Rdate == DateTime.Today).ToList(),
                dashboard = _db.Dashboard.FirstOrDefault(),
                TmLoans = _db.Loans.Where(l => l.Rdate == DateTime.Today.AddDays(1)).ToList()
            };
            return View(dashboardVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
