using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
    public class PlanOs2View
    {
        public OS_Plan_2 OsPlan2 { get; set; }
        public OS_Plan_2_podrucje OsPlan2Podrucje { get; set; }
        public OS_Plan_2_aktivnost OsPlan2Aktivnost { get; set; }
        public OS_Plan_2_akcija OsPlan2Akcija { get; set; }
        public List<OS_Plan_2_podrucje> OsPlan2Podrucja { get; set; }
        public List<OS_Plan_2_aktivnost> OsPlan2Aktivnosti { get; set; }
        public List<OS_Plan_2_akcija> OsPlan2Akcije { get; set; }
        public List<Ciljevi> Ciljevi { get; set; }
        public List<Zadaci> Zadaci { get; set; }
        public List<Subjekti> Subjekti { get; set; }
        public List<Oblici> Oblici { get; set; }
        public IEnumerable<SelectListItem> CiljeviItems
        {
            get { return new SelectList(Ciljevi, "Naziv", "Naziv"); }
        }
        public IEnumerable<SelectListItem> ZadaciItems
        {
            get { return new SelectList(Zadaci, "Naziv", "Naziv"); }
        }
        public IEnumerable<SelectListItem> SubjektiItems
        {
            get { return new SelectList(Subjekti, "Naziv", "Naziv"); }
        }
        public IEnumerable<SelectListItem> ObliciItems
        {
            get { return new SelectList(Oblici, "Naziv", "Naziv"); }
        }
        [DisplayName("Redni broj")]
        public int Broj { get; set; }
        public int Pozicija { get; set; }
        public int Pozicija2 { get; set; }
        public int Id { get; set; }
        public string Tekst { get; set; }
    }
}