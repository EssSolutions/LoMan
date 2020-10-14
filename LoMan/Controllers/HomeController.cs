using LoMan.Data;
using LoMan.Models;
using LoMan.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            DashboardVM dashboardVM = new DashboardVM
            {
                TdLoans = new List<Loan>(),
                TmLoans = new List<Loan>(),
                dashboard = new Dashboard()
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
