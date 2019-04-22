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
    public class PlanOs1Controller : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        private OS_Plan_1_DBHandle planovi_os1 = new OS_Plan_1_DBHandle();
        private Ciljevi_DBHandle ciljevi_db = new Ciljevi_DBHandle();
        private Podrucje_rada_DBHandle podrucje_rada_db = new Podrucje_rada_DBHandle();
        private Aktivnost_DBHandle aktivnost_db = new Aktivnost_DBHandle();     
               
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }         

            List<OS_Plan_1> planovi = new List<OS_Plan_1>();
            planovi = baza.OsPlan1.Where(w => w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId).ToList();
            return View("Index",planovi);            
        }

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.select = VratiSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult NoviPlan(OS_Plan_1 model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }			
            if(!ModelState.IsValid)
            {
                ViewBag.select = VratiSelectList();
                return View(model);
            }
            model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            try
            {
                using(var db = new BazaPodataka())
                {
                    db.OsPlan1.Add(model);
                    db.SaveChanges();
                    TempData["note"] = "Novi plan za osnovnu školu 1 je spremljen!";
                }
            }
            catch
            {
                TempData["note"] = "Novi plan nije spremljen";
            }         
			
			return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1 plan = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.select = VratiSelectList();
            return View("Uredi", plan);
        }
        [HttpPost]
        public ActionResult Edit(OS_Plan_1 plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.select = VratiSelectList();
                return View("Uredi", plan);
            }
            plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            try
            {
                using(var db = new BazaPodataka())
                {
                    db.OsPlan1.Add(plan);
                    db.Entry(plan).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Plan je uspješno promijenjen!";
                }
            }
            catch
            {
                TempData["note"] = "Plan nije promijenjen!";
            }            
			return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1 plan = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View("Obrisi",plan);
        }

        [HttpPost]
        public ActionResult Delete(OS_Plan_1 plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = plan.Id_plan;
            try
            {
                TempData["note"] = "Plan nije obrisan, dogodila se greška!";
                using (var db = new BazaPodataka())
                {
                    var result = db.OsPlan1.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        db.OsPlan1.Remove(result);
                        db.SaveChanges();
                        TempData["note"] = "Plan je uspješno obrisan!";
                    }
                }
            }
            catch
            {

            }            
			return RedirectToAction("Index");
		}

		public FileStreamResult Ispis()
		{
            List<OS_Plan_1> planovi = baza.OsPlan1.Where(w => w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId).ToList();

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


            /*dodatno*/
            List<Podrucje_rada> pr = new List<Podrucje_rada>();
            foreach(var i in podrucja)
            {
                Podrucje_rada pod = new Podrucje_rada();
                pod = podrucje_rada_db.ReadPodrucjeRada(i.Opis_Podrucje);
                pr.Add(pod);
            }

            
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
                plan.Id = podrucja.ElementAt(0).Id_plan;
            }
                plan.Os_Plan_1_Aktivnost = ak;
            
            
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
            
            return View("NovoPodrucje",plan);
        }

        [HttpPost]
        public ActionResult NovoPodrucje(PlanOs1View plan)
        {
            //test            
            if (plan.Podrucje.Potrebno_sati == null || plan.Podrucje.Opis_Podrucje==0 || plan.Podrucje.Cilj==null)
            {
                plan.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();
                plan.Ciljevi = ciljevi_db.ReadCiljevi();
                return View("NovoPodrucje", plan);
            }
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
                TempData["note"] = "Novo područje nije dodano. Pripazite da popunite sva polja.";
                return RedirectToAction("Details", new { id = _id });
            }
            TempData["note"] = "Novo područje je dodano";
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
            TempData["note"] = "Područje je pomaknuto prema gore";
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
            TempData["note"] = "Područje je pomaknuto prema dolje";
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
           
            return View("UrediPodrucje", plan);
        }

        [HttpPost]
        public ActionResult UrediPodrucje(PlanOs1View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (plan.Podrucje.Potrebno_sati == null || plan.Podrucje.Cilj==null || plan.Podrucje.Opis_Podrucje==0)
            {
                plan.Ciljevi = ciljevi_db.ReadCiljevi();
                plan.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();
                return View(plan);
            }
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

        public ActionResult ObrisiPodrucje (int id, int pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_podrucje podrucje = new OS_Plan_1_podrucje();
            PlanOs1View plan = new PlanOs1View();            
            Podrucje_rada podrucjeRada = new Podrucje_rada();

            podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
            
            podrucjeRada = podrucje_rada_db.ReadPodrucjeRada(podrucje.Opis_Podrucje);

            plan.Pozicija = pozicija;
            plan.Podrucje = podrucje;
            plan.PodrucjeRada = new List<Podrucje_rada>();
            plan.PodrucjeRada.Add(podrucjeRada);
            return View("ObrisiPodrucje", plan);
        }

        [HttpPost]
        public ActionResult ObrisiPodrucje (PlanOs1View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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

        public ActionResult NovaAktivnost(int id, int pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs1View plan = new PlanOs1View();
            plan.Id = id;
            plan.Pozicija = pozicija;

            List<Aktivnost> aktivnosti = new List<Aktivnost>();
            aktivnosti = aktivnost_db.ReadAktivnost();
            plan.Aktivnosti = aktivnosti;            
            return View("NovaAktivnost", plan);
        }
        [HttpPost]
        public ActionResult NovaAktivnost (PlanOs1View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (plan.Os_Plan_1_Aktivnost.Potrebno_sati == null || plan.Os_Plan_1_Aktivnost.Opis_aktivnost == 0 || plan.Os_Plan_1_Aktivnost.Mj_1 < 0 ||
                plan.Os_Plan_1_Aktivnost.Mj_10 < 0 || plan.Os_Plan_1_Aktivnost.Mj_11 < 0 || plan.Os_Plan_1_Aktivnost.Mj_12 < 0 || plan.Os_Plan_1_Aktivnost.Mj_2 < 0 ||
                plan.Os_Plan_1_Aktivnost.Mj_3 < 0 || plan.Os_Plan_1_Aktivnost.Mj_4 < 0 || plan.Os_Plan_1_Aktivnost.Mj_5 < 0 || plan.Os_Plan_1_Aktivnost.Mj_6 < 0 ||
                plan.Os_Plan_1_Aktivnost.Mj_7 < 0 || plan.Os_Plan_1_Aktivnost.Mj_8 < 0 || plan.Os_Plan_1_Aktivnost.Mj_9 < 0)
            {
                plan.Aktivnosti = aktivnost_db.ReadAktivnost();
                return View(plan);
            }
            int i = plan.Id;
            OS_Plan_1_aktivnost ak = new OS_Plan_1_aktivnost();
            ak = plan.Os_Plan_1_Aktivnost;
            ak.Id_podrucje = i;
            //zbrajanje
            ak.Br_sati = ak.Mj_1 + ak.Mj_10 + ak.Mj_11 + ak.Mj_12 + ak.Mj_2 + ak.Mj_3 + ak.Mj_4 + ak.Mj_5 + ak.Mj_6 + ak.Mj_7 + ak.Mj_8 + ak.Mj_9;
            //zbrajanje-kraj
            OS_Plan_1_podrucje p = new OS_Plan_1_podrucje();
            p = baza.OsPlan1Podrucje.Single(s => s.Id_plan == i);
            int _id = p.Id_glavni_plan;
            //zbrajanje podrucja
            p.Br_sati += ak.Br_sati; p.Mj_1 += ak.Mj_1;p.Mj_2 += ak.Mj_2;p.Mj_3 += ak.Mj_3;p.Mj_4 += ak.Mj_4;p.Mj_5 += ak.Mj_5;p.Mj_6 += ak.Mj_6;
            p.Mj_7 += ak.Mj_7;p.Mj_8 += ak.Mj_8;p.Mj_9 += ak.Mj_9;p.Mj_10 += ak.Mj_10;p.Mj_11 += ak.Mj_11;p.Mj_12 += ak.Mj_12;
            //zbrajanje podrucja-kraj
            int maxValue;
            List<OS_Plan_1_aktivnost> trenutne = new List<OS_Plan_1_aktivnost>();
            trenutne = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == i).ToList();
            if (trenutne.Count == 0)
            {
                maxValue = 1;
            }
            else
            {
                maxValue = trenutne.Max(m => m.Red_broj_aktivnost);
                maxValue++;
            }
            ak.Red_broj_aktivnost = maxValue;
            TempData["note"] = "Nova aktivnost je dodana";
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan1Aktivnost.Add(ak);
                    db.OsPlan1Podrucje.Add(p);
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    TempData["note"] = "Nova aktivnost nije dodana";
                }

            }
            TempData["prikaz"] = "1";
            return RedirectToAction("Details2", new { id = _id, pozicija=plan.Pozicija });
        }
        public ActionResult Details2 (int id, int pozicija)
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

            podrucja = podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            plan.OsPlan1 = p;
            plan.OsPlan1Podrucje = podrucja;


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

            List<OS_Plan_1_aktivnost> osPlan1Aktivnosti = new List<OS_Plan_1_aktivnost>();
            if (podrucja.Count != 0)
            {
                int id_pod = podrucja.ElementAt(0).Id_plan;

                osPlan1Aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == id_pod).ToList();
            }
            osPlan1Aktivnosti = osPlan1Aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();
            plan.OsPlan1Aktivnost = osPlan1Aktivnosti;

            OS_Plan_1_aktivnost ak = new OS_Plan_1_aktivnost();
            if (podrucja.Count != 0)
            {
                ak.Id_podrucje = podrucja.ElementAt(0).Id_plan;
                plan.Id = podrucja.ElementAt(0).Id_plan;
            }
            plan.Os_Plan_1_Aktivnost = ak;
            
            /*dodatno 2*/
            plan.Pozicija = pozicija;
            if (pozicija == -1)
            {
                return View("Details", plan);
            }
            OS_Plan_1_podrucje podr = new OS_Plan_1_podrucje();
            podr = podrucja.ElementAt(pozicija);
            osPlan1Aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == podr.Id_plan).ToList();
            osPlan1Aktivnosti = osPlan1Aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();
            plan.OsPlan1Aktivnost = osPlan1Aktivnosti;
            TempData["prikaz"] = "1";
            plan.Id = podr.Id_plan;
            return View("Details", plan);
        }

        public ActionResult AktivnostPomakGore(int id, int pozicija_podrucja)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_aktivnost aktivnost = new OS_Plan_1_aktivnost();
            aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            OS_Plan_1_podrucje podrucje = new OS_Plan_1_podrucje();
            podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == aktivnost.Id_podrucje);

            int pozicija = aktivnost.Red_broj_aktivnost;

            List<OS_Plan_1_aktivnost> aktivnosti = new List<OS_Plan_1_aktivnost>();
            aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == aktivnost.Id_podrucje && w.Red_broj_aktivnost <= pozicija).ToList();
            aktivnosti = aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();

            if (aktivnosti.Count == 1)
            {
                return RedirectToAction("Details2", new { id = podrucje.Id_glavni_plan, pozicija = pozicija_podrucja });
            }

            int pozicija_prethodni = aktivnosti.ElementAt(aktivnosti.Count - 2).Red_broj_aktivnost;
            int id_prethodni = aktivnosti.ElementAt(aktivnosti.Count - 2).Id_plan;

            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);                
                var rezultat1 = db.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id_prethodni);
                if(rezultat!=null && rezultat1 != null)
                {
                    try
                    {
                        rezultat.Red_broj_aktivnost = pozicija_prethodni;
                        rezultat1.Red_broj_aktivnost = pozicija;
                        db.SaveChanges();
                        TempData["note"] = "Aktivnost je pomaknuta za jedno mjesto prema gore";
                    }
                    catch
                    {
                        TempData["note"] = "Aktivnost iz nekog razloga nije pomaknuta prema gore. Pokušajte ponovno ili se obratite adminu";
                    }
                }
            }
            
            return RedirectToAction("Details2", new { id = podrucje.Id_glavni_plan, pozicija = pozicija_podrucja });
        }

        public ActionResult AktivnostPomakDolje (int id, int pozicija_podrucja)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }

            OS_Plan_1_aktivnost aktivnost = new OS_Plan_1_aktivnost();
            aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            OS_Plan_1_podrucje podrucje = new OS_Plan_1_podrucje();
            podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == aktivnost.Id_podrucje);

            
            List<OS_Plan_1_aktivnost> aktivnosti = new List<OS_Plan_1_aktivnost>();
            aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == aktivnost.Id_podrucje && w.Red_broj_aktivnost >= aktivnost.Red_broj_aktivnost).ToList();
            aktivnosti = aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();
            if (aktivnosti.Count == 1)
            {
                return RedirectToAction("Details2", new { id = podrucje.Id_glavni_plan, pozicija = pozicija_podrucja });
            }
            int pozicija = aktivnost.Red_broj_aktivnost;
            int pozicija_slijedeći = aktivnosti.ElementAt(1).Red_broj_aktivnost;
            int id_slijedeći = aktivnosti.ElementAt(1).Id_plan;
            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
                var rezultat1 = db.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id_slijedeći);

                if(rezultat!=null && rezultat1 != null)
                {
                    try
                    {
                        rezultat.Red_broj_aktivnost = pozicija_slijedeći;
                        rezultat1.Red_broj_aktivnost = pozicija;
                        db.SaveChanges();
                        TempData["note"] = "Aktivnost je pomaknuta za jedno mjesto prema dolje";
                    }
                    catch
                    {
                        TempData["note"] = "Aktivnost iz nekog razloga nije pomaknuta prema dolje. Pokušajte ponovno ili se obratite adminu";
                    }
                }
            }
            return RedirectToAction("Details2", new { id = podrucje.Id_glavni_plan, pozicija = pozicija_podrucja });
        }
        public ActionResult UrediAktivnost (int id, string pozicija, int pozicija_podrucja)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs1View plan = new PlanOs1View();            
            OS_Plan_1_aktivnost aktivnost = new OS_Plan_1_aktivnost();
            aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);

            plan.Pozicija = pozicija_podrucja;
            plan.Aktivnosti = new List<Aktivnost>();
            plan.Aktivnosti = aktivnost_db.ReadAktivnost();
            plan.Os_Plan_1_Aktivnost = aktivnost;
            plan.Broj = pozicija_podrucja;
            ViewBag.pozicija = pozicija;
            return View("UrediAktivnost", plan);
        }
        [HttpPost]
        public ActionResult UrediAktivnost (PlanOs1View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (plan.Os_Plan_1_Aktivnost.Potrebno_sati == null || plan.Os_Plan_1_Aktivnost.Opis_aktivnost==0 || plan.Os_Plan_1_Aktivnost.Mj_1<0||
                plan.Os_Plan_1_Aktivnost.Mj_10<0 || plan.Os_Plan_1_Aktivnost.Mj_11<0 || plan.Os_Plan_1_Aktivnost.Mj_12<0 || plan.Os_Plan_1_Aktivnost.Mj_2<0 ||
                plan.Os_Plan_1_Aktivnost.Mj_3<0 || plan.Os_Plan_1_Aktivnost.Mj_4<0 || plan.Os_Plan_1_Aktivnost.Mj_5<0 || plan.Os_Plan_1_Aktivnost.Mj_6<0 ||
                plan.Os_Plan_1_Aktivnost.Mj_7<0 || plan.Os_Plan_1_Aktivnost.Mj_8<0 || plan.Os_Plan_1_Aktivnost.Mj_9<0)
            {
                plan.Aktivnosti = aktivnost_db.ReadAktivnost();
                return View(plan);
            }
            int _id, poz = plan.Pozicija;
            OS_Plan_1_aktivnost aktivnost = new OS_Plan_1_aktivnost();
            aktivnost = plan.Os_Plan_1_Aktivnost;
            int id_ak = aktivnost.Id_plan;
            int id_pod = aktivnost.Id_podrucje;
            OS_Plan_1_podrucje p = new OS_Plan_1_podrucje();
            p = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id_pod);
            _id = p.Id_glavni_plan;
            //
            OS_Plan_1_aktivnost akPrije = new OS_Plan_1_aktivnost();
            akPrije = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id_ak);
            p.Br_sati -= akPrije.Br_sati;p.Mj_1 -= akPrije.Mj_1;p.Mj_2 -= akPrije.Mj_2;p.Mj_3 -= akPrije.Mj_3;p.Mj_4 -= akPrije.Mj_4;p.Mj_5 -= akPrije.Mj_5;
            p.Mj_6 -= akPrije.Mj_6;p.Mj_7 -= akPrije.Mj_7;p.Mj_8 -= akPrije.Mj_8;p.Mj_9 -= akPrije.Mj_9;p.Mj_10 -= akPrije.Mj_10;p.Mj_11 -= akPrije.Mj_11;p.Mj_12 -= akPrije.Mj_12;
            var ak = aktivnost;
            ak.Br_sati = ak.Mj_1 + ak.Mj_10 + ak.Mj_11 + ak.Mj_12 + ak.Mj_2 + ak.Mj_3 + ak.Mj_4 + ak.Mj_5 + ak.Mj_6 + ak.Mj_7 + ak.Mj_8 + ak.Mj_9;

            p.Br_sati += ak.Br_sati; p.Mj_1 += ak.Mj_1; p.Mj_2 += ak.Mj_2; p.Mj_3 += ak.Mj_3; p.Mj_4 += ak.Mj_4; p.Mj_5 += ak.Mj_5;
            p.Mj_6 += ak.Mj_6; p.Mj_7 += ak.Mj_7; p.Mj_8 += ak.Mj_8; p.Mj_9 += ak.Mj_9; p.Mj_10 += ak.Mj_10; p.Mj_11 += ak.Mj_11; p.Mj_12 += ak.Mj_12;
            //
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan1Aktivnost.Add(aktivnost);
                    db.Entry(aktivnost).State = System.Data.Entity.EntityState.Modified;
                    db.OsPlan1Podrucje.Add(p);
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Aktivnost je promijenjena";
                }
                catch
                {
                    TempData["note"] = "Aktivnost nije promijenjena.Pokušajte ponovno.";
                }
            }                
            return RedirectToAction("Details2", new { id = _id, pozicija = plan.Pozicija });            
        }
        public ActionResult ObrisiAktivnost(int id, int pozicija, string broj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs1View plan = new PlanOs1View();
            plan.Os_Plan_1_Aktivnost = new OS_Plan_1_aktivnost();
            plan.Os_Plan_1_Aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            int pod = plan.Os_Plan_1_Aktivnost.Id_podrucje;
            int ak = plan.Os_Plan_1_Aktivnost.Opis_aktivnost;
            plan.Aktivnosti = new List<Aktivnost>();
            Aktivnost a = new Aktivnost();
            a = aktivnost_db.ReadAktivnost(ak);
            plan.Aktivnosti.Add(a);
            OS_Plan_1_podrucje p = new OS_Plan_1_podrucje();
            p = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == pod);
            plan.Id = p.Id_glavni_plan;
            plan.Pozicija = pozicija;
            ViewBag.broj = broj;
            return View("ObrisiAktivnost", plan);
        }
        [HttpPost]
        public ActionResult ObrisiAktivnost (PlanOs1View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_akt = plan.Os_Plan_1_Aktivnost.Id_plan;
            int id_glavni, id_pod;
            OS_Plan_1_aktivnost a = new OS_Plan_1_aktivnost();
            a = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id_akt);
            id_pod = a.Id_podrucje;

            OS_Plan_1_podrucje p = new OS_Plan_1_podrucje();
            p = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id_pod);
            id_glavni = p.Id_glavni_plan;
            //
            p.Br_sati -= a.Br_sati; p.Mj_1 -= a.Mj_1; p.Mj_2 -= a.Mj_2; p.Mj_3 -= a.Mj_3; p.Mj_4 -= a.Mj_4; p.Mj_5 -= a.Mj_5;
            p.Mj_6 -= a.Mj_6; p.Mj_7 -= a.Mj_7; p.Mj_8 -= a.Mj_8; p.Mj_9 -= a.Mj_9; p.Mj_10 -= a.Mj_10; p.Mj_11 -= a.Mj_11; p.Mj_12 -=a.Mj_12;
            //
            using (var db = new BazaPodataka())
            {
                var aktivnost = db.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id_akt);
                if (aktivnost!=null)
                {                    
                    try
                    {
                        db.OsPlan1Aktivnost.Remove(aktivnost);
                        db.OsPlan1Podrucje.Add(p);
                        db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        TempData["note"] = "Aktivnost je obrisana";
                    }
                    catch
                    {
                        TempData["note"] = "Aktivnost nije obrisana";
                    }
                }
            }
            
            return RedirectToAction("Details2", "PlanOs1", new { id = id_glavni, pozicija = plan.Pozicija });
        }
        public FileStreamResult IspisDetalji(int id)
        {
            PlanOs1View plan = new PlanOs1View();
            plan.Aktivnosti = new List<Aktivnost>();
            plan.Aktivnosti = aktivnost_db.ReadAktivnost();
            plan.Ciljevi = new List<Ciljevi>();
            plan.Ciljevi = ciljevi_db.ReadCiljevi();
            plan.PodrucjeRada = new List<Podrucje_rada>();
            plan.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();
            plan.OsPlan1 = new OS_Plan_1();
            plan.OsPlan1 = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id);

            plan.OsPlan1Podrucje = new List<OS_Plan_1_podrucje>();
            plan.OsPlan1Podrucje = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id).ToList();

            plan.OsPlan1Aktivnost = new List<OS_Plan_1_aktivnost>();
            foreach(var item in plan.OsPlan1Podrucje)
            {
                List<OS_Plan_1_aktivnost> a = new List<OS_Plan_1_aktivnost>();
                a = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == item.Id_plan).ToList();
                foreach(var i in a)
                {
                    plan.OsPlan1Aktivnost.Add(i);
                }
            }
            Pedagog p = new Pedagog();
            int id_p = PlaniranjeSession.Trenutni.PedagogId;
            p = baza.Pedagog.SingleOrDefault(s => s.Id_Pedagog == id_p);
            PlanOs1DetailsReport report = new PlanOs1DetailsReport(plan, p);

            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
        private SelectList VratiSelectList()
        {
            List<Sk_godina> skGodina = baza.SkolskaGodina.ToList();
            var selectListItem = new List<SelectListItem>();
            foreach (var item in skGodina)
            {
                selectListItem.Add(new SelectListItem { Value = item.Sk_Godina.ToString(), Text = item.Sk_Godina + "./" + (item.Sk_Godina + 1).ToString() + "." });
            }
            var select = new SelectList(selectListItem, "Value", "Text");
            return select;
        }
    }
}