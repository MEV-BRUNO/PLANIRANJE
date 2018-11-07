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
    public class CiljeviController : Controller
    {
        private Ciljevi_DBHandle ciljevi_DB = new Ciljevi_DBHandle();

        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled ciljeva";

			CIljeviModel model = new CIljeviModel();
			model.ciljevi = ciljevi_DB.ReadCiljevi();
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
				CIljeviModel model = new CIljeviModel();
                return View("NoviPlan", model);
            }
			return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult NoviPlan(CIljeviModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            if (model.cilj.Naziv != null && ciljevi_DB.CreateCiljevi(model.cilj))
            {
				return RedirectToAction("Index");
			}
            else
            {
				return View("NoviPlan", model);
			}
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", cilj);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Edit(CIljeviModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.cilj.Naziv != null && ciljevi_DB.UpdateCiljevi(model.cilj))
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
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", cilj);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Delete(CIljeviModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ciljevi_DB.DeleteCiljevi(model.cilj.ID_cilj))
            {
				ViewBag.ErrorMessage = "Dogodila se greška, nije moguće obrisati cilj!";
				return View("Obrisi", model);
			}
            else
            {
				return RedirectToAction("Index");
			}
        }
    }
}