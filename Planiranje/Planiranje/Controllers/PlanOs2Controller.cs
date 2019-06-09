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
            return View(baza.OsPlan2.Where(w=>w.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId).ToList());
        }

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.selectGodina = VratiSelectList();            
            return View();
        }

        [HttpPost]
        public ActionResult NoviPlan(OS_Plan_2 model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(!ModelState.IsValid)
            {
                ViewBag.selectGodina = VratiSelectList();                
                return View(model);
            }			
            model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.OsPlan2.Add(model);
                    db.SaveChanges();
                    TempData["note"] = "Novi plan za osnovnu školu 2 je spremljen!";
                }
                catch
                {
                    TempData["note"] = "Novi plan nije spremljen";
                }
            }            
			return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2 os_plan_2 = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (os_plan_2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.selectGodina = VratiSelectList();
            return View("Uredi", os_plan_2);
        }
        [HttpPost]
        public ActionResult Edit(OS_Plan_2 plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(!ModelState.IsValid)
            {
                ViewBag.selectGodina = VratiSelectList();
                return View("Uredi", plan);
            }
            OS_Plan_2 pl = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == plan.Id_plan && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            plan.Id_pedagog = pl.Id_pedagog;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.OsPlan2.Add(plan);
                    db.Entry(plan).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Plan je promijenjen";
                }
                catch
                {
                    TempData["note"] = "Plan nije promijenjen";
                }
            }            
			return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2 plan = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }            
            return View("Obrisi", plan);
        }

        [HttpPost]
        public ActionResult Delete(OS_Plan_2 plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2 pl = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == plan.Id_plan && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (pl == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }            
            using (var db = new BazaPodataka())
            {
                try
                {
                    var result = db.OsPlan2.SingleOrDefault(s => s.Id_plan == plan.Id_plan);
                    if (result != null)
                    {
                        db.OsPlan2.Remove(result);
                        db.SaveChanges();
                        TempData["note"] = "Plan je obrisan";
                    }
                    else
                    {
                        TempData["note"] = "Plan nije pronađen";
                    }
                }
                catch
                {
                    TempData["note"] = "Plan nije obrisan";
                }
            }
            return RedirectToAction("Index");
		}

		public FileStreamResult Ispis()
		{
			PlanOs2Report report = new PlanOs2Report(baza.OsPlan2.Where(w=>w.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId).ToList());

			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}
        public ActionResult Details(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2 = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan.OsPlan2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            plan.OsPlan2Podrucja = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).OrderBy(o=>o.Red_br_podrucje).ToList();
            return View(plan);
        }
        public ActionResult NovoPodrucje(int id)
        {
            //ulazni parametar id je id glavnog plana
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.id = id;
            ViewBag.selectCiljevi = VratiSelectCilj();
            ViewBag.selectSubjekti = VratiSelectSubjekti();
            ViewBag.selectZadaci = VratiSelectZadaci();
            ViewBag.selectOblici = VratiSelectOblici();
            return View(new OS_Plan_2_podrucje());
        }
        [HttpPost]
        public ActionResult NovoPodrucje (OS_Plan_2_podrucje model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(!ModelState.IsValid)
            {
                ViewBag.id = model.Id_glavni_plan;
                ViewBag.selectCiljevi = VratiSelectCilj();
                ViewBag.selectSubjekti = VratiSelectSubjekti();
                ViewBag.selectZadaci = VratiSelectZadaci();
                ViewBag.selectOblici = VratiSelectOblici();
                return View(model);
            }
            if (!PlanIsValid(model.Id_glavni_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            
            int id = model.Id_glavni_plan;
            int br;
            List<OS_Plan_2_podrucje> trenutni = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
            
            if (trenutni.Count == 0)
            {
                br = 1;
            }
            else
            {
                br = trenutni.Max(m => m.Red_br_podrucje)+1;                
            }
            model.Red_br_podrucje = br;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Podrucje.Add(model);
                    db.SaveChanges();
                    TempData["note"] = "Novi posao je spremljen";
                }
                catch
                {
                    TempData["note"] = "Novi posao nije spremljen";
                }
            }
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult UrediPodrucje(int id, int broj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.selectCiljevi = VratiSelectCilj();
            ViewBag.selectSubjekti = VratiSelectSubjekti();
            ViewBag.selectZadaci = VratiSelectZadaci();
            ViewBag.selectOblici = VratiSelectOblici();

            ViewBag.broj = broj.ToString() + ".";
            OS_Plan_2_podrucje model = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            if (!PlanIsValid(model.Id_glavni_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View("NovoPodrucje",model);
        }
        [HttpPost]
        public ActionResult UrediPodrucje (OS_Plan_2_podrucje model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(!ModelState.IsValid)
            {
                ViewBag.selectCiljevi = VratiSelectCilj();
                ViewBag.selectSubjekti = VratiSelectSubjekti();
                ViewBag.selectZadaci = VratiSelectZadaci();
                ViewBag.selectOblici = VratiSelectOblici();

                if (Request.Params.HasKeys())
                {
                    ViewBag.broj = Request.Params.Get("broj");
                }
                return View("NovoPodrucje", model);
            }
            OS_Plan_2_podrucje podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == model.Id_plan);
            if (podrucje == null || !PlanIsValid(podrucje.Id_glavni_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            model.Id_glavni_plan = podrucje.Id_glavni_plan;
            model.Red_br_podrucje = podrucje.Red_br_podrucje;
            //model.Sati = podrucje.Sati;
            int id = model.Id_glavni_plan;
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Podrucje.Add(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["note"] = "Posao je promijenjen";
                }
                catch
                {
                    TempData["note"] = "Posao nije promijenjen";
                }
            }
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult ObrisiPodrucje(int id, int broj)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }            
            OS_Plan_2_podrucje model = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            if(model==null || !PlanIsValid(model.Id_glavni_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.broj = broj + ".";
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiPodrucje(OS_Plan_2_podrucje model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.Id_plan;            
            
            using (var db = new BazaPodataka())
            {
                var result = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
                if (result != null && PlanIsValid(result.Id_glavni_plan))
                {
                    try
                    {
                        id = result.Id_glavni_plan;
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
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult PodrucjePomakGore(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_podrucje podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            if (podrucje == null || !PlanIsValid(podrucje.Id_glavni_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int id_ = podrucje.Id_glavni_plan;

            int pozicija = podrucje.Red_br_podrucje;
            List<OS_Plan_2_podrucje> pod = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id_ && w.Red_br_podrucje <= pozicija).ToList();
                 

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
                        TempData["note"] = "Posao je pomaknut prema gore";
                    }
                    catch
                    {
                        TempData["note"] = "Dogodila se greška. Posao nije pomaknut";
                    }
                }
            }

            return RedirectToAction("Details", new { id = id_ });
        }
        public ActionResult PodrucjePomakDolje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_podrucje podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            if (podrucje == null || !PlanIsValid(podrucje.Id_glavni_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int idGlavniPlan = podrucje.Id_glavni_plan;
            int pozicija = podrucje.Red_br_podrucje;
            List<OS_Plan_2_podrucje> pod = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == idGlavniPlan && w.Red_br_podrucje >= pozicija).ToList();
            
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
                        TempData["note"] = "Posao je pomaknut prema dolje";
                    }
                    catch
                    {
                        TempData["note"] = "Dogodila se greška. Posao nije pomaknut";
                    }
                }
            }
            return RedirectToAction("Details", new { id = idGlavniPlan });
        }
        public ActionResult Aktivnosti(int idPodrucje, int id)
        {
            //ulazni parametar idPodrucje je id područja djelovanja
            //ulazni parametar id je id glavnog plana
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id > 0 && idPodrucje == 0)
            {
                if (!PlanIsValid(id)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                OS_Plan_2 plan = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                List<OS_Plan_2_podrucje> podrucja = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).OrderBy(o => o.Red_br_podrucje).ToList();
                List<OS_Plan_2_aktivnost> aktivnosti = new List<OS_Plan_2_aktivnost>();
                ViewBag.id = 0;
                if (podrucja.Count>0)
                {
                    int _id = podrucja.ElementAt(0).Id_plan;
                    aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == _id).OrderBy(o => o.Red_br_aktivnost).ToList();
                    ViewBag.id = _id;
                }
                ViewBag.podrucja = podrucja;
                return View(aktivnosti);
            }
            else if (id == 0 && idPodrucje > 0)
            {
                OS_Plan_2_podrucje podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
                if (podrucje == null || !PlanIsValid(podrucje.Id_glavni_plan))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                List<OS_Plan_2_aktivnost> aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPodrucje).OrderBy(o => o.Red_br_aktivnost).ToList();
                int _id = podrucje.Id_glavni_plan;
                List<OS_Plan_2_podrucje> podrucja = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == _id).OrderBy(o => o.Red_br_podrucje).ToList();
                ViewBag.podrucja = podrucja;
                ViewBag.id = podrucje.Id_plan;
                return View(aktivnosti);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }
        }
        public ActionResult NovaAktivnost (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id <= 0)
            {
                return RedirectToAction("Info", "OpciPodaci", new { poruka = "Morate dodati područje djelovanja" });
            }            
            ViewBag.id = id;            
            return View();
        }
        [HttpPost]
        public ActionResult NovaAktivnost(OS_Plan_2_aktivnost model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.id = model.Id_podrucje;
                return View(model);
            }
            if (!PodrucjeIsValid(model.Id_podrucje))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            int i = model.Id_podrucje;
            
            int maxValue;
            List<OS_Plan_2_aktivnost> trenutne = new List<OS_Plan_2_aktivnost>();
            trenutne = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == i).ToList();
            if (trenutne.Count == 0)
            {
                maxValue = 1;
            }
            else
            {
                maxValue = trenutne.Max(m => m.Red_br_aktivnost)+1;                
            }
            model.Red_br_aktivnost = maxValue;            
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Aktivnost.Add(model);
                    db.SaveChanges();                    
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return RedirectToAction("Aktivnosti", new { idPodrucje = model.Id_podrucje, id = 0 });
        }
        public ActionResult AktivnostPomakGore (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_aktivnost aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            OS_Plan_2_podrucje podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == aktivnost.Id_podrucje);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            int pozicija = aktivnost.Red_br_aktivnost;

            List<OS_Plan_2_aktivnost> aktivnosti = new List<OS_Plan_2_aktivnost>();
            aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == aktivnost.Id_podrucje && w.Red_br_aktivnost <= pozicija).ToList();
            aktivnosti = aktivnosti.OrderBy(o => o.Red_br_aktivnost).ToList();

            if (aktivnosti.Count == 1)
            {
                return RedirectToAction("Aktivnosti", new { idPodrucje = podrucje.Id_plan, id = 0 });
            }

            int pozicija_prethodni = aktivnosti.ElementAt(aktivnosti.Count - 2).Red_br_aktivnost;
            int id_prethodni = aktivnosti.ElementAt(aktivnosti.Count - 2).Id_plan;

            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
                var rezultat1 = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id_prethodni);
                if (rezultat != null && rezultat1 != null)
                {
                    try
                    {
                        rezultat.Red_br_aktivnost = pozicija_prethodni;
                        rezultat1.Red_br_aktivnost = pozicija;
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

            OS_Plan_2_aktivnost aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            OS_Plan_2_podrucje podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == aktivnost.Id_podrucje);
            if (!PlanIsValid(podrucje.Id_glavni_plan)) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);


            List<OS_Plan_2_aktivnost> aktivnosti = new List<OS_Plan_2_aktivnost>();
            aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == aktivnost.Id_podrucje && w.Red_br_aktivnost >= aktivnost.Red_br_aktivnost).ToList();
            aktivnosti = aktivnosti.OrderBy(o => o.Red_br_aktivnost).ToList();
            if (aktivnosti.Count == 1)
            {
                return RedirectToAction("Aktivnosti", new { idPodrucje = podrucje.Id_plan, id = 0 });
            }
            int pozicija = aktivnost.Red_br_aktivnost;
            int pozicija_slijedeći = aktivnosti.ElementAt(1).Red_br_aktivnost;
            int id_slijedeći = aktivnosti.ElementAt(1).Id_plan;
            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
                var rezultat1 = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id_slijedeći);

                if (rezultat != null && rezultat1 != null)
                {
                    try
                    {
                        rezultat.Red_br_aktivnost = pozicija_slijedeći;
                        rezultat1.Red_br_aktivnost = pozicija;
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
        public ActionResult UrediAktivnost(int id, int pozicija, string tekst)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Aktivnost = new OS_Plan_2_aktivnost();
            plan.OsPlan2Aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pozicija;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult UrediAktivnost(PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (plan.OsPlan2Aktivnost.Opis_aktivnost == null)
            {
                return View(plan);
            }
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
        public ActionResult ObrisiAktivnost (int id, int pozicija, string tekst)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Aktivnost = new OS_Plan_2_aktivnost();
            plan.OsPlan2Aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pozicija;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult ObrisiAktivnost(PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = plan.OsPlan2Aktivnost.Id_plan;
            int idPodrucje = plan.OsPlan2Aktivnost.Id_podrucje;
            OS_Plan_2_podrucje pod = new OS_Plan_2_podrucje();
            pod = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
            using(var db = new BazaPodataka())
            {
                var result = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
                var result1 = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
                if (result != null)
                {
                    try
                    {
                        result1.Sati -= result.Sati;
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
        public ActionResult NovaAkcija(int id,int pA, int pB)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.Pozicija = pA;
            plan.Pozicija2 = pB;
            plan.Id = id;
            return View(plan);
        }
        [HttpPost]
        public ActionResult NovaAkcija(PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(plan.OsPlan2Akcija.Opis_akcija==null || plan.OsPlan2Akcija.Sati < 0)
            {
                return View(plan);
            }
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
            int sati = plan.OsPlan2Akcija.Sati;
            using(var db=new BazaPodataka())
            {
                var result = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
                var result1 = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
                try
                {
                    db.OsPlan2Akcija.Add(plan.OsPlan2Akcija);
                    result.Sati += sati;
                    result1.Sati += sati;
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
        public ActionResult UrediAkcija(int id,int pA, int pB, string tekst)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Akcija = new OS_Plan_2_akcija();
            plan.OsPlan2Akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pA;
            plan.Pozicija2 = pB;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult UrediAkcija(PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(plan.OsPlan2Akcija.Opis_akcija==null || plan.OsPlan2Akcija.Sati < 0)
            {
                return View(plan);
            }
            int idAkcija = plan.OsPlan2Akcija.Id_plan;
            int idAktivnost = plan.OsPlan2Akcija.Id_aktivnost;
            OS_Plan_2_aktivnost aktivnost = new OS_Plan_2_aktivnost();
            aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
            int idPodrucje = aktivnost.Id_podrucje;
            OS_Plan_2_podrucje podrucje = new OS_Plan_2_podrucje();
            podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
            
            OS_Plan_2_akcija akcija = new OS_Plan_2_akcija();
            akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == idAkcija);
            int sati = akcija.Sati;
            using(var db=new BazaPodataka())
            {
                var result = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
                var result1 = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
                try
                {
                    result.Sati -= sati;
                    result.Sati += plan.OsPlan2Akcija.Sati;
                    result1.Sati -= sati;
                    result1.Sati += plan.OsPlan2Akcija.Sati;
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
        public ActionResult ObrisiAkcija(int id, int pA, int pB, string tekst)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2Akcija = new OS_Plan_2_akcija();
            plan.OsPlan2Akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            plan.Pozicija = pA;
            plan.Pozicija2 = pB;
            plan.Tekst = tekst;
            return View(plan);
        }
        [HttpPost]
        public ActionResult ObrisiAkcija(PlanOs2View plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
                int sati = result.Sati;
                var result1 = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == idAktivnost);
                var result2 = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == idPodrucje);
                try
                {
                    if (result != null && result1!=null)
                    {
                        result1.Sati -= sati;
                        result2.Sati -= sati;
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
        public ActionResult AkcijaPomakGore (int id, int pa, int pb)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
        public ActionResult AkcijaPomakDolje(int id, int pa, int pb)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
        public FileStreamResult IspisDetalji(int id)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2 = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            plan.OsPlan2Podrucja = new List<OS_Plan_2_podrucje>();
            plan.OsPlan2Podrucja = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
            plan.OsPlan2Podrucja = plan.OsPlan2Podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            plan.OsPlan2Aktivnosti = new List<OS_Plan_2_aktivnost>();
            foreach(var item in plan.OsPlan2Podrucja)
            {
                int i = item.Id_plan;
                plan.OsPlan2Aktivnosti.AddRange(baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == i));
            }
            plan.OsPlan2Akcije = new List<OS_Plan_2_akcija>();
            foreach(var item in plan.OsPlan2Aktivnosti)
            {
                int i = item.Id_plan;
                plan.OsPlan2Akcije.AddRange(baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == i));
            }
            Pedagog ped = new Pedagog();
            int idPed = PlaniranjeSession.Trenutni.PedagogId;
            ped = baza.Pedagog.SingleOrDefault(s => s.Id_Pedagog == idPed);

            PlanOs2DetailsReport report = new PlanOs2DetailsReport(plan, ped);
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
        private SelectList VratiSelectOblici()
        {
            List<Oblici> oblici = oblici_db.ReadOblici();
            var select = new SelectList(oblici, "Naziv", "Naziv");
            return select;
        }
        private SelectList VratiSelectCilj()
        {
            List<Ciljevi> ciljevi = ciljevi_db.ReadCiljevi();
            var select = new SelectList(ciljevi, "Naziv", "Naziv");
            return select;
        }
        private SelectList VratiSelectZadaci()
        {
            List<Zadaci> zadaci = zadaci_db.ReadZadaci();
            var select = new SelectList(zadaci, "Naziv", "Naziv");
            return select;
        }
        private SelectList VratiSelectSubjekti()
        {
            List<Subjekti> subjekti = subjekti_db.ReadSubjekti();
            var select = new SelectList(subjekti, "Naziv", "Naziv");
            return select;
        }
        private bool PlanIsValid(int id)
        {
            OS_Plan_2 plan = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool PodrucjeIsValid(int id)
        {
            OS_Plan_2_podrucje podrucje = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
            if (podrucje == null)
            {
                return false;
            }
            else
            {
                return PlanIsValid(podrucje.Id_glavni_plan);
            }
        }
    }
}