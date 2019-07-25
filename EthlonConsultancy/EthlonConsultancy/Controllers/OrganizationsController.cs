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
    public class OrganizationsController : Controller
    {
        private EthlonEntities db = new EthlonEntities();

        // GET: Organizations
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index(string search)
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            if(search==null)
            {
                var organizations = db.Organizations.Include(o => o.Status);
                return View(organizations.ToList());
            }
            else
            {
                var searching = db.Organizations.Where(o => o.Email.Contains(search)).ToList();
                return View(searching);
            }
        }

        // GET: Organizations/Details/5
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
                Organization organization = db.Organizations.Find(id);
                if (organization == null)
                {
                    return HttpNotFound();
                }
                return View(organization);
            }
        }

        // GET: Organizations/Create
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

        // POST: Organizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Password,CoordinatorName,CoordinatorPhone,IndustryNuture,Wallet,StatusId")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Organizations.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", organization.StatusId);
            return View(organization);
        }

        // GET: Organizations/Edit/5
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
                Organization organization = db.Organizations.Find(id);
                if (organization == null)
                {
                    return HttpNotFound();
                }
                ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", organization.StatusId);
                return View(organization);
            }
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Password,CoordinatorName,CoordinatorPhone,IndustryNuture,Wallet,StatusId")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", organization.StatusId);
            return View(organization);
        }

        // GET: Organizations/Delete/5
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
                Organization organization = db.Organizations.Find(id);
                if (organization == null)
                {
                    return HttpNotFound();
                }
                return View(organization);
            }
        }

        // POST: Organizations/Delete/5
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
                Organization organization = db.Organizations.Find(id);
                db.Organizations.Remove(organization);
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
