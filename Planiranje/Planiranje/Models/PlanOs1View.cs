using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
    public class PlanOs1View
    {
        /// <summary>
        /// id plana
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// glavni model za os plan 1
        /// </summary>
        public OS_Plan_1 OsPlan1 { get; set; }
        /// <summary>
        /// glavni model za os plan 1 područje
        /// </summary>
        public OS_Plan_1_podrucje Podrucje { get; set; }
        /// <summary>
        /// lista os plan 1 područja
        /// </summary>
        public List<OS_Plan_1_podrucje> OsPlan1Podrucje { get; set; }
        /// <summary>
        /// lista os plan 1 aktivnosti po područjima
        /// </summary>
        public List<OS_Plan_1_aktivnost> OsPlan1Aktivnost { get; set; }
        public List<OS_Plan_1_akcija> OsPlan1Akcija { get; set; }
        /// <summary>
        /// lista svih ciljeva
        /// </summary>
        public List<Ciljevi> Ciljevi { get; set; }
        /// <summary>
        /// lista svih područja djelovanja
        /// </summary>
        public List<Podrucje_rada> PodrucjeRada { get; set; }
        /// <summary>
        /// lista svih aktivnosti
        /// </summary>
        public List<Aktivnost> Aktivnosti { get; set; }
        /// <summary>
        /// lista svih područja za dropdown list
        /// </summary>
        public IEnumerable<SelectListItem> PodrucjeItems
        {
            get { return new SelectList(PodrucjeRada, "Id_podrucje", "Naziv"); }
        }
        /// <summary>
        /// lista svih ciljeva za dropdown list
        /// </summary>
        public IEnumerable<SelectListItem> CiljeviItems
        {
            get { return new SelectList(Ciljevi, "Id_cilj", "Naziv"); }
        }
        /// <summary>
        /// lista svih aktivnosti za dropdown list
        /// </summary>
        public IEnumerable<SelectListItem> AktivnostiItems
        {
            get { return new SelectList(Aktivnosti, "Id_aktivnost", "Naziv"); }
        }
        /// <summary>
        /// glavni model za os_plan_1_aktivnost
        /// </summary>
        public OS_Plan_1_aktivnost Os_Plan_1_Aktivnost { get; set; }
        /// <summary>
        /// pomoćni property
        /// </summary>
        public int Broj { get; set; }
        /// <summary>
        /// označava poziciju područja u listi za izbor područja u select elementu aktivnosti
        /// </summary>
        public int Pozicija { get; set; }
        public List<Sk_godina> SkolskaGodina { get; set; }
        public IEnumerable<SelectListItem> SkGodinaItems
        {
            get { return new SelectList(SkolskaGodina, "Sk_Godina", "Sk_Godina"); }
        }
    }
}