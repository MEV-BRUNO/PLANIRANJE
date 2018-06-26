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
        int Page_No_Master = 1;
        // GET: Ciljevi
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled ciljeva";
            ViewBag.CurrentSortOrder = Sort;
            ViewBag.SortingName = String.IsNullOrEmpty(Sort) ? "Naziv" : "";

            ViewBag.Message = "Grad";

            ViewBag.FilterValue = Search;
            if (Search != null)
            {
                Page_No = 1;
            }
            else
            {
                Search = Filter;
            }
            ViewBag.CurrentPage = 1;
            if (Page_No != null)
                ViewBag.CurrentPage = Page_No;


            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            if (Search == null || Search.Length == 0)
            {

                if (Request.IsAjaxRequest())
                {
                    int noP = (int)Page_No_Master;
                    var Popis2 = ciljevi_DB.ReadCiljevi().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = ciljevi_DB.ReadCiljevi().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = ciljevi_DB.ReadCiljevi(Search).ToPagedList(No_Of_Page, Size_Of_Page);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_GradView", Popis);
                }

                return View(Popis);
            }
        }

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("NoviPlan");
            }
            return View("NoviPlan");
        }

        [HttpPost]
        public ActionResult NoviPlan(Ciljevi cilj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            if (ciljevi_DB.CreateCiljevi(cilj))
            {
                TempData["alert"] = "<script>alert('Novi cilj je uspjesno spremljen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Novi cilj nije spremljen');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ciljevi cilj = new Ciljevi();
            cilj = ciljevi_DB.ReadCiljevi(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", cilj);
            }
            return View("Uredi", cilj);
        }

        [HttpPost]
        public ActionResult Edit(Ciljevi cilj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ciljevi_DB.UpdateCiljevi(cilj))
            {
                TempData["alert"] = "<script>alert('Cilj nije promijenjen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Cilj je uspjesno promijenjen!');</script>";
            }
            return RedirectToAction("Index","Ciljevi");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ciljevi cilj = new Ciljevi();
            cilj = ciljevi_DB.ReadCiljevi(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", cilj);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Ciljevi cilj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ciljevi_DB.DeleteCiljevi(cilj.ID_cilj))
            {
                TempData["alert"] = "<script>alert('Cilj nije obrisan, dogodila se greska!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Cilj je uspjesno obrisan!');</script>";
            }
            return RedirectToAction("Index");
        }
    }
}