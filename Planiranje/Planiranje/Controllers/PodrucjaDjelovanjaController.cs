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
        int Page_No_Master = 1;
        // GET: PodrucjaDjelovanja
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled podrucja djelovanja";
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
                    var Popis2 = podrucja_djelovanja.ReadPodrucjeRada().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = podrucja_djelovanja.ReadPodrucjeRada().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = podrucja_djelovanja.ReadPodrucjeRada(Search).ToPagedList(No_Of_Page, Size_Of_Page);
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
        public ActionResult NoviPlan(Podrucje_rada podrucje)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //Podrucje_rada rad = new Podrucje_rada();
            //mjesecni_plan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            //mjesecni_plan.Ak_godina = _mjesecni_plan.Ak_godina;
            //mjesecni_plan.Naziv = _mjesecni_plan.Naziv;
            //mjesecni_plan.Opis = _mjesecni_plan.Opis;
            if (podrucja_djelovanja.CreatePodrucjeRada(podrucje))
            {
                TempData["alert"] = "<script>alert('Novo podrucje djelovanja je uspjesno spremljeno!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Novi podrucje djelovanja nije spremljeno');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
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
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!podrucja_djelovanja.UpdatePodrucjeRada(podrucje))
            {
                TempData["alert"] = "<script>alert('Podrucje djelovanja nije promjenjeno!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Podrucje djelovanja je uspjesno promjenjeno!');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Podrucje_rada podrucje = new Podrucje_rada();
            podrucje = podrucja_djelovanja.ReadPodrucjeRada(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", podrucje);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Podrucje_rada podrucje)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!podrucja_djelovanja.DeletePodrucjeRada(podrucje.Id_podrucje))
            {
                TempData["alert"] = "<script>alert('Podrucje djelovanja nije obrisano, dogodila se greska!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Podrucje djelovanja je uspjesno obrisano!');</script>";
            }
            return RedirectToAction("Index");
        }
    }
}