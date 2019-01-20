using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models.Ucenici;
using Planiranje.Models;
using Planiranje.Reports;
using System.Net;
using System.IO;

namespace Planiranje.Controllers
{
    public class PopisUcenikaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: PopisUcenika
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Sk_godina> godine = baza.SkolskaGodina.ToList();
            ViewBag.godine = godine;
            return View();
        }      
        public ActionResult PrikazUcenika(int razred)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PopisUcenikaModel model = new PopisUcenikaModel();
            model.Ucenici = (from raz in baza.RazredniOdjel
                             join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                             join uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik
                             where raz.Id == razred
                             select uc).ToList();
            model.Obitelji = (from ob in baza.Obitelj join uc in baza.Ucenik on ob.Id_ucenik equals uc.Id_ucenik join
                              ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik join raz in baza.RazredniOdjel
                              on ur.Id_razred equals raz.Id where raz.Id==razred && (ob.Svojstvo=="Otac" || ob.Svojstvo=="Majka") select ob).ToList();
            model.PopisUcenika = (from popis in baza.PopisUcenika
                                  join ur in baza.UcenikRazred on popis.Id_ucenik_razred equals ur.Id
                                  join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                  select popis).ToList();
            model.Ucenik_razred = baza.UcenikRazred.Where(w => w.Id_razred == razred).ToList();
            model.Razred = baza.RazredniOdjel.SingleOrDefault(s => s.Id == razred);
            return View("Tablica", model);
        }
        public ActionResult UrediPopis(int id, int razred)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PopisUcenikaModel model = new PopisUcenikaModel();
            Ucenik_razred ur = baza.UcenikRazred.SingleOrDefault(s => s.Id_razred == razred && s.Id_ucenik == id);
            int idUcRaz = ur.Id;
            model.Popis = baza.PopisUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUcRaz);
            model.UcenikRazred = ur;
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediPopis (PopisUcenikaModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(model.Popis.Ponavlja_razred==0 || model.Popis.Putnik == 0)
            {
                return View(model);
            }            
            
            int id = model.Popis.Id;
            if (id == 0)
            {
                model.Popis.Id_ucenik_razred = model.UcenikRazred.Id;
            }
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.PopisUcenika.Add(model.Popis);
                    if (id != 0)
                    {
                        db.Entry(model.Popis).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("PrikazUcenika", new { razred = model.UcenikRazred.Id_razred });
        }
        public FileStreamResult Ispis (int id)
        {
            //ulazni parametar id je id razreda
            List<Popis_ucenika> popis = new List<Popis_ucenika>();
            popis = (from ur in baza.UcenikRazred
                     join p in baza.PopisUcenika on ur.Id equals p.Id_ucenik_razred
                     where ur.Id_razred == id
                     select p).ToList();
            RazredniOdjel razred = new RazredniOdjel();
            razred = baza.RazredniOdjel.Single(s => s.Id == id);
            List<Ucenik> ucenici = new List<Ucenik>();
            List<Obitelj> obitelji = new List<Obitelj>();
            ucenici = (from raz in baza.RazredniOdjel
                             join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                             join uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik
                             where raz.Id == id select uc).ToList();
            obitelji = (from ob in baza.Obitelj join uc in baza.Ucenik on ob.Id_ucenik equals uc.Id_ucenik join
                              ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik join raz in baza.RazredniOdjel
                              on ur.Id_razred equals raz.Id where raz.Id==id && (ob.Svojstvo=="Otac" || ob.Svojstvo=="Majka") select ob).ToList();
            Nastavnik razrednik = new Nastavnik();
            int idRazrednik = razred.Id_razrednik;
            int idSkola = razred.Id_skola;
            razrednik = baza.Nastavnik.Single(s => s.Id == idRazrednik);
            Skola skola = new Skola();
            skola = baza.Skola.Single(s => s.Id_skola == idSkola);
            List<Ucenik_razred> listaUR = baza.UcenikRazred.Where(w => w.Id_razred == id).ToList();
            PopisUcenikaReport report = new PopisUcenikaReport(popis, ucenici, skola, razrednik, razred, obitelji, listaUR);

            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
    }
}