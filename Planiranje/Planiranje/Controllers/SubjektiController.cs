using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Planiranje.Controllers;
using PagedList;
using System.Web.Mvc;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class SubjektiController : Controller
    {
        private Subjekt_DBHandle _subjekti = new Subjekt_DBHandle();

        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled subjekata";

			SubjektiModel model = new SubjektiModel();
			model.subjekti = _subjekti.ReadSubjekti();
			return View("Index", model);
        }

        public ActionResult NoviSubjekt()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
				SubjektiModel model = new SubjektiModel();
                return View("NoviSubjekt", model);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult NoviSubjekt(SubjektiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.subjekt.Naziv != null && _subjekti.CreateSubjekti(model.subjekt))
            {
				return RedirectToAction("Index");
			}
            else
            {
				return View("NoviSubjekt", model);
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
				SubjektiModel model = new SubjektiModel();
				model.subjekt = _subjekti.ReadSubjekti(id);
                return View("Uredi", model);
			}
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Edit(SubjektiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.subjekt.Naziv != null && _subjekti.UpdateSubjekti(model.subjekt))
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
				SubjektiModel model = new SubjektiModel();
				model.subjekt = _subjekti.ReadSubjekti(id);
				return View("Obrisi", model);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Delete(SubjektiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!_subjekti.DeleteSubjekti(model.subjekt.ID_subjekt))
            {
				return View("Obrisi", model);
			}
            else
            {
				return RedirectToAction("Index");
			}
        }
    }
}