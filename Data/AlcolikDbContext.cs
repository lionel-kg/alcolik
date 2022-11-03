using System.Diagnostics.Metrics;
using System;
using alcolikLib.Data;
using Microsoft.EntityFrameworkCore;
using alcolik.Model;

namespace alcolik.Data
{
    public class AlcolikDbContext : BaseDbContext
    {
        public DbSet<Alcool> Alcool { get; set; }

        public DbSet<Brand> Brand { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=alcolik;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
