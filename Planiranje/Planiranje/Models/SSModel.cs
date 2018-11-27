using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
	public class SSModel
	{
        public string Tekst { get; set; }
		public int ID_GODINA { get; set; }
		public int ID_PLAN { get; set; }
		public List<SS_Plan> SS_Planovi { get; set; }		
		public List<SS_Plan_podrucje> SS_Podrucja { get; set;  }
		public SS_Plan SS_Plan { get; set; }
		public SS_Plan_podrucje SS_Plan_Podrucje { get; set; }
		public int Ak_godina { get; set; }
        public List<Sk_godina> SkGodina { get; set; }
		public List<Podrucje_rada> PodrucjeRada { get; set; }
        public List<Oblici> Oblici { get; set; }
        public List<Subjekti> Subjekti { get; set; }
        public List<Ciljevi> Ciljevi { get; set; }
        public List<Zadaci> Zadaci { get; set; }
        public IEnumerable<SelectListItem> SubjektiItems
        {
            get { return new SelectList(Subjekti, "Naziv", "Naziv"); }
        }
        public IEnumerable<SelectListItem> PodrucjaItems
        {
            get { return new SelectList(PodrucjeRada, "Naziv", "Naziv"); }
        }
        public IEnumerable<SelectListItem> ObliciItems
        {
            get { return new SelectList(Oblici, "Naziv", "Naziv"); }
        }
        public IEnumerable<SelectListItem> CiljeviItems
        {
            get { return new SelectList(Ciljevi, "Naziv", "Naziv"); }
        }
        public IEnumerable<SelectListItem> SKGodinaItems
        {
            get { return new SelectList(SkGodina, "sk_godina", "sk_godina"); }
        }
        public IEnumerable<SelectListItem> ZadaciItems
        {
            get { return new SelectList(Zadaci, "Naziv", "Naziv"); }
        }
    }
}