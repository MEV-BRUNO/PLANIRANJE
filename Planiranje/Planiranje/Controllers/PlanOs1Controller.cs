using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Planiranje.Models;
using Planiranje.Reports;

namespace Planiranje.Controllers
{
    public class PlanOs1Controller : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        private OS_Plan_1_DBHandle planovi_os1 = new OS_Plan_1_DBHandle();
        private Ciljevi_DBHandle ciljevi_db = new Ciljevi_DBHandle();
        private Podrucje_rada_DBHandle podrucje_rada_db = new Podrucje_rada_DBHandle();
        private Aktivnost_DBHandle aktivnost_db = new Aktivnost_DBHandle();        
        int Page_No_Master = 1;

        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled - osnovna skola 1";
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
                    var Popis2 = planovi_os1.ReadOS_Plan_1().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = planovi_os1.ReadOS_Plan_1().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = planovi_os1.ReadOS_Plan_1(Search).ToPagedList(No_Of_Page, Size_Of_Page);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_GradView", Popis);
                }

                return View(Popis);
            }
        }

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
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
        public ActionResult NoviPlan(OS_Plan_1 gr)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			OS_Plan_1 os_plan = new OS_Plan_1();
            os_plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            os_plan.Ak_godina = gr.Ak_godina;
            os_plan.Naziv = gr.Naziv;
            os_plan.Opis = gr.Opis;
            if (planovi_os1.CreateOS_Plan_1(os_plan))
			{
				TempData["alert"] = "<script>alert('Novi plan za osnovnu skolu 1 je uspjesno spremljen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Novi plan nije spremljen');</script>";
			}
			return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1 os_plan_1 = new OS_Plan_1();
            os_plan_1 = planovi_os1.ReadOS_Plan_1(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", os_plan_1);
            }
            return View("Uredi", os_plan_1);
        }
        [HttpPost]
        public ActionResult Edit(OS_Plan_1 os_plan_1)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_os1.UpdateOS_Plan_1(os_plan_1))
			{
				TempData["alert"] = "<script>alert('Plan nije promjenjen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Plan je uspjesno promjenjen!');</script>";
			}
			return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1 os_plan_1 = new OS_Plan_1();
			os_plan_1 = planovi_os1.ReadOS_Plan_1(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", os_plan_1);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(OS_Plan_1 os_plan_1)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_os1.DeleteOS_Plan_1(os_plan_1.Id_plan))
			{
				TempData["alert"] = "<script>alert('Plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Index");
		}

		public FileStreamResult Ispis()
		{
			List<OS_Plan_1> planovi = planovi_os1.ReadOS_Plan_1();

			PlanOs1Report report = new PlanOs1Report(planovi);

			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}

        public ActionResult Details(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            List<OS_Plan_1_podrucje> podrucja = new List<OS_Plan_1_podrucje>();
            podrucja = baza.OsPlan1Podrucje.Where(izraz => izraz.Red_br_podrucje == id).ToList();
            
            PlanOs1View plan = new PlanOs1View();
            OS_Plan_1 p = new OS_Plan_1();
            p = planovi_os1.ReadOS_Plan_1(id);

            List<Podrucje_rada> pod_rada = new List<Podrucje_rada>();
            pod_rada = podrucje_rada_db.ReadPodrucjeRada();
            plan.PodrucjeRada = pod_rada;

            List<Ciljevi> ciljevi = new List<Ciljevi>();
            ciljevi = ciljevi_db.ReadCiljevi();
            plan.Ciljevi = ciljevi;

            plan.OsPlan1 = p;
            plan.OsPlan1Podrucje = podrucja;
            
            return View("Details",plan);
        }
        
        public ActionResult NovoPodrucje(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Podrucje_rada> podrucje = new List<Podrucje_rada>();
            List<Ciljevi> ciljevi = new List<Ciljevi>();
            PlanOs1View plan = new PlanOs1View();
            podrucje = podrucje_rada_db.ReadPodrucjeRada();
            ciljevi = ciljevi_db.ReadCiljevi();
            plan.Ciljevi = ciljevi;
            plan.PodrucjeRada = podrucje;
            plan.Id = id;
            
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("NovoPodrucje",plan);
            }
            return View("NovoPodrucje",plan);
        }

        [HttpPost]
        public ActionResult NovoPodrucje(PlanOs1View plan)
        {
            //test
            plan.Podrucje.Mj_2 = 2;
            plan.Podrucje.Mj_3 = 2;
            plan.Podrucje.Mj_4 = 2;
            plan.Podrucje.Mj_5 = 2;
            plan.Podrucje.Mj_6 = 2;
            plan.Podrucje.Mj_7 = 2;
            plan.Podrucje.Mj_8 = 2;
            //kraj testa
            //int id = plan.Podrucje.Red_br_podrucje; 
            plan.Podrucje.Red_br_podrucje = plan.Id;
            int _id = plan.Podrucje.Red_br_podrucje;
            baza.OsPlan1Podrucje.Add(plan.Podrucje);
            baza.SaveChanges();
            return RedirectToAction("Details",new { id=_id});
        }
    }
}