using System;
using System.ComponentModel.DataAnnotations;

namespace LoMan.Models
{
    public class Loan
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [MaxLength(10), MinLength(10)]
        public string Phone { get; set; }
        public string Asset { get; set; }
        public float Principle { get; set; }
        public float Interest { get; set; }
        public float Rate { get; set; }
        public float Amount { get; set; }
        [DataType("Date")]
        public DateTime Idate { get; set; }
        [DataType("Date")]
        public DateTime Rdate { get; set; }
        public float Penalty { get; set; }
        public int Times { get; set; }
        public int Period { get; set; }
        public string Status { get; set; }
    }
}
