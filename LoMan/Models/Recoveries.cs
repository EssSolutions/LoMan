using System;
using System.ComponentModel.DataAnnotations;

namespace LoMan.Models
{
    public class Recoveries
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public float Principle { get; set; }
        public float Interest { get; set; }
    }
}
