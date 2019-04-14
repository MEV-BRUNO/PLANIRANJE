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
    public class DokumentController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: Dokument
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Dokument> dokumenti = baza.Dokument.Where(w => w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && 
            w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
            return View(dokumenti);
        }
    }
}