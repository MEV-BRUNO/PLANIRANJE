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

        public DbSet<OS_Plan_1_akcija> OsPlan1Akcija { get; set; }
        public DbSet<OS_Plan_1_podrucje> OsPlan1Podrucje { get; set; }
        public DbSet<OS_Plan_1> OsPlan1 { get; set; }
        public DbSet<OS_Plan_1_aktivnost> OsPlan1Aktivnost { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}