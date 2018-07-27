﻿using System;
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
            podrucja = baza.OsPlan1Podrucje.Where(izraz => izraz.Id_glavni_plan == id).ToList();
            
            PlanOs1View plan = new PlanOs1View();
            OS_Plan_1 p = new OS_Plan_1();
            p = planovi_os1.ReadOS_Plan_1(id);

            List<Podrucje_rada> pod_rada = new List<Podrucje_rada>();
            pod_rada = podrucje_rada_db.ReadPodrucjeRada();
            plan.PodrucjeRada = pod_rada;

            List<Ciljevi> ciljevi = new List<Ciljevi>();
            ciljevi = ciljevi_db.ReadCiljevi();
            plan.Ciljevi = ciljevi;

            podrucja=podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            plan.OsPlan1 = p;
            plan.OsPlan1Podrucje = podrucja;

            plan.PodrucjeRadaSelected = pod_rada;
            /*dodatno*/
            
                List<Aktivnost> aktivnosti = new List<Aktivnost>();
                aktivnosti = aktivnost_db.ReadAktivnost();
                plan.Aktivnosti = aktivnosti;

                List<OS_Plan_1_aktivnost> osPlan1Aktivnosti = new List<OS_Plan_1_aktivnost>();
            if (podrucja.Count != 0)
            {
                int id_pod = podrucja.ElementAt(0).Id_plan;

                osPlan1Aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == id_pod).ToList();
            }
                plan.OsPlan1Aktivnost = osPlan1Aktivnosti;

                OS_Plan_1_aktivnost ak = new OS_Plan_1_aktivnost();
            if (podrucja.Count != 0)
            {
                ak.Id_podrucje = podrucja.ElementAt(0).Id_plan;
            }
                plan.Os_Plan_1_Aktivnost = ak;
            
            plan.Id = 1;
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
            
            //kraj testa
            //int id = plan.Podrucje.Red_br_podrucje; 
            plan.Podrucje.Id_glavni_plan = plan.Id;
            int _id = plan.Podrucje.Id_glavni_plan;

            int maxValue;
            List<OS_Plan_1_podrucje> trenutna_podrucja = new List<OS_Plan_1_podrucje>();
            trenutna_podrucja = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == _id).ToList();
            if (trenutna_podrucja.Count == 0)
            {
                maxValue = 1;
            }
            else
            {
                maxValue = trenutna_podrucja.Max(m => m.Red_br_podrucje);
                maxValue++;
            }
            

            plan.Podrucje.Red_br_podrucje = maxValue;
            try
            {
                baza.OsPlan1Podrucje.Add(plan.Podrucje);
                baza.SaveChanges();
            }
            catch
            {
                TempData["novop"] = "Novo područje nije dodano. Pripazite da popunite sva polja.";
                return RedirectToAction("Details", new { id = _id });
            }
            TempData["novop"] = "Novo područje je dodano";
            return RedirectToAction("Details",new { id=_id});
        }

        public ActionResult PodrucjePomakGore(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_podrucje plan = new OS_Plan_1_podrucje();
            plan = baza.OsPlan1Podrucje.Single(s => s.Id_plan == id);
            int id_glavni_plan = plan.Id_glavni_plan;
            int pozicija = plan.Red_br_podrucje;

            List<OS_Plan_1_podrucje> podrucja = new List<OS_Plan_1_podrucje>();
            podrucja = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id_glavni_plan && w.Red_br_podrucje <= pozicija).ToList();
            podrucja=podrucja.OrderBy(o => o.Red_br_podrucje).ToList();

            if (podrucja.Count == 1)
            {
                return RedirectToAction("Details", new { id = id_glavni_plan });
            }

            int pozicija_prethodni, id_prethodni;
            OS_Plan_1_podrucje p = podrucja.ElementAt(podrucja.Count - 2);
            pozicija_prethodni = p.Red_br_podrucje;
            id_prethodni = p.Id_plan;            

            plan.Red_br_podrucje = pozicija_prethodni;
            p.Red_br_podrucje = pozicija;
            using(var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan1Podrucje.SingleOrDefault(b => b.Id_plan == id_prethodni);
                if (rezultat != null)
                {
                    try
                    {
                        db.OsPlan1Podrucje.Attach(plan);
                        db.Entry(plan).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }                
            }
            using (var db=new BazaPodataka())
            {
                var rezultat2 = db.OsPlan1Podrucje.SingleOrDefault(d => d.Id_plan == id);
                if (rezultat2 != null)
                {
                    try
                    {
                        db.OsPlan1Podrucje.Attach(p);
                        db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }
            TempData["pomak"] = "Područje je pomaknuto prema gore";
            return RedirectToAction("Details", new { id = id_glavni_plan });
        }

        public ActionResult PodrucjePomakDolje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }

            OS_Plan_1_podrucje plan = new OS_Plan_1_podrucje();
            plan = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
            int id_glavni_plan = plan.Id_glavni_plan;
            int pozicija = plan.Red_br_podrucje;

            List<OS_Plan_1_podrucje> podrucja = new List<OS_Plan_1_podrucje>();
            podrucja = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id_glavni_plan && w.Red_br_podrucje >= pozicija).ToList();

            if (podrucja.Count == 1)
            {
                return RedirectToAction("Details", new { id = id_glavni_plan });
            }

            podrucja = podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            int pozicija_nakon = podrucja.ElementAt(1).Red_br_podrucje;
            int id_nakon = podrucja.ElementAt(1).Id_plan;

            using(var db=new BazaPodataka())
            {
                var rezultat = db.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
                var rezultat2 = db.OsPlan1Podrucje.SingleOrDefault(d => d.Id_plan == id_nakon);
                if (rezultat != null && rezultat2!=null)
                {
                    rezultat.Red_br_podrucje = pozicija_nakon;
                    rezultat2.Red_br_podrucje = pozicija;
                    db.SaveChanges();
                }
            }
            TempData["pomak"] = "Područje je pomaknuto prema dolje";
            return RedirectToAction("Details", new { id = id_glavni_plan });
        }

        public ActionResult UrediPodrucje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }

            PlanOs1View plan = new PlanOs1View();
            OS_Plan_1_podrucje podrucje = new OS_Plan_1_podrucje();
            List<Ciljevi> ciljevi = new List<Ciljevi>();
            List<Podrucje_rada> podrucja_rada = new List<Podrucje_rada>();

            podrucje = baza.OsPlan1Podrucje.Single(s => s.Id_plan == id);
            ciljevi = ciljevi_db.ReadCiljevi();
            podrucja_rada = podrucje_rada_db.ReadPodrucjeRada();

            
            plan.Ciljevi = ciljevi;
            plan.Podrucje = podrucje;
            plan.PodrucjeRada = podrucja_rada;

            

            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("UrediPodrucje", plan);
            }
            return View("UrediPodrucje", plan);
        }

        [HttpPost]
        public ActionResult UrediPodrucje(PlanOs1View plan)
        {
            int _id=0;
            TempData["note"] = "Glavni plan nije pronađen.";
            using (var db = new BazaPodataka())
            {
                
                var rezultat = db.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == plan.Podrucje.Id_plan);
                if (rezultat != null)
                {                    
                    try
                    {
                        _id = rezultat.Id_glavni_plan;
                        rezultat.Cilj = plan.Podrucje.Cilj;
                        rezultat.Opis_Podrucje = plan.Podrucje.Opis_Podrucje;
                        rezultat.Potrebno_sati = plan.Podrucje.Potrebno_sati;
                        db.SaveChanges();
                        TempData["note"] = "Promjena je spremljena";
                    }
                    catch
                    {
                        TempData["note"] = "Promjena nije spremljena";
                    }
                }
            }
            return RedirectToAction("Details", new { id = _id });
        }

        public ActionResult ObrisiPodrucje (int id)
        {
            OS_Plan_1_podrucje podrucje = new OS_Plan_1_podrucje();
            PlanOs1View plan = new PlanOs1View();
            Ciljevi cilj = new Ciljevi();
            Podrucje_rada podrucjeRada = new Podrucje_rada();

            podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
            cilj = ciljevi_db.ReadCiljevi(podrucje.Cilj);
            podrucjeRada = podrucje_rada_db.ReadPodrucjeRada(podrucje.Opis_Podrucje);

            plan.Podrucje = podrucje;
            plan.PodrucjeRada = new List<Podrucje_rada>();
            plan.PodrucjeRada.Add(podrucjeRada);
            plan.Ciljevi = new List<Ciljevi>();
            plan.Ciljevi.Add(cilj);

            return View("ObrisiPodrucje", plan);
        }

        [HttpPost]
        public ActionResult ObrisiPodrucje (PlanOs1View plan)
        {
            int _id = 0;
            
            using (var db = new BazaPodataka())
            {
                var rjesenje = db.OsPlan1Podrucje.SingleOrDefault(w => w.Id_plan == plan.Podrucje.Id_plan);
                _id = rjesenje.Id_glavni_plan;
                int id_ = rjesenje.Id_plan;
                if (rjesenje != null)
                {
                    TempData["note"] = "Područje rada je obrisano";
                    try
                    {                        
                        db.OsPlan1Podrucje.Remove(rjesenje);
                        db.SaveChanges();
                    }
                    catch
                    {
                        TempData["note"] = "Područje rada nije obrisano. Pokušajte ponovno";
                    }
                }
            }
            return RedirectToAction("Details", new { id = _id });
        }
    }
}