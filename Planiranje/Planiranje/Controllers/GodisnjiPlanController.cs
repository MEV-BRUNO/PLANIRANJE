using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class GodisnjiPlanController : Controller
	{
		// GET: GodisnjiPlan
		public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
                List<Godisnji_plan> lista = new List<Godisnji_plan>();
                Godisnji_plan g = new Godisnji_plan
                {
                    Ak_godina = "2017/2018",
                    Br_dana_godina_odmor = 4,
                    God_fond_sati = 450,
                    Id_god = 3,
                    Id_pedagog = PlaniranjeSession.Trenutni.PedagogId,
                    Br_radnih_dana = 40,
                    Ukupni_rad_dana = 66
                };
                lista.Add(g);
				return View(lista);
			}
			return RedirectToAction("Prijava");
		}        
    }
}