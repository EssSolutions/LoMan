using LoMan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoMan.ViewModels
{
    public class DashboardVM
    {
        public Dashboard dashboard { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
