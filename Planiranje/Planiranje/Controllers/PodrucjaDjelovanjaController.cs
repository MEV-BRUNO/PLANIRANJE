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
    public class PodrucjaDjelovanjaController : Controller
    {
        private Podrucje_rada_DBHandle podrucja_djelovanja = new Podrucje_rada_DBHandle();
        
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled područja djelovanja";

			PodrucjaRadaModel model = new PodrucjaRadaModel();
			model.podrucjaRada = podrucja_djelovanja.ReadPodrucjeRada();
			return View("Index", model);
        }

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
				Podrucje_rada podrucje = new Podrucje_rada();
                return PartialView("NoviPlan", podrucje);
            }
			return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult NoviPlan(Podrucje_rada podrucje)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (podrucje.Naziv != null && podrucja_djelovanja.CreatePodrucjeRada(podrucje))
            {
				return RedirectToAction("Index");
			}
            else
            {
				return PartialView("NoviPlan", podrucje);
			}
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Podrucje_rada podrucje = new Podrucje_rada();
            podrucje = podrucja_djelovanja.ReadPodrucjeRada(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", podrucje);
            }
            return View("Uredi", podrucje);
        }

        [HttpPost]
        public ActionResult Edit(Podrucje_rada podrucje)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (podrucje.Naziv != null && podrucja_djelovanja.UpdatePodrucjeRada(podrucje))
            {
				return RedirectToAction("Index");
			}
            else
            {
				return PartialView("NoviPlan", podrucje);
			}
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
				ViewBag.ErrorMessage = null;
				Podrucje_rada podrucje = new Podrucje_rada();
				podrucje = podrucja_djelovanja.ReadPodrucjeRada(id);
				return View("Obrisi", podrucje);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Delete(Podrucje_rada podrucje)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!podrucja_djelovanja.DeletePodrucjeRada(podrucje.Id_podrucje))
            {
				ViewBag.ErrorMessage = "Dogodila se greška, nije moguće obrisati podrucje djelovanja!";
				return View("Obrisi", podrucje);
			}
            else
            {
				return RedirectToAction("Index");
			}
        }
    }
}