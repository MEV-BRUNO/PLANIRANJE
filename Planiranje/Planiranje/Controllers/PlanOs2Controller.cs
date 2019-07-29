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
                    TempData["note"] = "Novi godišnji plan za osnovnu školu je spremljen!";
                }
                catch
                {
                    TempData["note"] = "Novi godišnji plan za osnovnu školu nije spremljen!";
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
                    TempData["note"] = "Godišnji plan za osnovnu školu je promijenjen";
                }
                catch
                {
                    TempData["note"] = "Dogodila se greška! Plan nije promjenjen!";
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
                        TempData["note"] = "Godišnji plan za osnovnu školu je obrisan";
                    }
                    else
                    {
                        TempData["note"] = "Godišnji plan nije pronađen!";
                    }
                }
                catch
                {
                    TempData["note"] = "Dogodila se greška! Plan nije obrisan!";
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
                return RedirectToAction("Info", "OpciPodaci", new { poruka = "Morate dodati posao" });
            }            
            ViewBag.id = id;            
            return View(new OS_Plan_2_aktivnost());
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
            UpdatePodrucje(model.Id_podrucje);
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
        public ActionResult UrediAktivnost(int id, string pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_aktivnost aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null || !PodrucjeIsValid(aktivnost.Id_podrucje))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            else
            {                
                ViewBag.broj = pozicija;
                return View(aktivnost);
            }
        }
        [HttpPost]
        public ActionResult UrediAktivnost(OS_Plan_2_aktivnost model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {                
                ViewBag.broj = Request.Form.Get("broj");
                return View(model);
            }
            if (!PodrucjeIsValid(model.Id_podrucje) || !AktivnostIsValid(model.Id_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }           
            
            int id_ak = model.Id_plan;
            int id_pod = model.Id_podrucje;
            OS_Plan_2_podrucje p = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id_pod);            
            
            
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Aktivnost.Add(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;                    
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            UpdatePodrucje(p.Id_plan);
            return RedirectToAction("Aktivnosti", new { idPodrucje = p.Id_plan, id = 0 });
        }
        public ActionResult ObrisiAktivnost (int id, string pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2_aktivnost aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null || !PodrucjeIsValid(aktivnost.Id_podrucje)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            ViewBag.pozicija = pozicija;            
            return View(aktivnost);
        }
        [HttpPost]
        public ActionResult ObrisiAktivnost(OS_Plan_2_aktivnost model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_akt = model.Id_plan;
            int id_pod;
            OS_Plan_2_aktivnost a = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id_akt);

            if (a == null || !PodrucjeIsValid(a.Id_podrucje)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            id_pod = a.Id_podrucje;

            OS_Plan_2_podrucje p = baza.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id_pod);
           
            using (var db = new BazaPodataka())
            {
                var aktivnost = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id_akt);
                if (aktivnost != null)
                {
                    try
                    {
                        db.OsPlan2Aktivnost.Remove(aktivnost);                        
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
        public ActionResult Akcije(int idAktivnost, int idPodrucje)
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
                List<OS_Plan_2_aktivnost> aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPodrucje).OrderBy(o => o.Red_br_aktivnost).ToList();
                ViewBag.aktivnosti = aktivnosti;
                List<OS_Plan_2_akcija> akcije = new List<OS_Plan_2_akcija>();
                if (aktivnosti.Count > 0)
                {
                    int idAkt = aktivnosti.First().Id_plan;
                    ViewBag.id = idAkt;
                    akcije = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == idAkt).OrderBy(o => o.Red_br_akcija).ToList();
                }
                else ViewBag.id = 0;           

                return View(akcije);
            }
            else
            {
                if (!AktivnostIsValid(idAktivnost))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<OS_Plan_2_akcija> akcije = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == idAktivnost).OrderBy(o => o.Red_br_akcija).ToList();
                idPodrucje = (from pod in baza.OsPlan2Podrucje
                              join ak in baza.OsPlan2Aktivnost on pod.Id_plan equals ak.Id_podrucje
                              where ak.Id_plan == idAktivnost
                              select pod.Id_plan).First();
                List<OS_Plan_2_aktivnost> aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPodrucje).OrderBy(o => o.Red_br_aktivnost).ToList();
                ViewBag.aktivnosti = aktivnosti;                
                ViewBag.id = idAktivnost;
                return View(akcije);
            }
        }
        public ActionResult NovaAkcija(int id)
        {
            //id je id aktivnosti
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id <= 0)
            {
                return RedirectToAction("Info", "OpciPodaci", new { poruka = "Morate dodati zadatke" });
            }
            ViewBag.id = id;
            return View(new OS_Plan_2_akcija());
        }
        [HttpPost]
        public ActionResult NovaAkcija(OS_Plan_2_akcija akcija)
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
            int i = akcija.Id_aktivnost;
            int maxValue;
            List<OS_Plan_2_akcija> trenutne = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == i).ToList();
            
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
                    db.OsPlan2Akcija.Add(akcija);
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            UpdateAktivnost(akcija.Id_aktivnost);
            return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
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
            OS_Plan_2_akcija akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            ViewBag.broj = pozicija;
            return View(akcija);
        }
        [HttpPost]
        public ActionResult UrediAkcija(OS_Plan_2_akcija akcija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.broj = Request.Form.Get("broj");
                return View(akcija);
            }
            if (!AkcijaIsValid(akcija.Id_plan) || !AktivnostIsValid(akcija.Id_aktivnost))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }            
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.OsPlan2Akcija.Add(akcija);
                    db.Entry(akcija).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            UpdateAktivnost(akcija.Id_aktivnost);
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
            OS_Plan_2_akcija akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            ViewBag.pozicija = pozicija;
            return View(akcija);
        }
        [HttpPost]
        public ActionResult ObrisiAkcija(OS_Plan_2_akcija akcija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!AkcijaIsValid(akcija.Id_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int id = akcija.Id_plan;
            using (var db = new BazaPodataka())
            {
                var result = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
                if (result != null)
                {
                    id = result.Id_aktivnost;
                    db.OsPlan2Akcija.Remove(result);
                    db.SaveChanges();
                }
                else
                {
                    id = 0;
                }
            }
            if (id != 0)
            {
                UpdateAktivnost(id);
            }
            return RedirectToAction("Akcije", new { idAktivnost = id, idPodrucje = 0 });
        }
        public ActionResult AkcijaPomakGore (int id)
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
            OS_Plan_2_akcija akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            //OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == akcija.Id_aktivnost);           

            int pozicija = akcija.Red_br_akcija;

            List<OS_Plan_2_akcija> akcije = new List<OS_Plan_2_akcija>();
            akcije = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == akcija.Id_aktivnost && w.Red_br_akcija <= pozicija).ToList();
            akcije = akcije.OrderBy(o => o.Red_br_akcija).ToList();

            if (akcije.Count <= 1)
            {
                return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
            }

            int pozicija_prethodni = akcije.ElementAt(akcije.Count - 2).Red_br_akcija;
            int id_prethodni = akcije.ElementAt(akcije.Count - 2).Id_plan;

            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
                var rezultat1 = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id_prethodni);
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
            OS_Plan_2_akcija akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            //OS_Plan_1_aktivnost aktivnost = baza.OsPlan1Aktivnost.SingleOrDefault(s => s.Id_plan == akcija.Id_aktivnost);           

            int pozicija = akcija.Red_br_akcija;

            List<OS_Plan_2_akcija> akcije = new List<OS_Plan_2_akcija>();
            akcije = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == akcija.Id_aktivnost && w.Red_br_akcija >= pozicija).ToList();
            akcije = akcije.OrderBy(o => o.Red_br_akcija).ToList();

            if (akcije.Count <= 1)
            {
                return RedirectToAction("Akcije", new { idAktivnost = akcija.Id_aktivnost, idPodrucje = 0 });
            }

            int pozicija_prethodni = akcije.ElementAt(1).Red_br_akcija;
            int id_prethodni = akcije.ElementAt(1).Id_plan;

            using (var db = new BazaPodataka())
            {
                var rezultat = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
                var rezultat1 = db.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id_prethodni);
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
        public ActionResult Kopiraj(int id)
        {
            // id je ig glavnog plana
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2 plan = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
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
        public ActionResult Kopiraj(OS_Plan_2 plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!PlanIsValid(plan.Id_plan))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (!ModelState.IsValid)
            {
                OS_Plan_2 pl = baza.OsPlan2.SingleOrDefault(s => s.Id_plan == plan.Id_plan && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                ViewBag.naziv = pl.Naziv;
                ViewBag.godina = pl.Ak_godina;
                ViewBag.select = VratiSelectList();
                return View(plan);
            }
            int id = plan.Id_plan;
            using (var db = new BazaPodataka())
            {
                plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                db.OsPlan2.Add(plan);
                db.SaveChanges();

                int idPlan = db.OsPlan2.Where(l => l.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId).Max(m => m.Id_plan);
                List<OS_Plan_2_podrucje> podrucja = db.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).ToList();

                foreach (var podrucje in podrucja)
                {
                    int idPodrucje = podrucje.Id_plan;
                    podrucje.Id_glavni_plan = idPlan;
                    db.OsPlan2Podrucje.Add(podrucje);
                    db.SaveChanges();

                    var pod = db.OsPlan2Podrucje.Where(l => l.Id_glavni_plan == idPlan).Max(m => m.Id_plan);
                    List<OS_Plan_2_aktivnost> aktivnosti = db.OsPlan2Aktivnost.Where(w => w.Id_podrucje == idPodrucje).ToList();
                    foreach (var aktivnost in aktivnosti)
                    {
                        int idAktivnost = aktivnost.Id_plan;
                        aktivnost.Id_podrucje = pod;
                        db.OsPlan2Aktivnost.Add(aktivnost);
                        db.SaveChanges();

                        var akt = db.OsPlan2Aktivnost.Where(l => l.Id_podrucje == pod).Max(m => m.Id_plan);
                        List<OS_Plan_2_akcija> akcije = db.OsPlan2Akcija.Where(w => w.Id_aktivnost == idAktivnost).ToList();
                        foreach (var akcija in akcije)
                        {
                            akcija.Id_aktivnost = akt;
                            db.OsPlan2Akcija.Add(akcija);
                            db.SaveChanges();
                        }
                    }
                }
            }
            TempData["note"] = "Plan je kopiran";
            return RedirectToAction("Index");
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
        private bool AktivnostIsValid(int id)
        {
            OS_Plan_2_aktivnost aktivnost = baza.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
            if (aktivnost == null)
            {
                return false;
            }
            else
            {
                return PodrucjeIsValid(aktivnost.Id_podrucje);
            }
        }
        private bool AkcijaIsValid(int id)
        {
            OS_Plan_2_akcija akcija = baza.OsPlan2Akcija.SingleOrDefault(s => s.Id_plan == id);
            if (akcija == null)
            {
                return false;
            }
            else
            {
                return AktivnostIsValid(akcija.Id_aktivnost);
            }
        }
        private void UpdatePodrucje(int id)
        {
            baza = new BazaPodataka();
            List<OS_Plan_2_aktivnost> aktivnosti = baza.OsPlan2Aktivnost.Where(w => w.Id_podrucje == id).ToList();
            int suma = 0;
            foreach(var item in aktivnosti)
            {
                suma += item.Sati;
            }
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.OsPlan2Podrucje.SingleOrDefault(s => s.Id_plan == id);
                    if (result != null)
                    {
                        result.Sati = suma;
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
            }
        }
        private void UpdateAktivnost(int id)
        {
            baza = new BazaPodataka();
            List<OS_Plan_2_akcija> akcije = baza.OsPlan2Akcija.Where(w => w.Id_aktivnost == id).ToList();
            int suma = 0;
            foreach(var item in akcije)
            {
                suma += item.Sati;
            }
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.OsPlan2Aktivnost.SingleOrDefault(s => s.Id_plan == id);
                    if (result != null)
                    {
                        result.Sati = suma;
                        db.SaveChanges();
                        UpdatePodrucje(result.Id_podrucje);
                    }
                }
                catch
                {

                }
            }
        }
    }
}