using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Planiranje.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Planiranje.Controllers
{
    public class BazaPodataka : DbContext
    {
        public DbSet<Pedagog> Pedagog { get; set; }

        public DbSet<OS_Plan_2_akcija> OsPlan2Akcija { get; set; }
        public DbSet<OS_Plan_2_podrucje> OsPlan2Podrucje { get; set; }
        public DbSet<OS_Plan_2> OsPlan2 { get; set; }
        public DbSet<OS_Plan_2_aktivnost> OsPlan2Aktivnost { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}