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
            //ulazni parametar je id glavnog plana
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }

            //List<OS_Plan_1_podrucje> podrucja = new List<OS_Plan_1_podrucje>();
            //podrucja = baza.OsPlan1Podrucje.Where(izraz => izraz.Id_glavni_plan == id).ToList();

            //PlanOs1View plan = new PlanOs1View();
            //OS_Plan_1 p = new OS_Plan_1();
            //p = planovi_os1.ReadOS_Plan_1(id);

            //List<Podrucje_rada> pod_rada = new List<Podrucje_rada>();
            //pod_rada = podrucje_rada_db.ReadPodrucjeRada();
            //plan.PodrucjeRada = pod_rada;

            //List<Ciljevi> ciljevi = new List<Ciljevi>();
            //ciljevi = ciljevi_db.ReadCiljevi();
            //plan.Ciljevi = ciljevi;

            //podrucja=podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            //plan.OsPlan1 = p;
            //plan.OsPlan1Podrucje = podrucja;


            ///*dodatno*/
            //List<Podrucje_rada> pr = new List<Podrucje_rada>();
            //foreach(var i in podrucja)
            //{
            //    Podrucje_rada pod = new Podrucje_rada();
            //    pod = podrucje_rada_db.ReadPodrucjeRada(i.Opis_Podrucje);
            //    pr.Add(pod);
            //}


            //    List<Aktivnost> aktivnosti = new List<Aktivnost>();
            //    aktivnosti = aktivnost_db.ReadAktivnost();
            //    plan.Aktivnosti = aktivnosti;

            //    List<OS_Plan_1_aktivnost> osPlan1Aktivnosti = new List<OS_Plan_1_aktivnost>();
            //if (podrucja.Count != 0)
            //{
            //    int id_pod = podrucja.ElementAt(0).Id_plan;

            //    osPlan1Aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == id_pod).ToList();
            //}
            //    plan.OsPlan1Aktivnost = osPlan1Aktivnosti;

            //    OS_Plan_1_aktivnost ak = new OS_Plan_1_aktivnost();
            //if (podrucja.Count != 0)
            //{
            //    ak.Id_podrucje = podrucja.ElementAt(0).Id_plan;
            //    plan.Id = podrucja.ElementAt(0).Id_plan;
            //}
            //    plan.Os_Plan_1_Aktivnost = ak;
            PlanOs1View model = new PlanOs1View();
            model.OsPlan1 = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model.OsPlan1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            model.OsPlan1Podrucje = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id).OrderBy(o=>o.Red_br_podrucje).ToList();
            model.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();
            return View("Details", model);
        }
        
        public ActionResult NovoPodrucje(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!PlanIsValid(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.id = id;
            ViewBag.selectPodrucje = VratiSelectPodrucje();
            ViewBag.selectCilj = VratiSelectCilj();
            return View();
        }

        [HttpPost]
        public ActionResult NovoPodrucje(OS_Plan_1_podrucje model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //test            
            if (!ModelState.IsValid)
            {
                ViewBag.id = model.Id_glavni_plan;
                ViewBag.selectPodrucje = VratiSelectPodrucje();
                ViewBag.selectCilj = VratiSelectCilj();
                return View(model);
            }
            //kraj testa
            //provjera ako postoji glavni plan i ako je glavni plan od istog pedagoga; 
            int idGlavniPlan = model.Id_glavni_plan;
            if(!PlanIsValid(idGlavniPlan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            int maxValue;
            List<OS_Plan_1_podrucje> trenutna_podrucja = new List<OS_Plan_1_podrucje>();
            trenutna_podrucja = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == idGlavniPlan).ToList();
            if (trenutna_podrucja.Count == 0)
            {
                maxValue = 1;
            }
            else
            {
                maxValue = trenutna_podrucja.Max(m => m.Red_br_podrucje);
                maxValue++;
            }
            

            model.Red_br_podrucje = maxValue;
            try
            {
                baza.OsPlan1Podrucje.Add(model);
                baza.SaveChanges();
            }
            catch
            {
                TempData["note"] = "Novo područje nije dodano. Pripazite da popunite sva polja.";
                return RedirectToAction("Details", new { id = idGlavniPlan });
            }
            TempData["note"] = "Novo područje je dodano";
            return RedirectToAction("Details",new { id = idGlavniPlan});
        }

        public ActionResult PodrucjePomakGore(int id)
        {
            //ulazni parametar je id os plan 1 područja
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_podrucje plan = new OS_Plan_1_podrucje();
            plan = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);

            if (plan == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            int idGlavniPlan = plan.Id_glavni_plan;
            if(!PlanIsValid(idGlavniPlan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            int pozicija = plan.Red_br_podrucje;

            List<OS_Plan_1_podrucje> podrucja = new List<OS_Plan_1_podrucje>();
            podrucja = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == idGlavniPlan && w.Red_br_podrucje <= pozicija).ToList();
            podrucja=podrucja.OrderBy(o => o.Red_br_podrucje).ToList();

            if (podrucja.Count == 1)
            {
                return RedirectToAction("Details", new { id = idGlavniPlan });
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
                var rezultat2 = db.OsPlan1Podrucje.SingleOrDefault(d => d.Id_plan == id);
                if (rezultat != null && rezultat2 != null)
                {
                    try
                    {                        
                        rezultat.Red_br_podrucje = p.Red_br_podrucje;
                        rezultat2.Red_br_podrucje = plan.Red_br_podrucje;
                        db.SaveChanges();
                        TempData["note"] = "Područje je pomaknuto prema gore";
                    }
                    catch
                    {
                        TempData["note"] = "Područje NIJE pomaknuto prema gore";
                    }
                }                
            }          
            return RedirectToAction("Details", new { id = idGlavniPlan });
        }

        public ActionResult PodrucjePomakDolje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }

            OS_Plan_1_podrucje plan = new OS_Plan_1_podrucje();
            plan = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);

            if (plan == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            int idGlavniPlan = plan.Id_glavni_plan;
            if (!PlanIsValid(idGlavniPlan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            int pozicija = plan.Red_br_podrucje;

            List<OS_Plan_1_podrucje> podrucja = new List<OS_Plan_1_podrucje>();
            podrucja = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == idGlavniPlan && w.Red_br_podrucje >= pozicija).ToList();

            if (podrucja.Count == 1)
            {
                return RedirectToAction("Details", new { id = idGlavniPlan });
            }

            podrucja = podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            int pozicija_nakon = podrucja.ElementAt(1).Red_br_podrucje;
            int id_nakon = podrucja.ElementAt(1).Id_plan;

            using(var db=new BazaPodataka())
            {
                try
                {
                    var rezultat = db.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
                    var rezultat2 = db.OsPlan1Podrucje.SingleOrDefault(d => d.Id_plan == id_nakon);
                    if (rezultat != null && rezultat2 != null)
                    {
                        rezultat.Red_br_podrucje = pozicija_nakon;
                        rezultat2.Red_br_podrucje = pozicija;
                        db.SaveChanges();
                        TempData["note"] = "Područje je pomaknuto prema dolje";
                    }
                }
                catch
                {
                    TempData["note"] = "Područje NIJE pomaknuto prema dolje";
                }
            }            
            return RedirectToAction("Details", new { id = idGlavniPlan });
        }

        public ActionResult UrediPodrucje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);

            if(podrucje==null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            ViewBag.selectCilj = VratiSelectCilj();
            ViewBag.selectPodrucje = VratiSelectPodrucje();
           
            return View(podrucje);
        }

        [HttpPost]
        public ActionResult UrediPodrucje(OS_Plan_1_podrucje plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.selectCilj = VratiSelectCilj();
                ViewBag.selectPodrucje = VratiSelectPodrucje();
                return View(plan);
            }
            int id = plan.Id_plan;
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);

            if (podrucje == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            plan.Id_glavni_plan = podrucje.Id_glavni_plan;
            plan.Red_br_podrucje = podrucje.Red_br_podrucje;
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan1Podrucje.Add(plan);
                    db.Entry(plan).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Područje djelovanja je promijenjeno";
                }
                catch
                {
                    TempData["note"] = "Područje djelovanja nije promijenjeno";
                }                
            }
            return RedirectToAction("Details", new { id = plan.Id_glavni_plan });
        }

        public ActionResult ObrisiPodrucje (int id, int pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s=>s.Id_plan==id);
            if (podrucje == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            ViewBag.pozicija = pozicija.ToString()+".";
            ViewBag.podrucje = podrucje_rada_db.ReadPodrucjeRada(podrucje.Opis_Podrucje).Naziv;
            return View(podrucje);
        }

        [HttpPost]
        public ActionResult ObrisiPodrucje (OS_Plan_1_podrucje model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.Id_plan;
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
            if (podrucje == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            int idGlavniPlan = podrucje.Id_glavni_plan;
            using (var db = new BazaPodataka())
            {
                var result = db.OsPlan1Podrucje.SingleOrDefault(w => w.Id_plan == id);                
                
                if (result != null)
                {
                    TempData["note"] = "Područje djelovanja je obrisano";
                    try
                    {                        
                        db.OsPlan1Podrucje.Remove(result);
                        db.SaveChanges();
                    }
                    catch
                    {
                        TempData["note"] = "Područje djelovanja nije obrisano. Pokušajte ponovno";
                    }
                }
            }
            return RedirectToAction("Details", new { id = idGlavniPlan });
        }
        public ActionResult Aktivnosti (int idPodrucje,int id)
        {
            //ulazni parametar idPodrucje je id podrucja djelovanja 
            //ulazni parametar id je id glavnog plana
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(id>0 && idPodrucje==0)
            {
                if (!PlanIsValid(id)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                PlanOs1View model = new PlanOs1View();
                model.OsPlan1 = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                model.OsPlan1Podrucje = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id).OrderBy(o => o.Red_br_podrucje).ToList();
                model.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();
                model.OsPlan1Aktivnost = new List<OS_Plan_1_aktivnost>();
                model.Aktivnosti = aktivnost_db.ReadAktivnost();                
                ViewBag.id = 0;
                if (model.OsPlan1Podrucje.Count > 0)
                {
                    int _id = model.OsPlan1Podrucje.ElementAt(0).Id_plan;
                    model.OsPlan1Aktivnost = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == _id).OrderBy(o=>o.Red_broj_aktivnost).ToList();
                    ViewBag.id = _id;
                }
                return View(model);
            }
            else if (id==0 && idPodrucje > 0)
            {
                PlanOs1View model = new PlanOs1View();
                model.Podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
                if (model.Podrucje == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                if (!PlanIsValid(model.Podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                model.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();
                model.OsPlan1Aktivnost = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == idPodrucje).OrderBy(o=>o.Red_broj_aktivnost).ToList();
                model.Aktivnosti = aktivnost_db.ReadAktivnost();
                int _id = model.Podrucje.Id_glavni_plan;
                model.OsPlan1Podrucje = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == _id).ToList();
                ViewBag.id = model.Podrucje.Id_plan;
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }            
        }
        
        public ActionResult NovaAktivnost(int id)
        {
            //ulazni parametar id je id područja djelovanja
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id <= 0)
            {
                return RedirectToAction("Info", "OpciPodaci", new { poruka = "Morate dodati područje djelovanja" });
            }
            ViewBag.selectAktivnost = VratiSelectAktivnost();
            ViewBag.id = id;
            return View(new OS_Plan_1_aktivnost());
        }
        [HttpPost]
        public ActionResult NovaAktivnost (OS_Plan_1_aktivnost plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //if (plan.Os_Plan_1_Aktivnost.Potrebno_sati == null || plan.Os_Plan_1_Aktivnost.Opis_aktivnost == 0 || plan.Os_Plan_1_Aktivnost.Mj_1 < 0 ||
            //    plan.Os_Plan_1_Aktivnost.Mj_10 < 0 || plan.Os_Plan_1_Aktivnost.Mj_11 < 0 || plan.Os_Plan_1_Aktivnost.Mj_12 < 0 || plan.Os_Plan_1_Aktivnost.Mj_2 < 0 ||
            //    plan.Os_Plan_1_Aktivnost.Mj_3 < 0 || plan.Os_Plan_1_Aktivnost.Mj_4 < 0 || plan.Os_Plan_1_Aktivnost.Mj_5 < 0 || plan.Os_Plan_1_Aktivnost.Mj_6 < 0 ||
            //    plan.Os_Plan_1_Aktivnost.Mj_7 < 0 || plan.Os_Plan_1_Aktivnost.Mj_8 < 0 || plan.Os_Plan_1_Aktivnost.Mj_9 < 0)
            //{
            //    plan.Aktivnosti = aktivnost_db.ReadAktivnost();
            //    return View(plan);
            //}
            if (!ModelState.IsValid)
            {
                ViewBag.selectAktivnost = VratiSelectAktivnost();
                ViewBag.id = plan.Id_podrucje;
                return View(plan);
            }
            int i = plan.Id_podrucje;
            //OS_Plan_1_aktivnost ak = new OS_Plan_1_aktivnost();
            OS_Plan_1_aktivnost ak = plan;
            //ak.Id_podrucje = i;
            ////zbrajanje
            ak.Br_sati = ak.Mj_1 + ak.Mj_10 + ak.Mj_11 + ak.Mj_12 + ak.Mj_2 + ak.Mj_3 + ak.Mj_4 + ak.Mj_5 + ak.Mj_6 + ak.Mj_7 + ak.Mj_8 + ak.Mj_9;
            ////zbrajanje-kraj
            //OS_Plan_1_podrucje p = new OS_Plan_1_podrucje();
            //p = baza.OsPlan1Podrucje.Single(s => s.Id_plan == i);
            //int _id = p.Id_glavni_plan;
            ////zbrajanje podrucja
            //p.Br_sati += ak.Br_sati; p.Mj_1 += ak.Mj_1;p.Mj_2 += ak.Mj_2;p.Mj_3 += ak.Mj_3;p.Mj_4 += ak.Mj_4;p.Mj_5 += ak.Mj_5;p.Mj_6 += ak.Mj_6;
            //p.Mj_7 += ak.Mj_7;p.Mj_8 += ak.Mj_8;p.Mj_9 += ak.Mj_9;p.Mj_10 += ak.Mj_10;p.Mj_11 += ak.Mj_11;p.Mj_12 += ak.Mj_12;
            ////zbrajanje podrucja-kraj
            plan.Br_sati = ak.Br_sati;
            if (!PodrucjeIsValid(plan.Id_podrucje))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }            

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
            plan.Red_broj_aktivnost = maxValue;            
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan1Aktivnost.Add(plan);               
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            UpdatePodrucje(plan.Id_podrucje);
            return RedirectToAction("Aktivnosti", new { idPodrucje = plan.Id_podrucje, id=0 });
        }        

        public ActionResult AktivnostPomakGore(int id)
        {
            //ulazni parametar id je id aktivnosti
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == aktivnost.Id_podrucje);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            int pozicija = aktivnost.Red_broj_aktivnost;

            List<OS_Plan_1_aktivnost> aktivnosti = new List<OS_Plan_1_aktivnost>();
            aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == aktivnost.Id_podrucje && w.Red_broj_aktivnost <= pozicija).ToList();
            aktivnosti = aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();

            if (aktivnosti.Count == 1)
            {
                return RedirectToAction("Aktivnosti", new { idPodrucje = podrucje.Id_plan, id=0 });
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
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
            }
            
            return RedirectToAction("Aktivnosti", new { idPodrucje = podrucje.Id_plan, id = 0 });
        }

        public ActionResult AktivnostPomakDolje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }

            OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == aktivnost.Id_podrucje);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);


            List<OS_Plan_1_aktivnost> aktivnosti = new List<OS_Plan_1_aktivnost>();
            aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == aktivnost.Id_podrucje && w.Red_broj_aktivnost >= aktivnost.Red_broj_aktivnost).ToList();
            aktivnosti = aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();
            if (aktivnosti.Count == 1)
            {
                return RedirectToAction("Aktivnosti", new { idPodrucje = podrucje.Id_plan, id = 0 });
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
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
            }
            return RedirectToAction("Aktivnosti", new { idPodrucje = podrucje.Id_plan, id = 0 });
        }
        public ActionResult UrediAktivnost (int id, string pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }            
            OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if(aktivnost==null || !PodrucjeIsValid(aktivnost.Id_podrucje))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            else
            {
                ViewBag.selectAktivnost = VratiSelectAktivnost();
                ViewBag.pozicija = pozicija;
                return View(aktivnost);
            }            
        }
        [HttpPost]
        public ActionResult UrediAktivnost (OS_Plan_1_aktivnost model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.selectAktivnost = VratiSelectAktivnost();
                ViewBag.pozicija = Request.Form.Get("pozicija");
                return View(model);
            }
            if (!PodrucjeIsValid(model.Id_podrucje))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int _id;
            OS_Plan_1_aktivnost aktivnost = model;            
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
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return RedirectToAction("Aktivnosti", new { idPodrucje = p.Id_plan, id = 0 });
        }
        public ActionResult ObrisiAktivnost(int id, string pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if(aktivnost==null || !PodrucjeIsValid(aktivnost.Id_podrucje)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            ViewBag.pozicija = pozicija;
            ViewBag.aktivnost = aktivnost_db.ReadAktivnost(aktivnost.Opis_aktivnost).Naziv;
            return View(aktivnost);
        }
        [HttpPost]
        public ActionResult ObrisiAktivnost (OS_Plan_1_aktivnost model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_akt = model.Id_plan;
            int id_pod;
            OS_Plan_1_aktivnost a = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id_akt);

            if (a == null || !PodrucjeIsValid(a.Id_podrucje)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            id_pod = a.Id_podrucje;

            OS_Plan_1_podrucje p = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id_pod);            
            
            //
            //p.Br_sati -= a.Br_sati; p.Mj_1 -= a.Mj_1; p.Mj_2 -= a.Mj_2; p.Mj_3 -= a.Mj_3; p.Mj_4 -= a.Mj_4; p.Mj_5 -= a.Mj_5;
            //p.Mj_6 -= a.Mj_6; p.Mj_7 -= a.Mj_7; p.Mj_8 -= a.Mj_8; p.Mj_9 -= a.Mj_9; p.Mj_10 -= a.Mj_10; p.Mj_11 -= a.Mj_11; p.Mj_12 -=a.Mj_12;
            //
            using (var db = new BazaPodataka())
            {
                var aktivnost = db.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id_akt);
                if (aktivnost!=null)
                {                    
                    try
                    {
                        db.OsPlan1Aktivnost.Remove(aktivnost);
                        //db.OsPlan1Podrucje.Add(p);
                        //db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();                        
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
            }
            UpdatePodrucje(p.Id_plan);
            return RedirectToAction("Aktivnosti", new { idPodrucje = p.Id_plan, id = 0 });
        }
        public ActionResult Akcije (int idAktivnost, int idPodrucje)
        {
            //ulazni parametar je id os_plan_1_aktivnosti
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (idAktivnost == 0 && idPodrucje > 0)
            {
                if (!PodrucjeIsValid(idPodrucje))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<OS_Plan_1_aktivnost> aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == idPodrucje).OrderBy(o => o.Red_broj_aktivnost).ToList();
                ViewBag.aktivnosti = aktivnosti;
                List<OS_Plan_1_akcija> akcije = new List<OS_Plan_1_akcija>();
                if (aktivnosti.Count > 0)
                {
                    int idAkt = aktivnosti.First().Id_plan;
                    ViewBag.id = idAkt;
                    akcije = baza.OsPlan1Akcija.Where(w => w.Id_aktivnost == idAkt).OrderBy(o=>o.Red_br_akcija).ToList();
                }
                else ViewBag.id = 0;

                ViewBag.akt = aktivnost_db.ReadAktivnost();
                
                return View(akcije);
            }
            else
            {                
                if (!AktivnostIsValid(idAktivnost))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<OS_Plan_1_akcija> akcije = baza.OsPlan1Akcija.Where(w => w.Id_aktivnost == idAktivnost).OrderBy(o => o.Red_br_akcija).ToList();
                idPodrucje = (from pod in baza.OsPlan1Podrucje
                                  join ak in baza.OsPlan1Aktivnost on pod.Id_plan equals ak.Id_podrucje
                                  where ak.Id_plan == idAktivnost
                                  select pod.Id_plan).First();
                List<OS_Plan_1_aktivnost> aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == idPodrucje).OrderBy(o => o.Red_broj_aktivnost).ToList();
                ViewBag.aktivnosti = aktivnosti;
                ViewBag.akt = aktivnost_db.ReadAktivnost();
                ViewBag.id = idAktivnost;
                return View(akcije);
            }
        }
        public ActionResult NovaAkcija (int id)
        {
            //id je id aktivnosti
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id <= 0)
            {
                return RedirectToAction("Info", "OpciPodaci", new { poruka = "Morate dodati programske aktivnosti" });
            }
            ViewBag.id = id;
            return View(new OS_Plan_1_akcija());
        }
        [HttpPost]
        public async Task<ActionResult> NovaAkcija(OS_Plan_1_akcija akcija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.id = akcija.Id_aktivnost;
                return View(akcija);
            }
            if (!AktivnostIsValid(akcija.Id_aktivnost))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            akcija.Br_sati = akcija.Mj_9 + akcija.Mj_10 + akcija.Mj_11 + akcija.Mj_12 + akcija.Mj_1 + akcija.Mj_2 + akcija.Mj_3 + akcija.Mj_4 + akcija.Mj_5
                + akcija.Mj_6 + akcija.Mj_7 + akcija.Mj_8;
            int i = akcija.Id_aktivnost;
            int maxValue;
            List<OS_Plan_1_akcija> trenutne = new List<OS_Plan_1_akcija>();
            trenutne = baza.OsPlan1Akcija.Where(w => w.Id_aktivnost == i).ToList();
            if (trenutne.Count == 0)
            {
                maxValue = 1;
            }
            else
            {
                maxValue = trenutne.Max(m => m.Red_br_akcija);
                maxValue++;
            }
            akcija.Red_br_akcija = maxValue;

            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan1Akcija.Add(akcija);
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            await UpdateAktivnost(akcija.Id_aktivnost);
            return RedirectToAction("Akcije", new { idAktivnost=akcija.Id_aktivnost, idPodrucje=0});
        }
        public ActionResult UrediAkcija(int id, string pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!AkcijaIsValid(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            OS_Plan_1_akcija akcija = baza.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);
            ViewBag.pozicija = pozicija;
            return View(akcija);
        }
        [HttpPost]
        public async Task<ActionResult> UrediAkcija(OS_Plan_1_akcija akcija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                return View(akcija);
            }
            if (!AkcijaIsValid(akcija.Id) || !AktivnostIsValid(akcija.Id_aktivnost))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            akcija.Br_sati = akcija.Mj_9 + akcija.Mj_10 + akcija.Mj_11 + akcija.Mj_12 + akcija.Mj_1 + akcija.Mj_2 + akcija.Mj_3 + akcija.Mj_4 + akcija.Mj_5
                + akcija.Mj_6 + akcija.Mj_7 + akcija.Mj_8;
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan1Akcija.Add(akcija);
                    db.Entry(akcija).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            await UpdateAktivnost(akcija.Id_aktivnost);
            return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
        }
        public ActionResult ObrisiAkcija(int id, string pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!AkcijaIsValid(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            OS_Plan_1_akcija akcija = baza.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);
            ViewBag.pozicija = pozicija;
            return View(akcija);
        }
        public async Task<ActionResult> ObrisiAkcijaAsync (OS_Plan_1_akcija akcija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!AkcijaIsValid(akcija.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int id = akcija.Id;
            using (var db = new BazaPodataka())
            {
                var result = db.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);
                if (result != null)
                {
                    id = result.Id_aktivnost;
                    db.OsPlan1Akcija.Remove(result);
                    db.SaveChanges();
                }
                else
                {
                    id = 0;
                }
            }
            if (id != 0)
            {
                await UpdateAktivnost(id);
            }            
            return RedirectToAction("Akcije", new { idAktivnost = id, idPodrucje = 0 });
        }
        public ActionResult AkcijaPomakGore(int id)
        {
            //ulazni parametar id je id akcije
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!AkcijaIsValid(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            OS_Plan_1_akcija akcija = baza.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);            
            //OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == akcija.Id_aktivnost);           

            int pozicija = akcija.Red_br_akcija;

            List<OS_Plan_1_akcija> akcije = new List<OS_Plan_1_akcija>();
            akcije = baza.OsPlan1Akcija.Where(w => w.Id_aktivnost == akcija.Id_aktivnost && w.Red_br_akcija <= pozicija).ToList();
            akcije = akcije.OrderBy(o => o.Red_br_akcija).ToList();

            if (akcije.Count <= 1)
            {
                return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
            }

            int pozicija_prethodni = akcije.ElementAt(akcije.Count - 2).Red_br_akcija;
            int id_prethodni = akcije.ElementAt(akcije.Count - 2).Id;

            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);
                var rezultat1 = db.OsPlan1Akcija.SingleOrDefault(s => s.Id == id_prethodni);
                if (rezultat != null && rezultat1 != null)
                {
                    try
                    {
                        rezultat.Red_br_akcija = pozicija_prethodni;
                        rezultat1.Red_br_akcija = pozicija;
                        db.SaveChanges();
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
            }

            return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
        }
        public ActionResult AkcijaPomakDolje(int id)
        {
            //ulazni parametar id je id akcije
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!AkcijaIsValid(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            OS_Plan_1_akcija akcija = baza.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);
            //OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == akcija.Id_aktivnost);           

            int pozicija = akcija.Red_br_akcija;

            List<OS_Plan_1_akcija> akcije = new List<OS_Plan_1_akcija>();
            akcije = baza.OsPlan1Akcija.Where(w => w.Id_aktivnost == akcija.Id_aktivnost && w.Red_br_akcija >= pozicija).ToList();
            akcije = akcije.OrderBy(o => o.Red_br_akcija).ToList();

            if (akcije.Count <= 1)
            {
                return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
            }

            int pozicija_prethodni = akcije.ElementAt(1).Red_br_akcija;
            int id_prethodni = akcije.ElementAt(1).Id;

            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);
                var rezultat1 = db.OsPlan1Akcija.SingleOrDefault(s => s.Id == id_prethodni);
                if (rezultat != null && rezultat1 != null)
                {
                    try
                    {
                        rezultat.Red_br_akcija = pozicija_prethodni;
                        rezultat1.Red_br_akcija = pozicija;
                        db.SaveChanges();
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
            }

            return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
        }
        public ActionResult Vrijeme (int id)
        {
            //id je id glavnog plana
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs1View model = new PlanOs1View();
            if (!PlanIsValid(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            model.OsPlan1Podrucje = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id).OrderBy(o=>o.Red_br_podrucje).ToList();
            model.OsPlan1Aktivnost = (from pl in baza.OsPlan1
                                      join pod in baza.OsPlan1Podrucje on pl.Id_plan equals pod.Id_glavni_plan
                                      join akt in baza.OsPlan1Aktivnost on pod.Id_plan equals akt.Id_podrucje
                                      where pl.Id_plan == id && pl.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                      select akt).ToList();
            model.OsPlan1Akcija = (from pl in baza.OsPlan1
                                      join pod in baza.OsPlan1Podrucje on pl.Id_plan equals pod.Id_glavni_plan
                                      join akt in baza.OsPlan1Aktivnost on pod.Id_plan equals akt.Id_podrucje
                                      join akc in baza.OsPlan1Akcija on akt.Id_plan equals akc.Id_aktivnost
                                      where pl.Id_plan == id && pl.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                      select akc).ToList();
            model.Aktivnosti = aktivnost_db.ReadAktivnost();
            model.Ciljevi = ciljevi_db.ReadCiljevi();
            model.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();
            return View(model);
        }
        public ActionResult Kopiraj (int id)
        {
            // id je ig glavnog plana
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_1 plan = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }            
            ViewBag.select = VratiSelectList();
            ViewBag.naziv = plan.Naziv;
            ViewBag.godina = plan.Ak_godina;
            plan.Naziv = string.Empty;
            plan.Opis = string.Empty;
            return View(plan);
        }
        [HttpPost]
        public ActionResult Kopiraj (OS_Plan_1 plan)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!PlanIsValid(plan.Id_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (!ModelState.IsValid)
            {
                OS_Plan_1 pl = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == plan.Id_plan && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                ViewBag.naziv = pl.Naziv;
                ViewBag.godina = pl.Ak_godina;
                ViewBag.select = VratiSelectList();
                return View(plan);
            }
            int id = plan.Id_plan;
            using(var db = new BazaPodataka())
            {
                plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                db.OsPlan1.Add(plan);
                db.SaveChanges();

                int idPlan = db.OsPlan1.Where(l => l.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId).Max(m=>m.Id_plan);
                List<OS_Plan_1_podrucje> podrucja = db.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
                
                foreach(var podrucje in podrucja)
                {
                    int idPodrucje = podrucje.Id_plan;
                    podrucje.Id_glavni_plan = idPlan;
                    db.OsPlan1Podrucje.Add(podrucje);
                    db.SaveChanges();

                    var pod = db.OsPlan1Podrucje.Where(l => l.Id_glavni_plan == idPlan).Max(m=>m.Id_plan);
                    List<OS_Plan_1_aktivnost> aktivnosti = db.OsPlan1Aktivnost.Where(w => w.Id_podrucje == idPodrucje).ToList();
                    foreach(var aktivnost in aktivnosti)
                    {
                        int idAktivnost = aktivnost.Id_plan;
                        aktivnost.Id_podrucje = pod;
                        db.OsPlan1Aktivnost.Add(aktivnost);
                        db.SaveChanges();

                        var akt = db.OsPlan1Aktivnost.Where(l => l.Id_podrucje == pod).Max(m=>m.Id_plan);
                        List<OS_Plan_1_akcija> akcije = db.OsPlan1Akcija.Where(w => w.Id_aktivnost == idAktivnost).ToList();
                        foreach(var akcija in akcije)
                        {
                            akcija.Id_aktivnost = akt;
                            db.OsPlan1Akcija.Add(akcija);
                            db.SaveChanges();
                        }
                    }
                }
            }
            TempData["note"] = "Plan je kopiran";
            return RedirectToAction("Index");
        }
        public ActionResult IspisDetalji(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!PlanIsValid(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            PlanOs1View plan = new PlanOs1View();           
            plan.Aktivnosti = aktivnost_db.ReadAktivnost();            
            plan.Ciljevi = ciljevi_db.ReadCiljevi();            
            plan.PodrucjeRada = podrucje_rada_db.ReadPodrucjeRada();            
            plan.OsPlan1 = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id);            
            plan.OsPlan1Podrucje = baza.OsPlan1Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
            plan.OsPlan1Aktivnost = (from pl in baza.OsPlan1
                                     join pod in baza.OsPlan1Podrucje on pl.Id_plan equals pod.Id_glavni_plan
                                     join akt in baza.OsPlan1Aktivnost on pod.Id_plan equals akt.Id_podrucje
                                     where pl.Id_plan == id && pl.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                     select akt).ToList();
            plan.OsPlan1Akcija = (from pl in baza.OsPlan1
                                  join pod in baza.OsPlan1Podrucje on pl.Id_plan equals pod.Id_glavni_plan
                                  join akt in baza.OsPlan1Aktivnost on pod.Id_plan equals akt.Id_podrucje
                                  join akc in baza.OsPlan1Akcija on akt.Id_plan equals akc.Id_aktivnost
                                  where pl.Id_plan == id && pl.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                  select akc).ToList();

            Pedagog p = baza.Pedagog.SingleOrDefault(s => s.Id_Pedagog == PlaniranjeSession.Trenutni.PedagogId);
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
        private SelectList VratiSelectPodrucje()
        {
            List<Podrucje_rada> podrucja = podrucje_rada_db.ReadPodrucjeRada();                        
            var select = new SelectList(podrucja, "Id_podrucje", "Naziv");
            return select;
        }
        private SelectList VratiSelectCilj()
        {
            List<Ciljevi> ciljevi = ciljevi_db.ReadCiljevi();
            var select = new SelectList(ciljevi, "Naziv", "Naziv");
            return select;
        }
        private SelectList VratiSelectAktivnost()
        {
            List<Aktivnost> ciljevi = aktivnost_db.ReadAktivnost();
            var select = new SelectList(ciljevi, "Id_aktivnost", "Naziv");
            return select;
        }
        private bool PlanIsValid(int id)
        {
            OS_Plan_1 plan = baza.OsPlan1.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return false;
            }
            else return true;
        }
        private bool PodrucjeIsValid(int id)
        {
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
            if (podrucje == null)
            {
                return false;
            }
            return PlanIsValid(podrucje.Id_glavni_plan);
        }
        private bool AktivnostIsValid(int id)
        {
            OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null)
            {
                return false;
            }
            return PodrucjeIsValid(aktivnost.Id_podrucje);
        }
        private bool AkcijaIsValid(int id)
        {
            OS_Plan_1_akcija akcija = baza.OsPlan1Akcija.SingleOrDefault(s => s.Id == id);
            if (akcija == null)
            {
                return false;
            }
            return AktivnostIsValid(akcija.Id_aktivnost);
        }
        private async Task UpdateAktivnost(int id)
        {
            baza = new BazaPodataka();
            List<OS_Plan_1_akcija> akcije = baza.OsPlan1Akcija.Where(w => w.Id_aktivnost == id).ToList();
            OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            OS_Plan_1_aktivnost a = new OS_Plan_1_aktivnost();
            a.Id_plan = aktivnost.Id_plan;
            a.Id_podrucje = aktivnost.Id_podrucje;
            a.Opis_aktivnost = aktivnost.Opis_aktivnost;
            a.Potrebno_sati = aktivnost.Potrebno_sati;
            a.Red_broj_aktivnost = aktivnost.Red_broj_aktivnost;
            foreach(var item in akcije)
            {
                a.Mj_9 += item.Mj_9;
                a.Mj_10 += item.Mj_10;
                a.Mj_11 += item.Mj_11;
                a.Mj_12 += item.Mj_12;
                a.Mj_1 += item.Mj_1;
                a.Mj_2 += item.Mj_2;
                a.Mj_3 += item.Mj_3;
                a.Mj_4 += item.Mj_4;
                a.Mj_5 += item.Mj_5;
                a.Mj_6 += item.Mj_6;
                a.Mj_7 += item.Mj_7;
                a.Mj_8 += item.Mj_8;
                a.Br_sati += item.Br_sati;
            }
            using(var db=new BazaPodataka())
            {
                db.OsPlan1Aktivnost.Add(a);
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            UpdatePodrucje(aktivnost.Id_podrucje);
        }
        private void UpdatePodrucje(int id)
        {
            baza = new BazaPodataka();
            List<OS_Plan_1_aktivnost> aktivnosti = baza.OsPlan1Aktivnost.Where(w => w.Id_podrucje == id).ToList();
            OS_Plan_1_podrucje podrucje = baza.OsPlan1Podrucje.SingleOrDefault(s => s.Id_plan == id);
            OS_Plan_1_podrucje p = new OS_Plan_1_podrucje();
            p.Id_plan = podrucje.Id_plan;
            p.Id_glavni_plan = podrucje.Id_glavni_plan;
            p.Opis_Podrucje = podrucje.Opis_Podrucje;
            p.Potrebno_sati = podrucje.Potrebno_sati;
            p.Red_br_podrucje = podrucje.Red_br_podrucje;
            p.Cilj = podrucje.Cilj;
            foreach (var item in aktivnosti)
            {
                p.Mj_9 += item.Mj_9;
                p.Mj_10 += item.Mj_10;
                p.Mj_11 += item.Mj_11;
                p.Mj_12 += item.Mj_12;
                p.Mj_1 += item.Mj_1;
                p.Mj_2 += item.Mj_2;
                p.Mj_3 += item.Mj_3;
                p.Mj_4 += item.Mj_4;
                p.Mj_5 += item.Mj_5;
                p.Mj_6 += item.Mj_6;
                p.Mj_7 += item.Mj_7;
                p.Mj_8 += item.Mj_8;
                p.Br_sati += item.Br_sati;
            }
            using(var db = new BazaPodataka())
            {
                db.OsPlan1Podrucje.Add(p);
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}