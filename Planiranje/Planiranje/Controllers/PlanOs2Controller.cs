using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class PlanOs2Controller : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        private OS_Plan_2_DBHandle planovi_os2 = new OS_Plan_2_DBHandle();
        private Ciljevi_DBHandle ciljevi_db = new Ciljevi_DBHandle();
        private Podrucje_rada_DBHandle podrucje_rada_db = new Podrucje_rada_DBHandle();
        private Aktivnost_DBHandle aktivnost_db = new Aktivnost_DBHandle();

        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled - osnovna skola 1";

            List<OS_Plan_2> planovi = new List<OS_Plan_2>();
            planovi = planovi_os2.ReadOS_Plan_2();
            return View("Index", planovi);
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
        public ActionResult NoviPlan(OS_Plan_2 gr)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2 os_plan = new OS_Plan_2();
            os_plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            os_plan.Ak_godina = gr.Ak_godina;
            os_plan.Naziv = gr.Naziv;
            os_plan.Opis = gr.Opis;
            if (planovi_os2.CreateOS_Plan_2(os_plan))
            {
                TempData["alert"] = "<script>alert('Novi plan za osnovnu skolu 2 je uspjesno spremljen!');</script>";
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
            OS_Plan_2 os_plan_2 = new OS_Plan_2();
            os_plan_2 = planovi_os2.ReadOS_Plan_2(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", os_plan_2);
            }
            return View("Uredi", os_plan_2);
        }
        [HttpPost]
        public ActionResult Edit(OS_Plan_2 os_plan_2)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_os2.UpdateOS_Plan_2(os_plan_2))
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
            OS_Plan_2 os_plan_2 = new OS_Plan_2();
            os_plan_2 = planovi_os2.ReadOS_Plan_2(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", os_plan_2);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(OS_Plan_2 os_plan_1)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_os2.DeleteOS_Plan_2(os_plan_2.Id_plan))
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
            List<OS_Plan_2> planovi = planovi_os2.ReadOS_Plan_2();

            PlanOs2Report report = new PlanOs2Report(planovi);

            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }

        public ActionResult Details(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }

            List<OS_Plan_2_podrucje> podrucja = new List<OS_Plan_2_podrucje>();
            podrucja = baza.OsPlan2Podrucje.Where(izraz => izraz.Id_glavni_plan == id).ToList();

            PlanOs2View plan = new PlanOs2View();
            OS_Plan_2 p = new OS_Plan_2();
            p = planovi_os2.ReadOS_Plan_2(id);

            List<Podrucje_rada> pod_rada = new List<Podrucje_rada>();
            pod_rada = podrucje_rada_db.ReadPodrucjeRada();
            plan.PodrucjeRada = pod_rada;

            List<Ciljevi> ciljevi = new List<Ciljevi>();
            ciljevi = ciljevi_db.ReadCiljevi();
            plan.Ciljevi = ciljevi;

            podrucja = podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            plan.OsPlan2 = p;
            plan.OsPlan2Podrucje = podrucja;


            /*dodatno*/
            List<Podrucje_rada> pr = new List<Podrucje_rada>();
            foreach (var i in podrucja)
            {
                Podrucje_rada pod = new Podrucje_rada();
                pod = podrucje_rada_db.ReadPodrucjeRada(i.Opis_Podrucje);
                pr.Add(pod);
            }


            List<Aktivnost> aktivnosti = new List<Aktivnost>();
            aktivnosti = aktivnost_db.ReadAktivnost();
            plan.Aktivnosti = aktivnosti;

            List<OS_Plan_2_aktivnost> osPlan2Aktivnosti = new List<OS_Plan_2_aktivnost>();
            if (podrucja.Count != 0)
            {
                int id_pod = podrucja.ElementAt(0).Id_plan;

                osPlan2Aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == id_pod).ToList();
            }
            plan.OsPlan2Aktivnost = osPlan2Aktivnosti;

            OS_Plan_2_aktivnost ak = new OS_Plan_2_aktivnost();
            if (podrucja.Count != 0)
            {
                ak.Id_podrucje = podrucja.ElementAt(0).Id_plan;
                plan.Id = podrucja.ElementAt(0).Id_plan;
            }
            plan.Os_Plan_2_Aktivnost = ak;


            return View("Details", plan);
        }






    }
}