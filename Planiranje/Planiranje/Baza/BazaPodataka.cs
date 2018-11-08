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
        public DbSet<OS_Plan_2> OsPlan2 { get; set; }
        public DbSet<OS_Plan_2_podrucje> OsPlan2Podrucje { get; set; }
        public DbSet<OS_Plan_2_aktivnost> OsPlan2Aktivnost { get; set; }
        public DbSet<OS_Plan_2_akcija> OsPlan2Akcija { get; set; }
        public DbSet<Sk_godina> SkolskaGodina { get; set; }
        public DbSet<Mjesecni_plan> MjesecniPlan { get; set; }
        public DbSet<Mjesecni_detalji> MjesecniDetalji { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}