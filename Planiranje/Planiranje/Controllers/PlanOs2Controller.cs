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
        private OS_Plan_2_DBHandle planovi_os2 = new OS_Plan_2_DBHandle();
        private BazaPodataka baza = new BazaPodataka();
        private Ciljevi_DBHandle ciljevi_db = new Ciljevi_DBHandle();
        private Zadaci_DBHandle zadaci_db = new Zadaci_DBHandle();
        private Subjekt_DBHandle subjekti_db = new Subjekt_DBHandle();
        private Oblici_DBHandle oblici_db = new Oblici_DBHandle();

        
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<OS_Plan_2> planovi = new List<OS_Plan_2>();
            planovi = planovi_os2.ReadOS_Plan_2();
            return View(planovi);
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
			OS_Plan_2 os_plan_2 = new OS_Plan_2();
            os_plan_2.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            os_plan_2.Ak_godina = gr.Ak_godina;
            os_plan_2.Naziv = gr.Naziv;
            os_plan_2.Opis = gr.Opis;
            if (planovi_os2.CreateOS_Plan_2(os_plan_2))
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
        public ActionResult Delete(OS_Plan_2 os_plan_2)
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
        public ActionResult Details(int id, int? pA)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2 = new OS_Plan_2();
            plan.OsPlan2 = planovi_os2.ReadOS_Plan_2(id);
            plan.OsPlan2Podrucja = new List<OS_Plan_2_podrucje>();
            plan.OsPlan2Podrucja = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
            plan.OsPlan2Podrucja = plan.OsPlan2Podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            plan.Ciljevi = new List<Ciljevi>();
            plan.Ciljevi = ciljevi_db.ReadCiljevi();
            plan.Subjekti = new List<Subjekti>();
            plan.Subjekti = subjekti_db.ReadSubjekti();
            plan.Zadaci = new List<Zadaci>();
            plan.Zadaci = zadaci_db.ReadZadaci();
            plan.Oblici = new List<Oblici>();
            plan.Oblici = oblici_db.ReadOblici();
            if (pA == null)
            {
                pA = 0;
            }
            else
            {
                TempData["prikaz"] = "1";
            }
            int parametar = (Int32)pA;
            int idPod = plan.OsPlan2Podrucja.ElementAt(parametar).Id_plan;
            plan.OsPlan2Aktivnosti = new List<OS_Plan_2_aktivnost>();
            plan.OsPlan2Aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPod).ToList();
            plan.OsPlan2Aktivnosti = plan.OsPlan2Aktivnosti.OrderBy(o => o.Red_br_aktivnost).ToList();
            plan.Pozicija = parametar;

            return View(plan);
        }
        public ActionResult NoviPosao(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2 = new OS_Plan_2();
            plan.OsPlan2 = planovi_os2.ReadOS_Plan_2(id);
            plan.Ciljevi = new List<Ciljevi>();
            plan.Ciljevi = ciljevi_db.ReadCiljevi();
            plan.Subjekti = new List<Subjekti>();
            plan.Subjekti = subjekti_db.ReadSubjekti();
            plan.Zadaci = new List<Zadaci>();
            plan.Zadaci = zadaci_db.ReadZadaci();
            plan.Oblici = new List<Oblici>();
            plan.Oblici = oblici_db.ReadOblici();
            return View(plan);
        }
        [HttpPost]
        public ActionResult NoviPosao (PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            plan.OsPlan2Podrucje.Id_glavni_plan = plan.OsPlan2.Id_plan;
            int _id = plan.OsPlan2.Id_plan;
            int br;
            List<OS_Plan_2_podrucje> trenutni = new List<OS_Plan_2_podrucje>();
            trenutni = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == _id).ToList();
            if (trenutni.Count == 0)
            {
                br = 1;
            }
            else
            {
                br = trenutni.Max(m => m.Red_br_podrucje);
                br++;
            }
            plan.OsPlan2Podrucje.Red_br_podrucje = br;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Podrucje.Add(plan.OsPlan2Podrucje);
                    db.SaveChanges();
                    TempData["note"] = "Novi posao je dodan";
                }
                catch
                {
                    TempData["note"] = "Novi posao nije dodan.\nPobrinite se da sva polja pravilno ispunite";
                }
            }
            return RedirectToAction("Details", new { id = _id });
        }
        public ActionResult UrediPosao(int id, int broj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            plan.Ciljevi = new List<Ciljevi>();
            plan.Ciljevi = ciljevi_db.ReadCiljevi();
            plan.Subjekti = new List<Subjekti>();
            plan.Subjekti = subjekti_db.ReadSubjekti();
            plan.Zadaci = new List<Zadaci>();
            plan.Zadaci = zadaci_db.ReadZadaci();
            plan.Oblici = new List<Oblici>();
            plan.Oblici = oblici_db.ReadOblici();
            plan.Broj = broj;
            return View(plan);
        }
        [HttpPost]
        public ActionResult UrediPosao (PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_ = plan.OsPlan2Podrucje.Id_glavni_plan;
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Podrucje.Add(plan.OsPlan2Podrucje);
                    db.Entry(plan.OsPlan2Podrucje).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Posao je promijenjen";
                }
                catch
                {
                    TempData["note"] = "Posao nije promijenjen";
                }
            }
            return RedirectToAction("Details", new { id = id_ });
        }
        public ActionResult ObrisiPosao(int id, int broj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.Broj = broj;
            plan.OsPlan2Podrucje = new OS_Plan_2_podrucje();
            plan.OsPlan2Podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            plan.Ciljevi = new List<Ciljevi>();
            plan.Ciljevi.Add(ciljevi_db.ReadCiljevi(plan.OsPlan2Podrucje.Cilj));
            return View(plan);
        }
        [HttpPost]
        public ActionResult ObrisiPosao(PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = plan.OsPlan2Podrucje.Id_plan;
            int id_ = plan.OsPlan2Podrucje.Id_glavni_plan;
            using (var db = new BazaPodataka())
            {
                var result = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
                if (result != null)
                {
                    try
                    {
                        db.OsPlan2Podrucje.Remove(result);
                        db.SaveChanges();
                        TempData["note"] = "Posao je obrisan";
                    }
                    catch
                    {
                        TempData["note"] = "Posao nije obrisan";
                    }
                }
                else
                {
                    TempData["note"] = "Posao nije obrisan. Dogodila se greška";
                }
            }
            return RedirectToAction("Details", new { id = id_ });
        }
        public ActionResult PosaoPomakGore(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            int id_ = podrucje.Id_glavni_plan;

            int pozicija = podrucje.Red_br_podrucje;
            List<OS_Plan_2_podrucje> pod = new List<OS_Plan_2_podrucje>();
            pod = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id_ && w.Red_br_podrucje <= pozicija).ToList();            

            if (pod.Count == 1)
            {
                return RedirectToAction("Details", new { id = id_ });
            }
            pod = pod.OrderBy(o => o.Red_br_podrucje).ToList();
            int idPrethodni = pod.ElementAt(pod.Count - 2).Id_plan;
            int pozicijaPrethodni = pod.ElementAt(pod.Count - 2).Red_br_podrucje;
            using(var db = new BazaPodataka())
            {
                var result = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
                var result2 = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPrethodni);
                if(result!=null && result2 != null)
                {
                    try
                    {
                        result.Red_br_podrucje = pozicijaPrethodni;
                        result2.Red_br_podrucje = pozicija;
                        db.SaveChanges();
                        TempData["note"] = "Posao je pomaknut za jedno mjesto gore";
                    }
                    catch
                    {
                        TempData["note"] = "Dogodila se greška. Posao nije pomaknut";
                    }
                }
            }

            return RedirectToAction("Details", new { id = id_ });
        }
        public ActionResult PosaoPomakDolje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            int idGlavniPlan = podrucje.Id_glavni_plan;
            int pozicija = podrucje.Red_br_podrucje;
            List<OS_Plan_2_podrucje> pod = new List<OS_Plan_2_podrucje>();
            pod = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == idGlavniPlan && w.Red_br_podrucje >= pozicija).ToList();
            if (pod.Count == 1)
            {
                return RedirectToAction("Details", new { id = idGlavniPlan });
            }
            pod = pod.OrderBy(o => o.Red_br_podrucje).ToList();
            int idNakon = pod.ElementAt(1).Id_plan;
            int pozicijaNakon = pod.ElementAt(1).Red_br_podrucje;
            using(var db = new BazaPodataka())
            {
                var result = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
                var result1 = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idNakon);
                if(result!=null && result1 != null)
                {
                    try
                    {
                        result.Red_br_podrucje = pozicijaNakon;
                        result1.Red_br_podrucje = pozicija;
                        db.SaveChanges();
                        TempData["note"] = "Posao je pomaknut za jedno mjesto prema dolje";
                    }
                    catch
                    {
                        TempData["note"] = "Dogodila se greška. Posao nije pomaknut";
                    }
                }
            }
            return RedirectToAction("Details", new { id = idGlavniPlan });
        }
        public ActionResult NoviZadatak (int id, int pozicija)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.Pozicija = pozicija;
            plan.Id = id;
            return View(plan);
        }
        [HttpPost]
        public ActionResult NoviZadatak(PlanOs2View plan)
        {
            List<OS_Plan_2_podrucje> pod = new List<OS_Plan_2_podrucje>();
            int id = plan.Id;
            pod = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
            pod = pod.OrderBy(o => o.Red_br_podrucje).ToList();

            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = pod.ElementAt(plan.Pozicija);

            plan.OsPlan2Aktivnost.Id_podrucje = podrucje.Id_plan;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Aktivnost.Add(plan.OsPlan2Aktivnost);
                    db.SaveChanges();
                    TempData["note"] = "Novi zadatak je dodan";
                }
                catch
                {
                    TempData["note"] = "Novi zadatak nije dodan";
                }
            }
            return RedirectToAction("Details", new { id = plan.Id, pA = plan.Pozicija });
        }
	}
}