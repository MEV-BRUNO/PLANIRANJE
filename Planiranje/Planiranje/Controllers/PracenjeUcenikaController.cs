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
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<RazredniOdjel> razredi = new List<RazredniOdjel>();
            razredi = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola && w.Sk_godina == godina).ToList();
            return View(razredi);
        }
        public ActionResult OdabirUcenika (int razred)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
        public ActionResult Detalji (int id, int godina)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId<=0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();            
            model.Ucenik = (from uc in baza.Ucenik join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik join
                            raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id where uc.Id_ucenik == id && raz.Id_skola == 
                            PlaniranjeSession.Trenutni.OdabranaSkola select uc).FirstOrDefault();
            if (model.Ucenik == null)
            {
                return HttpNotFound();
            }
            model.Razred = (from uc in baza.Ucenik join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik join
                            raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id where raz.Sk_godina == godina && uc.Id_ucenik==id
                            select raz).FirstOrDefault();
            if (model.Razred == null)
            {
                return HttpNotFound();
            }
            int idRazrednik = model.Razred.Id_razrednik;
            model.Razrednik = baza.Nastavnik.SingleOrDefault(s => s.Id == idRazrednik);
            if (model.Razred == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
    }
}