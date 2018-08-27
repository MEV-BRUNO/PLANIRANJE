using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Planiranje.Models;


namespace Planiranje.Controllers
{
    public class AkcijeController : Controller
    {
        private Aktivnost_akcija_DBHandle akcije_planovi = new Aktivnost_akcija_DBHandle();
		
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled planova akcije";

			AkcijeModel model = new AkcijeModel();
			model.akcije = akcije_planovi.ReadAktivnostAkcija();
			return View("Index", model);
        }
        public ActionResult NovaAkcija()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
				AkcijeModel model = new AkcijeModel();
				model.akcija = new Aktivnost_akcija();
				model.aktivnosti = akcije_planovi.ReadAktivnosti();
                return View("NovaAkcija", model);
            }
            return View("NovaAkcija");
        }

        [HttpPost]
        public ActionResult NovaAkcija(AkcijeModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.akcija.Naziv != null && akcije_planovi.CreateAktivnostAkcija(model.akcija))
            {
				return RedirectToAction("Index");
			}
            else
			{
				model.aktivnosti = akcije_planovi.ReadAktivnosti();
				return View("NovaAkcija", model);
			}
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
			{
				AkcijeModel model = new AkcijeModel();
				model.aktivnosti = akcije_planovi.ReadAktivnosti();
				model.akcija = akcije_planovi.ReadAktivnostAkcija(id);
				return View("Uredi", model);
            }
			return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(AkcijeModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.akcija.Naziv != null && akcije_planovi.UpdateAktivnostAkcija(model.akcija))
			{
				return RedirectToAction("Index");
			}
            else
			{
				model.aktivnosti = akcije_planovi.ReadAktivnosti();
				return View("Uredi", model);
			}
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost_akcija akcija_plan = new Aktivnost_akcija();
            akcija_plan = akcije_planovi.ReadAktivnostAkcija(id);
            if (Request.IsAjaxRequest())
            {
				ViewBag.ErrorMessage = null;
				return View("Obrisi", akcija_plan);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Aktivnost_akcija akcija_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!akcije_planovi.DeleteAktivnostAkcija(akcija_plan.Id_akcija))
            {
				ViewBag.ErrorMessage = "Dogodila se greška, nije moguće obrisati akciju!";
				return View("Obrisi", akcija_plan);
			}
            else
			{
				return RedirectToAction("Index");
			}
        }
    }
}