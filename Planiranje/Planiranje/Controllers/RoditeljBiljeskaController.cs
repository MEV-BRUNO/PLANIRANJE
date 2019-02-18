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
                ViewBag.ur = UR;
                ViewBag.roditelji = select;
                return View();
            }
            else if (id > 0 && idUcenik>0)
            {
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }            
        }
    }
}