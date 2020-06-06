using LoMan.Models;
using System.Collections.Generic;

namespace LoMan.ViewModels
{
    public class DashboardVM
    {
        public Dashboard dashboard { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
