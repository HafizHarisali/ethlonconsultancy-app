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
    public class AdminsController : Controller
    {
        private EthlonEntities db = new EthlonEntities();

        // GET: Admins
        public ActionResult Index(string search)
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            if (search == null)
            {
                var admins = db.Admins.Include(a => a.AdminType);
                return View(admins.ToList());
            }
            else
            {
                var searching = db.Admins.Where(p => p.Name.Contains(search)).ToList();
                return View(searching);
            }
        }

        // GET: Admins/Details/5
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
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }

            else
            {
                ViewBag.AdminTypeId = new SelectList(db.AdminTypes, "Id", "Name");
                return View();
            }
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Password,Phone,Address,AdminTypeId")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminTypeId = new SelectList(db.AdminTypes, "Id", "Name", admin.AdminTypeId);
            return View(admin);
        }

        // GET: Admins/Edit/5
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
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                ViewBag.AdminTypeId = new SelectList(db.AdminTypes, "Id", "Name", admin.AdminTypeId);
                return View(admin);
            }
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Password,Phone,Address,AdminTypeId")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminTypeId = new SelectList(db.AdminTypes, "Id", "Name", admin.AdminTypeId);
            return View(admin);
        }

        // GET: Admins/Delete/5
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
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
        }

        // POST: Admins/Delete/5
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
                Admin admin = db.Admins.Find(id);
                db.Admins.Remove(admin);
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
