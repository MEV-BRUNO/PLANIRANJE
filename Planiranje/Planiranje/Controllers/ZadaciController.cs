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
    public class ZadaciController : Controller
    {
        private Zadaci_DBHandle zadaci = new Zadaci_DBHandle();

        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled zadataka";
			ZadaciModel model = new ZadaciModel();
			model.zadaci = zadaci.ReadZadaci();
			return View("Index", model);
        }

        public ActionResult NoviZadatak()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
				ZadaciModel model = new ZadaciModel();
                return View("NoviZadatak", model);
            }
			return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult NoviZadatak(ZadaciModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.zadatak.Naziv != null && zadaci.CreateZadaci(model.zadatak))
            {
				return RedirectToAction("Index");
			}
            else
            {
				return View("NoviZadatak", model);
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
                ViewBag.IsUpdate = false;
                return View("Uredi", zadatak);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Edit(ZadaciModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.zadatak.Naziv != null && zadaci.UpdateZadaci(model.zadatak))
            {
				return RedirectToAction("Index");
			}
            else
            {
				return View("Uredi", model);
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
                ViewBag.IsUpdate = false;
                return View("Obrisi", zadatak);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Delete(ZadaciModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!zadaci.DeleteZadaci(model.zadatak.ID_zadatak))
            {
				ViewBag.ErrorMessage = "Dogodila se greška, nije moguće obrisati zadatak!";
				return View("Obrisi", model);
			}
            else
            {
				return RedirectToAction("Index");
			}
        }
    }
}