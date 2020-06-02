using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoMan.Models
{
    public class Analytics
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public float Principle { get; set; }
        public float Interest { get; set; }
        public float Ppercent { get; set; }
        public float Ipercent { get; set; }
    }
}
