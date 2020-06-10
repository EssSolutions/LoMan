using System.ComponentModel.DataAnnotations;

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
        public int PreviousLoans { get; set; }
        public int Printerest { get; set; }
        public int Prprinciple { get; set; }

    }
}
