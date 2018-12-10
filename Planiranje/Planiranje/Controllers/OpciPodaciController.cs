using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models.Ucenici;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class OpciPodaciController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        public ActionResult RazredniOdjel()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            List<RazredniOdjel> odjeli = new List<RazredniOdjel>();
            odjeli = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola &&
            w.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId).ToList();
            return View(odjeli);
        }
    }
}