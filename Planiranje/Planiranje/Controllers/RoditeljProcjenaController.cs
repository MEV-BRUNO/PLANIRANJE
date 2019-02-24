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
    public class RoditeljProcjenaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: RoditeljProcjena
        public ActionResult Index()
        {
            List<Sk_godina> godine = baza.SkolskaGodina.ToList();
            ViewBag.godine = godine;
            return View();
        }
    }
}