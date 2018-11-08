
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
    public class AktivnostiController : Controller
    {
        private Aktivnost_DBHandle aktivnosti = new Aktivnost_DBHandle();
		
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled aktivnosti";

			AktivnostiModel model = new AktivnostiModel();
			model.aktivnosti = aktivnosti.ReadAktivnost();
            return View("Index", model);
        }

        public ActionResult NovaAktivnost()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
				AktivnostiModel model = new AktivnostiModel();
                return View("NovaAktivnost", model);
            }
			return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult NovaAktivnost(AktivnostiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.aktivnost.Naziv != null && aktivnosti.CreateAktivnost(model.aktivnost))
            {
				return RedirectToAction("Index");
			}
            else
            {
				return View("NovaAktivnost", model);
			}
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
			AktivnostiModel model = new AktivnostiModel();
            model.aktivnost = aktivnosti.ReadAktivnost(id);
            if (Request.IsAjaxRequest() && model.aktivnost.Vrsta==PlaniranjeSession.Trenutni.PedagogId)
            {
                return View("Uredi", model);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Edit(AktivnostiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost a = aktivnosti.ReadAktivnost(model.aktivnost.Id_aktivnost);
            if (a.Vrsta != PlaniranjeSession.Trenutni.PedagogId)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.aktivnost.Naziv != null && aktivnosti.UpdateAktivnost(model.aktivnost))
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
            Aktivnost aktivnost = new Aktivnost();
            aktivnost = aktivnosti.ReadAktivnost(id);
            if (Request.IsAjaxRequest() && aktivnost.Vrsta==PlaniranjeSession.Trenutni.PedagogId)
            {
				ViewBag.ErrorMessage = null;
				return View("Obrisi", aktivnost);
			}
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Delete(Aktivnost aktivnost)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost a = aktivnosti.ReadAktivnost(aktivnost.Id_aktivnost);
            if (a.Vrsta != PlaniranjeSession.Trenutni.PedagogId)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!aktivnosti.DeleteAktivnost(aktivnost.Id_aktivnost))
            {
				ViewBag.ErrorMessage = "Dogodila se greška, nije moguće obrisati aktivnost!";
				return View("Obrisi", aktivnost);
			}
            else
            {
				return RedirectToAction("Index");
			}
        }
    }
}