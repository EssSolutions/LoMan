using LoMan.Data;
using LoMan.Models;
using LoMan.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace LoMan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            _ = _db.Database.ExecuteSqlRaw("EXEC Set_Dashboard");
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.Loans = _db.Loans.Where(l => l.Rdate == DateTime.Today).ToList();
            dashboardVM.dashboard = _db.Dashboard.FirstOrDefault();
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
