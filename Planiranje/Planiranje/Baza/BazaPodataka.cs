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

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

	}
}