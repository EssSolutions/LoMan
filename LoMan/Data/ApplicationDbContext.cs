using System;
using System.Collections.Generic;
using System.Text;
using LoMan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoMan.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Recoveries> Recoveries { get; set; }
        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<Dashboard> Dashboard { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Analytics>().HasKey(table => new { table.Month, table.Year });

            base.OnModelCreating(builder);
        }
    }
}
