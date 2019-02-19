using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models.Ucenici;
using Planiranje.Models;
using System.Net;
using Planiranje.Reports;
using System.IO;

namespace Planiranje.Controllers
{
    public class RoditeljBiljeskaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: RoditeljBiljeska
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.godine = baza.SkolskaGodina.ToList();
            return View();
        }
        public ActionResult Detalji(int id, int godina)
        {
            //ulazni parametar id je id učenika   
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Roditelj_biljeska> biljeske = (from bilj in baza.RoditeljBiljeska join ur in baza.UcenikRazred on
                                               bilj.Id_ucenik_razred equals ur.Id join raz in baza.RazredniOdjel on
                                               ur.Id_razred equals raz.Id
                                               where raz.Sk_godina == godina && ur.Id_ucenik == id && bilj.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                               select bilj).ToList();
            List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == id && (w.Svojstvo == 1 || w.Svojstvo == 2 || w.Svojstvo == 3)).ToList();
            List<string> svojstvo = new List<string>() { "","Otac","Majka","Skrbnik" };
            Ucenik ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            if (ucenik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.svojstvo = svojstvo;
            ViewBag.roditelji = roditelji;
            ViewBag.ucenik = ucenik;
            return View(biljeske);
        }
        public ActionResult NovaBiljeska(int idUcenik, int godina, int id)
        {
            //ulazni parametar id je id bilješke
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(id==0 && godina > 0 && idUcenik>0)
            {
                Ucenik_razred UR = (from ur in baza.UcenikRazred
                                    join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                    where ur.Id_ucenik == idUcenik && raz.Sk_godina == godina
                                    select ur).FirstOrDefault();
                if (UR == null)
                {                   
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }                
                List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == idUcenik).ToList();
                IEnumerable<SelectListItem> select = new SelectList(roditelji, "Id_obitelj", "ImePrezime");
                ViewBag.ur = UR.Id;
                ViewBag.roditelji = select;
                return View();
            }
            else if (id > 0 && idUcenik>0)
            {
                Roditelj_biljeska model = baza.RoditeljBiljeska.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == idUcenik).ToList();
                IEnumerable<SelectListItem> select = new SelectList(roditelji, "Id_obitelj", "ImePrezime");
                ViewBag.ur = null;
                ViewBag.roditelji = select;
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }            
        }
        [HttpPost]
        public ActionResult NovaBiljeska (Roditelj_biljeska model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(string.IsNullOrWhiteSpace(model.Naslov) || model.Id_roditelj == 0)
            {
                int idUR = model.Id_ucenik_razred;
                Ucenik_razred UR = baza.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
                if (UR == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                int idUcenik = UR.Id_ucenik;
                List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == idUcenik).ToList();
                IEnumerable<SelectListItem> select = new SelectList(roditelji, "Id_obitelj", "ImePrezime");
                ViewBag.ur = UR.Id;
                ViewBag.roditelji = select;
                return View(model);
            }
            try
            {
                if (model.Id == 0)
                {
                    using (var db = new BazaPodataka())
                    {
                        model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        db.RoditeljBiljeska.Add(model);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (var db = new BazaPodataka())
                    {
                        model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        db.RoditeljBiljeska.Add(model);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK,"uspjesno");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
        }
        public ActionResult ObrisiBiljeska (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Roditelj_biljeska model = baza.RoditeljBiljeska.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int idRoditelj = model.Id_roditelj;
            Obitelj o = baza.Obitelj.SingleOrDefault(s => s.Id_obitelj == idRoditelj);
            if (o == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.ime = o.ImePrezime;
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiBiljeska(Roditelj_biljeska model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idUcenik=0, g=0;
            try
            {
                using (var db = new BazaPodataka())
                {
                    int id = model.Id;
                    var result = db.RoditeljBiljeska.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        int idUR = result.Id_ucenik_razred;
                        Ucenik_razred ur = db.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
                        idUcenik = ur.Id_ucenik;
                        g = (from urr in db.UcenikRazred
                             join raz in db.RazredniOdjel on urr.Id_razred equals raz.Id
                             where urr.Id == idUR
                             select raz.Sk_godina).FirstOrDefault();
                        db.RoditeljBiljeska.Remove(result);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Detalji",new { id=idUcenik, godina=g});
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
        }
    }
}