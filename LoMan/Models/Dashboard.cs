using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoMan.Models
{
    public class Dashboard
    {
        [Key]
        public int Id { get; set; }
        public int TotalLoans { get; set; }
        public int Tprinciple { get; set; }
        public int Tinterest { get; set; }
        public int MonthlyLoans { get; set; }
        public int Mprinciple { get; set; }
        public int Minterest { get; set; }
        public int Pincrement { get; set; }
        public int Iincrement { get; set; }
        public int MonthlyGrowth { get; set; }
    }
}
