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
        public ActionResult Details(int id, int? pA, int? pB)
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
            plan.OsPlan2Aktivnosti = new List<OS_Plan_2_aktivnost>();
            plan.OsPlan2Akcije = new List<OS_Plan_2_akcija>();
            if (plan.OsPlan2Podrucja.Count == 0)
            {
                return View(plan);
            }
            if (pA == null)
            {
                pA = 0;
            }
            else
            {
                TempData["prikaz"] = "1";
            }
            if (pB == null)
            {
                pB = 0;
            }
            else
            {
                TempData["prikaz"] = "2";
            }
            int parametar = (Int32)pA;
            int idPod = plan.OsPlan2Podrucja.ElementAt(parametar).Id_plan;

            int parametar2 = (Int32)pB;            

            plan.OsPlan2Aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPod).ToList();
            plan.OsPlan2Aktivnosti = plan.OsPlan2Aktivnosti.OrderBy(o => o.Red_br_aktivnost).ToList();
            plan.Pozicija = parametar;
            plan.Pozicija2 = parametar2;
            if (plan.OsPlan2Aktivnosti.Count == 0)
            {
                return View(plan);
            }
            int idAktivnost = plan.OsPlan2Aktivnosti.ElementAt(parametar2).Id_plan;
            plan.OsPlan2Akcije = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == idAktivnost).ToList();
            plan.OsPlan2Akcije = plan.OsPlan2Akcije.OrderBy(o => o.Red_br_akcija).ToList();
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
                    TempData["note"] = "Novi posao nije dodan. Pobrinite se da sva polja pravilno ispunite";
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
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.Pozicija = pozicija;
            plan.Id = id;
            return View(plan);
        }
        [HttpPost]
        public ActionResult NoviZadatak(PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<OS_Plan_2_podrucje> pod = new List<OS_Plan_2_podrucje>();
            int id = plan.Id;
            pod = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
            pod = pod.OrderBy(o => o.Red_br_podrucje).ToList();

            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = pod.ElementAt(plan.Pozicija);
            int maxValue;
            List<OS_Plan_2_aktivnost> aktivnosti = new List<OS_Plan_2_aktivnost>();
            aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == podrucje.Id_plan).ToList();
            if (aktivnosti.Count == 0)
            {
                maxValue = 1;
            }
            else
            {
                maxValue = aktivnosti.Max(m => m.Red_br_aktivnost)+1;                
            }
            plan.OsPlan2Aktivnost.Red_br_aktivnost = maxValue;
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
        public ActionResult ZadatakPomakGore (int id, int pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_aktivnost akt = new OS_Plan_2_aktivnost();
            akt = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            int idPodrucje = akt.Id_podrucje;
            int idTrenutni = akt.Id_plan;
            int pozTrenutni = akt.Red_br_aktivnost;

            OS_Plan_2_podrucje p = new OS_Plan_2_podrucje();
            p = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);

            List<OS_Plan_2_aktivnost> trenutne = new List<OS_Plan_2_aktivnost>();
            trenutne = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPodrucje && w.Red_br_aktivnost <= pozTrenutni).ToList();
            if (trenutne.Count == 1)
            {
                return RedirectToAction("Details", new { id = p.Id_glavni_plan, pA = pozicija });
            }
            trenutne = trenutne.OrderBy(o => o.Red_br_aktivnost).ToList();

            int idPrije = trenutne.ElementAt(trenutne.Count - 2).Id_plan;
            int pozPrije = trenutne.ElementAt(trenutne.Count - 2).Red_br_aktivnost;

            using(var db = new BazaPodataka())
            {
                var result = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idTrenutni);
                var result1 = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idPrije);
                if(result!=null && result1 != null)
                {
                    try
                    {
                        result.Red_br_aktivnost = pozPrije;
                        result1.Red_br_aktivnost = pozTrenutni;
                        db.SaveChanges();
                        TempData["note"] = "Zadatak je pomaknut za jedno mjesto prema gore";
                    }
                    catch
                    {
                        TempData["note"] = "Zadatak nije pomaknut";
                    }
                }
            }
            
            return RedirectToAction("Details", new { id=p.Id_glavni_plan, pA = pozicija});
        }
        public ActionResult ZadatakPomakDolje (int id, int pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_aktivnost aktivnost = new OS_Plan_2_aktivnost();
            aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            int idPodrucje = aktivnost.Id_podrucje;
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);

            int pozicijaTrenutni = aktivnost.Red_br_aktivnost;
            List<OS_Plan_2_aktivnost> trenutne = new List<OS_Plan_2_aktivnost>();
            trenutne = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPodrucje && w.Red_br_aktivnost >= pozicijaTrenutni).ToList();
            if (trenutne.Count == 1)
            {
                return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = pozicija });
            }
            trenutne = trenutne.OrderBy(o => o.Red_br_aktivnost).ToList();
            int idPoslije = trenutne.ElementAt(1).Id_plan;
            int pozicijaPoslije = trenutne.ElementAt(1).Red_br_aktivnost;
            using(var db=new BazaPodataka())
            {
                var result = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
                var result1 = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idPoslije);
                if(result!=null && result1 != null)
                {
                    try
                    {
                        result.Red_br_aktivnost = pozicijaPoslije;
                        result1.Red_br_aktivnost = pozicijaTrenutni;
                        db.SaveChanges();
                        TempData["note"] = "Zadatak je pomaknut za jedno mjesto prema dolje";
                    }
                    catch
                    {
                        TempData["note"] = "Zadatak nije pomaknut";
                    }
                }
            }
            return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = pozicija });
        }
        public ActionResult UrediZadatak(int id, int pozicija, string tekst)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Aktivnost = new OS_Plan_2_aktivnost();
            plan.OsPlan2Aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pozicija;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult UrediZadatak(PlanOs2View plan)
        {
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            int idPod = plan.OsPlan2Aktivnost.Id_podrucje;
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPod);

            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Aktivnost.Add(plan.OsPlan2Aktivnost);
                    db.Entry(plan.OsPlan2Aktivnost).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Zadatak je promijenjen";
                }
                catch
                {
                    TempData["note"] = "Zadatak nije promijenjen";
                }
            }
            return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = plan.Pozicija });
        }
        public ActionResult ObrisiZadatak (int id, int pozicija, string tekst)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Aktivnost = new OS_Plan_2_aktivnost();
            plan.OsPlan2Aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pozicija;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult ObrisiZadatak(PlanOs2View plan)
        {
            int id = plan.OsPlan2Aktivnost.Id_plan;
            int idPodrucje = plan.OsPlan2Aktivnost.Id_podrucje;
            OS_Plan_2_podrucje pod = new OS_Plan_2_podrucje();
            pod = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
            using(var db = new BazaPodataka())
            {
                var result = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
                if (result != null)
                {
                    try
                    {
                        db.OsPlan2Aktivnost.Remove(result);
                        db.SaveChanges();
                        TempData["note"] = "Zadatak je obrisan";
                    }
                    catch
                    {
                        TempData["note"] = "Zadatak nije obrisan";
                    }
                }
            }
            return RedirectToAction("Details", new { id = pod.Id_glavni_plan, pA = plan.Pozicija });
        }
        public ActionResult NovaAktivnost(int id,int pA, int pB)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.Pozicija = pA;
            plan.Pozicija2 = pB;
            plan.Id = id;
            return View(plan);
        }
        [HttpPost]
        public ActionResult NovaAktivnost(PlanOs2View plan)
        {            
            int idGlavniPlan = plan.Id;
            List<OS_Plan_2_podrucje> podrucja = new List<OS_Plan_2_podrucje>();
            podrucja = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == idGlavniPlan).ToList();
            podrucja = podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            OS_Plan_2_podrucje pod = new OS_Plan_2_podrucje();
            pod = podrucja.ElementAt(plan.Pozicija);
            int idPodrucje = pod.Id_plan;
            List<OS_Plan_2_aktivnost> aktivnosti = new List<OS_Plan_2_aktivnost>();
            aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPodrucje).ToList();
            aktivnosti = aktivnosti.OrderBy(o => o.Red_br_aktivnost).ToList();
            OS_Plan_2_aktivnost akt = new OS_Plan_2_aktivnost();
            akt = aktivnosti.ElementAt(plan.Pozicija2);
            int idAktivnost = akt.Id_plan;
            plan.OsPlan2Akcija.Id_aktivnost = akt.Id_plan;

            List<OS_Plan_2_akcija> akcije = new List<OS_Plan_2_akcija>();
            akcije = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == idAktivnost).ToList();
            int maxValue;
            if (akcije.Count == 0)
            {
                maxValue = 1;
            }
            else
            {
                maxValue = akcije.Max(m => m.Red_br_akcija);
                maxValue++;
            }
            plan.OsPlan2Akcija.Red_br_akcija = maxValue;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Akcija.Add(plan.OsPlan2Akcija);
                    db.SaveChanges();
                    TempData["note"] = "Nova aktivnost je dodana";
                }
                catch
                {
                    TempData["note"] = "Nova aktivnost nije dodana";
                }
            }
            return RedirectToAction("Details", new { id = pod.Id_glavni_plan, pA = plan.Pozicija, pB = plan.Pozicija2 });
        }
        public ActionResult UrediAktivnost(int id,int pA, int pB, string tekst)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Akcija = new OS_Plan_2_akcija();
            plan.OsPlan2Akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pA;
            plan.Pozicija2 = pB;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult UrediAktivnost(PlanOs2View plan)
        {
            int idAkcija = plan.OsPlan2Akcija.Id_plan;
            int idAktivnost = plan.OsPlan2Akcija.Id_aktivnost;
            OS_Plan_2_aktivnost aktivnost = new OS_Plan_2_aktivnost();
            aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
            int idPodrucje = aktivnost.Id_podrucje;
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Akcija.Add(plan.OsPlan2Akcija);
                    db.Entry(plan.OsPlan2Akcija).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Aktivnost je promijenjena";
                }
                catch
                {
                    TempData["note"] = "Aktivnost nije promijenjena";
                }
            }
            return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = plan.Pozicija, pB = plan.Pozicija2 });
        }
        public ActionResult ObrisiAktivnost(int id, int pA, int pB, string tekst)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Akcija = new OS_Plan_2_akcija();
            plan.OsPlan2Akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pA;
            plan.Pozicija2 = pB;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult ObrisiAktivnost(PlanOs2View plan)
        {
            int idAkcija = plan.OsPlan2Akcija.Id_plan;
            int idAktivnost = plan.OsPlan2Akcija.Id_aktivnost;
            OS_Plan_2_aktivnost aktivnost = new OS_Plan_2_aktivnost();
            aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
            int idPodrucje = aktivnost.Id_podrucje;
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
            using(var db=new BazaPodataka())
            {
                var result = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == idAkcija);
                try
                {
                    if (result != null)
                    {
                        db.OsPlan2Akcija.Remove(result);
                        db.SaveChanges();
                        TempData["note"] = "Aktivnost je obrisana";
                    }                    
                }
                catch
                {
                    TempData["note"] = "Aktivnost nije obrisana";
                }
            }
            return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = plan.Pozicija, pB = plan.Pozicija2 });
        }
        public ActionResult AktivnostPomakGore (int id, int pa, int pb)
        {
            OS_Plan_2_akcija akcija = new OS_Plan_2_akcija();
            akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            int idAktivnost = akcija.Id_aktivnost;
            OS_Plan_2_aktivnost aktivnost = new OS_Plan_2_aktivnost();
            aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
            int idPodrucje = aktivnost.Id_podrucje;
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);

            int pozicija = akcija.Red_br_akcija;
            List<OS_Plan_2_akcija> trenutne = new List<OS_Plan_2_akcija>();
            trenutne = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == idAktivnost && w.Red_br_akcija <= pozicija).ToList();
            if (trenutne.Count == 1)
            {
                return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = pa, pB = pb });
            }
            trenutne = trenutne.OrderBy(o => o.Red_br_akcija).ToList();
            int idPrije = trenutne.ElementAt(trenutne.Count - 2).Id_plan;
            int pozicijaPrije = trenutne.ElementAt(trenutne.Count - 2).Red_br_akcija;
            using(var db = new BazaPodataka())
            {
                var result = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
                var result1 = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == idPrije);
                try
                {
                    if(result!=null && result1 != null)
                    {
                        result.Red_br_akcija = pozicijaPrije;
                        result1.Red_br_akcija = pozicija;
                        db.SaveChanges();
                        TempData["note"] = "Aktivnost je pomaknuta za jedno mjesto gore";
                    }                    
                }
                catch
                {
                    TempData["note"] = "Aktivnost nije pomaknuta";
                }
            }
            return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = pa, pB = pb });
        }
        public ActionResult AktivnostPomakDolje(int id, int pa, int pb)
        {
            OS_Plan_2_akcija akcija = new OS_Plan_2_akcija();
            akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            int idAktivnost = akcija.Id_aktivnost;
            OS_Plan_2_aktivnost aktivnost = new OS_Plan_2_aktivnost();
            aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
            int idPodrucje = aktivnost.Id_podrucje;
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);

            int pozicija = akcija.Red_br_akcija;
            List<OS_Plan_2_akcija> trenutne = new List<OS_Plan_2_akcija>();
            trenutne = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == idAktivnost && w.Red_br_akcija >= pozicija).ToList();
            if (trenutne.Count == 1)
            {
                return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = pa, pB = pb });
            }
            trenutne = trenutne.OrderBy(o => o.Red_br_akcija).ToList();
            int idPoslije = trenutne.ElementAt(1).Id_plan;
            int pozicijaPoslije = trenutne.ElementAt(1).Red_br_akcija;
            using(var db = new BazaPodataka())
            {
                var result = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
                var result1 = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == idPoslije);
                try
                {
                    if(result!=null && result1!= null)
                    {
                        result.Red_br_akcija = pozicijaPoslije;
                        result1.Red_br_akcija = pozicija;
                        db.SaveChanges();
                        TempData["note"] = "Aktivnost je pomaknuta za jedno mjesto dolje";
                    }
                }
                catch
                {
                    TempData["note"] = "Aktivnost nije pomaknuta";
                }
            }
            return RedirectToAction("Details", new { id = podrucje.Id_glavni_plan, pA = pa, pB = pb });
        }
	}
}