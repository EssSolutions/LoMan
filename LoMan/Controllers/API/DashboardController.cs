using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoMan.Data;
using LoMan.Models;
using LoMan.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoMan.Controllers.API
{
    [AllowAnonymous]
    [Route("api")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DashboardController( ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<DashboardVM>> Get()
        {
            _ = await _db.Database.ExecuteSqlRawAsync("EXEC Set_Dashboard");
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
                Loans = _db.Loans.Where(l => l.Rdate == DateTime.Today).ToList(),
                dashboard = _db.Dashboard.FirstOrDefault()
            };
            return dashboardVM;
        }
    }
}
