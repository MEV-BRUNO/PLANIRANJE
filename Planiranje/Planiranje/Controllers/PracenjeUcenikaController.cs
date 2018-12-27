using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models.Ucenici;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class PracenjeUcenikaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: PracenjeUcenika
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.SkGodine = baza.SkolskaGodina.ToList();
            return View(model);
        }
        public ActionResult OdabirRazreda (int godina)
        {
            List<RazredniOdjel> razredi = new List<RazredniOdjel>();
            razredi = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola && w.Sk_godina == godina).ToList();
            return View(razredi);
        }
        public ActionResult OdabirUcenika (int razred)
        {
            RazredniOdjel razredniOdjel = baza.RazredniOdjel.SingleOrDefault(s => s.Id == razred && s.Id_skola==PlaniranjeSession.Trenutni.OdabranaSkola);
            List<Ucenik> ucenici = new List<Ucenik>();
            if (razredniOdjel == null)
            {
                return View(ucenici);
            }
            ucenici = (from ur in baza.UcenikRazred
                       join uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik
                       where ur.Id_razred == razred
                       select uc).ToList();
            return View(ucenici);
        }
    }
}