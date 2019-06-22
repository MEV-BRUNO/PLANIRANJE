using Planiranje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId != 1)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db=new BazaPodataka())
            {
                List<Pedagog> lista = db.Pedagog.Where(w => w.Id_Pedagog > 1).OrderBy(o=>o.Ime).ThenBy(t=>t.Prezime).ToList();
                ViewBag.pedagogSkola = db.PedagogSkola.ToList();
                ViewBag.skola = db.Skola.ToList();
                return View(lista);
            }            
        }
        [HttpPost]
        public ActionResult Aktivan(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId != 1)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id == 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            using(var db=new BazaPodataka())
            {
                Pedagog pedagog = db.Pedagog.SingleOrDefault(s => s.Id_Pedagog == id);
                if (pedagog == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                if (pedagog.Aktivan)
                {
                    pedagog.Aktivan = false;
                }
                else
                {
                    pedagog.Aktivan = true;
                }
                db.SaveChanges();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}