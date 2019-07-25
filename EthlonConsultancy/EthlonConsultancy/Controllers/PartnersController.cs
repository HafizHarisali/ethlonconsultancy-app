using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EthlonConsultancy;

namespace EthlonConsultancy.Controllers
{
    public class PartnersController : Controller
    {
        private EthlonEntities db = new EthlonEntities();

        // GET: Partners
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index(string search)
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }

            if(search==null)
            {
                var partners = db.Partners.Include(p => p.Status);
                return View(partners.ToList());
            }
            else
            {
                var searching = db.Partners.Where(p =>p.Email.Contains(search)).ToList();
                return View(searching);
            }
        }

        // GET: Partners/Details/5
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }

            else
            {
                Partner partner = db.Partners.Find(id);
                if (partner == null)
                {
                    return HttpNotFound();
                }
                return View(partner);
            }
        }

        // GET: Partners/Create
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Create()
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
                return View();
            }
        }

        // POST: Partners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Country,City,FirmName,Email,Password,StatusId")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                db.Partners.Add(partner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", partner.StatusId);
            return View(partner);
        }

        // GET: Partners/Edit/5
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }

            else
            {
                Partner partner = db.Partners.Find(id);
                if (partner == null)
                {
                    return HttpNotFound();
                }
                ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", partner.StatusId);
                return View(partner);
            }
        }

        // POST: Partners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Country,City,FirmName,Email,Password,StatusId")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", partner.StatusId);
            return View(partner);
        }

        // GET: Partners/Delete/5
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }

            else
            {
                Partner partner = db.Partners.Find(id);
                if (partner == null)
                {
                    return HttpNotFound();
                }
                return View(partner);
            }
        }

        // POST: Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }

            else
            {


                Partner partner = db.Partners.Find(id);
                db.Partners.Remove(partner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
