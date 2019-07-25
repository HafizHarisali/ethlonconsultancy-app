using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EthlonConsultancy;

namespace EthlonConsultancy.Controllers
{
    public class EducationController : Controller
    {
        EthlonEntities db = new EthlonEntities();
        // GET: Education
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ApplicantpartnersCA()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
            else
            {
                var getappprtca = db.ApplicantpartnerACCAViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getappprtca = getappprtca;
                return View();
            }

        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ApplicantpartnersBBA()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
            else
            {
                var getappprtbba = db.ApplicantpartnerBBAViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getappprtbba = getappprtbba;
                return View();
            }

        }


        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult BBA()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("OrganizationsLogin", "Accounts");
            }
            else
            {
                var getbbacv = db.OrganizationGpaViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getbbacv = getbbacv;
                return View();
            }
           
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult CA()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("OrganizationsLogin", "Accounts");
            }
            else
            {
                var getcacv = db.OrganizationQualViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getcacv = getcacv;
                return View();
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult PCA()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UsersLogin", "Accounts");
            }
            else
            {
                var getpcacv = db.PartnersQualViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getpcacv = getpcacv;
                return View();
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult PBBA()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UsersLogin", "Accounts");
            }
            else
            {
                var getpbbacv = db.PartnersGpaViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getpbbacv = getpbbacv;
                return View();
            }
        }

        public ActionResult PostJob()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("OrganizationsLogin", "Accounts");
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult PostJob(string orgname,string jobtitle, DateTime deadline, string qualrequirements)
        {
            var date=DateTime.Now;
            PostJob postjob = new PostJob();
            postjob.OrgName = orgname;
            postjob.JobTitle = jobtitle;
            postjob.CurrentDate = date;
            postjob.Deadline = deadline;
            postjob.QualRequirements = qualrequirements;
            db.PostJobs.Add(postjob);
            db.SaveChanges();
            TempData["successmessage"] = "Job Posted Successfully";
            return RedirectToAction("PostJob", "Education");
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ApplicantCA()
        {
            if (Session["OrgId"] == null)
            {
                return RedirectToAction("OrganizationsLogin", "Accounts");
            }
            else
            {
                var getcacv = db.ApplicantCAViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getcacv = getcacv;
                return View();
            }
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ApplicantBBA()
        {
            if (Session["OrgId"] == null)
            {
                return RedirectToAction("OrganizationsLogin", "Accounts");
            }
            else
            {
                var getbbacv = db.ApplicantBBAViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getbbacv = getbbacv;
                return View();
            }

        }

    }
}