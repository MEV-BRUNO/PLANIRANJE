using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Planiranje.Models;
using Planiranje.Models.Ucenici;
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
        public DbSet<SS_Plan> SSPlan { get; set; }
        public DbSet<SS_Plan_podrucje> SSPodrucje { get; set; }

        //pedagog i učenici
        public DbSet<RazredniOdjel> RazredniOdjel { get; set; }
        public DbSet<Pedagog_skola> PedagogSkola { get; set; }
        public DbSet<Skola> Skola { get; set; }
        public DbSet<Nastavnik> Nastavnik { get; set; }
        public DbSet<Ucenik> Ucenik { get; set; }
        public DbSet<Ucenik_razred> UcenikRazred { get; set; }
        public DbSet<Obitelj> Obitelj { get; set; }
        public DbSet<Pracenje_ucenika> PracenjeUcenika { get; set; }
        public DbSet<Postignuce> Postignuce { get; set; }
        public DbSet<Neposredni_rad> NeposredniRad { get; set; }
        public DbSet<Popis_ucenika> PopisUcenika { get; set; }
        public DbSet<Ucenik_biljeska> UcenikBiljeska { get; set; }
        public DbSet<Mjesecna_biljeska> MjesecnaBiljeska { get; set; }
        public DbSet<Promatranje_ucenika> PromatranjeUcenika { get; set; }
        public DbSet<Roditelj_biljeska> RoditeljBiljeska { get; set; }
        public DbSet<Roditelj_procjena> RoditeljProcjena { get; set; }
        public DbSet<Roditelj_razgovor> RoditeljRazgovor { get; set; }
        public DbSet<Roditelj_ugovor> RoditeljUgovor { get; set; }
        public DbSet<Nastavnik_analiza> NastavnikAnaliza { get; set; }
        public DbSet<Nastavnik_protokol> NastavnikProtokol { get; set; }
        public DbSet<Nastavnik_uvid> NastavnikUvid { get; set; }
        public DbSet<Nastavnik_obrazac> NastavnikObrazac { get; set; }
        public DbSet<Dokument> Dokument { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}