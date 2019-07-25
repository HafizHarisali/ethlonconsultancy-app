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
    public class EmployeLogoesController : Controller
    {
        private EthlonEntities db = new EthlonEntities();

        // GET: EmployeLogoes
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                return View(db.EmployeLogos.ToList());
            }
        }

        // GET: EmployeLogoes/Details/5
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Details(int? id)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeLogo employeLogo = db.EmployeLogos.Find(id);
                if (employeLogo == null)
                {
                    return HttpNotFound();
                }
                return View(employeLogo);
            }
        }

        // GET: EmployeLogoes/Create
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Create()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                return View();
            }
        }

        // POST: EmployeLogoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            Random ss = new Random();
            var ran = ss.Next(1, 100001);
            var vari = ran + file.FileName;
            var save = vari;
            string path = System.IO.Path.Combine("/Logos/" + vari);
            file.SaveAs(Server.MapPath(path));
            EmployeLogo emplogo = new EmployeLogo();
            emplogo.Image = save;
            db.EmployeLogos.Add(emplogo);
            db.SaveChanges();
            TempData["successmessage"] = "Logo Added Successfully";
            return View();
        }

        // GET: EmployeLogoes/Edit/5
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Edit(int? id)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeLogo employeLogo = db.EmployeLogos.Find(id);
                if (employeLogo == null)
                {
                    return HttpNotFound();
                }
                return View(employeLogo);
            }
        }

        // POST: EmployeLogoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image")] EmployeLogo employeLogo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeLogo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeLogo);
        }

        // GET: EmployeLogoes/Delete/5
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Delete(int? id)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeLogo employeLogo = db.EmployeLogos.Find(id);
                if (employeLogo == null)
                {
                    return HttpNotFound();
                }
                return View(employeLogo);
            }
        }

        // POST: EmployeLogoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeLogo employeLogo = db.EmployeLogos.Find(id);
            db.EmployeLogos.Remove(employeLogo);
            db.SaveChanges();
            return RedirectToAction("Index");
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
