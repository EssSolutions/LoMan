using LoMan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoMan.ViewModels
{
    public class LoanVM
    {
        public Loan loan { get; set; }
        public string PreviousUrl { get; set; }
    }
}
