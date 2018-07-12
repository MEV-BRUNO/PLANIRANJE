using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
    public class PlanOs1View
    {
        public int Id { get; set; }
        public OS_Plan_1 OsPlan1 { get; set; }
        public OS_Plan_1_podrucje Podrucje { get; set; }
        public List<OS_Plan_1_podrucje> OsPlan1Podrucje { get; set; }
        public List<OS_Plan_1_aktivnost> OsPlan1Aktivnost { get; set; }
        public List<OS_Plan_1_akcija> OsPlan1Akcija { get; set; }
        public List<Ciljevi> Ciljevi { get; set; }
        public List<Podrucje_rada> PodrucjeRada { get; set; }
        public List<Aktivnost> Aktivnosti { get; set; }
        
        public IEnumerable<SelectListItem> PodrucjeItems
        {
            get { return new SelectList(PodrucjeRada, "Id_podrucje", "Naziv"); }
        }
        public IEnumerable<SelectListItem> CiljeviItems
        {
            get { return new SelectList(Ciljevi, "Id_cilj", "Naziv"); }
        }
    }
}