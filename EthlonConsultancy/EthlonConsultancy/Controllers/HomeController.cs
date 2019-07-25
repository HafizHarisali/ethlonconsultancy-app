using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EthlonConsultancy;

namespace EthlonConsultancy.Controllers
{
    public class HomeController : Controller
    {
        EthlonEntities db = new EthlonEntities();
        // GET: Home
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
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

       
        public ActionResult Landing()
        {
            var date = DateTime.Now;
            var postjobs = db.PostJobs.Where(p =>p.Deadline > date).OrderByDescending(p=>p.Deadline).Take(6);
            ViewBag.postjobs = postjobs;
            var logos = db.EmployeLogos.ToList();
            ViewBag.logos = logos;
            return View();
        }

        [HttpPost]
        public ActionResult ApplicantsLogin(string email, string password)
        {
            var applicantapprove = db.Applicants.FirstOrDefault(p => p.Email == email && p.Password == password && p.Status.Name == "Approved");
            var applicantpending = db.Applicants.FirstOrDefault(p => p.Email == email && p.Password == password && p.Status.Name == "Pending");
            var applicantdenied = db.Applicants.FirstOrDefault(p => p.Email == email && p.Password == password && p.Status.Name == "Denied");
            var applicantcv = db.Applicantscvs.ToList();

            if (applicantapprove != null && applicantapprove.Status.Name == "Approved")
            {
                Session["Id"] = applicantapprove.Id;
                Session["Name"] = applicantapprove.Name;
                Session["AppId"] = applicantapprove.Id;
                var check = db.Applicantscvs.FirstOrDefault(u => u.Applicantid == applicantapprove.Id);
                var check_app_id = db.Applicantpartnerscvs.FirstOrDefault(u => u.Applicantid == applicantapprove.Id);
                //if(check != null)
                //{
                //    return RedirectToAction("Generatecv", "Applicantinfo", new { id = applicantapprove.Applicantscvs.FirstOrDefault().Id });
                //}
                //else if(applicantapprove.Type == "Fresh Candidates" && applicantapprove.Status.Name == "Approved")
                //{
                //    return RedirectToAction("Index", "Applicantinfo");
                //}
                //else if(applicantapprove.Type == "Experienced Professional" && applicantapprove.Status.Name == "Approved")
                //{
                //    return RedirectToAction("Applicantpartners","Applicantinfo");
                //}
                //else
                //{
                //    return View();
                //}
                if (check_app_id != null)
                {
                    return RedirectToAction("EditApplicantPartnerCv", "Applicantinfo", new { id = applicantapprove.Applicantpartnerscvs.FirstOrDefault().Id });
                }
                else if (applicantapprove.Type == "Experienced Professional" && applicantapprove.Status.Name == "Approved")
                {
                    return RedirectToAction("Applicantpartners", "Applicantinfo");
                }
                else
                {
                    return View();
                }

            }

            else if (applicantpending != null && applicantpending.Status.Name == "Pending")
            {
                TempData["pendingmessage"] = "You are not approved yet";
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
            else if (applicantdenied != null && applicantdenied.Status.Name == "Denied")
            {
                TempData["deniedmessage"] = "Your request has been deleted by Admin";
                return RedirectToAction("ApplicantsLogin", "Accounts");

            }

            else
            {
                TempData["message"] = "Incorrect username or password";
                return RedirectToAction("Landing","Home");
            }

        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Pcacvs()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                var pcacv = db.PartnersQualViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.pcacv = pcacv;
                return View();
            }
        }

        public int Number()
        {
            Random ss = new Random();
            var ran = ss.Next(1, 100001);
            return ran;
        }

        public static string vari = "";

        [HttpGet]
        public ActionResult EditPcacvs(int id)
        {
            Partnerscv partnercv = db.Partnerscvs.Find(id);
            return View(partnercv);
        }

        [HttpPost]
        public ActionResult EditPcacvs(int id,string name, string email,
            string City, string DOB, string ReferBy,
            string qualification, string qualifystatus,
            string doq, string nopl, string firmname,
            string expcity, string joindate, string ldate,
            string industry, string totalexperience, string experties,
            HttpPostedFileBase file)
        {
            var aa = Number() + file.FileName;
            vari = aa;
            string path = System.IO.Path.Combine(@"~/CVs/PartnerCV/" + aa);
            file.SaveAs(Server.MapPath(path));
            Partnerscv partner = new Partnerscv();
            partner.Id = id;
            partner.Name = name;
            partner.Email = email;
            partner.PrtCVUpload = vari;
            partner.City = City;
            partner.DOB = DOB;
            partner.ReferBy = ReferBy;
            if (qualification == "CA" || qualification == "ACCA")
            {
                partner.QName = qualification;
                partner.QStatus = qualifystatus;
            }
            else if (qualification == "Graduation")
            {
                partner.GName = qualification;
                partner.GStatus = qualifystatus;
            }
            partner.DOQ = doq;
            partner.NOPL = nopl;
            partner.FirmName = firmname;
            partner.ExpCity = expcity;
            partner.JoinDate = joindate;
            partner.LeaveDateOPW = ldate;
            partner.Industry = industry;
            partner.TotalExp = totalexperience;
            partner.Experties = experties;
            db.Entry(partner).State = EntityState.Modified;
            db.SaveChanges();
            TempData["successmessage"] = "Cv Successfully Updated";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", partner.Email);
            msg.IsBodyHtml = true;
            msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
            msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
            msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear " + name + ",</h3>";
            msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been updated</p>";
            msg.Body += "</div>";
            msg.Subject = "CV";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
            client.Port = 587;
            client.Host = "mail.ethlongroup.com";
            client.EnableSsl = true;
            client.Send(msg);
            return RedirectToAction("Editpcacvs", "Home",new { id});
        }


        public int Number1()
        {
            Random ss = new Random();
            var ran = ss.Next(1, 100001);
            return ran;
        }

        public static string vari1 = "";

        [HttpGet]
        public ActionResult EditPbbacvs(int id)
        {
            Partnerscv partnercv = db.Partnerscvs.Find(id);
            return View(partnercv);
        }

        [HttpPost]
        public ActionResult EditPbbacvs(int id, string name, string email,
            string City, string DOB, string ReferBy,
            string qualification, string qualifystatus,
            string doq, string nopl, string firmname,
            string expcity, string joindate, string ldate,
            string industry, string totalexperience, string experties,
            HttpPostedFileBase file)
        {
            var aa = Number1() + file.FileName;
            vari = aa;
            string path = System.IO.Path.Combine(@"~/CVs/PartnerCV/" + aa);
            file.SaveAs(Server.MapPath(path));
            Partnerscv partner = new Partnerscv();
            partner.Id = id;
            partner.Name = name;
            partner.Email = email;
            partner.PrtCVUpload = vari1;
            partner.City = City;
            partner.DOB = DOB;
            partner.ReferBy = ReferBy;
            if (qualification == "CA" || qualification == "ACCA")
            {
                partner.QName = qualification;
                partner.QStatus = qualifystatus;
            }
            else if (qualification == "Graduation")
            {
                partner.GName = qualification;
                partner.GStatus = qualifystatus;
            }
            partner.DOQ = doq;
            partner.NOPL = nopl;
            partner.FirmName = firmname;
            partner.ExpCity = expcity;
            partner.JoinDate = joindate;
            partner.LeaveDateOPW = ldate;
            partner.Industry = industry;
            partner.TotalExp = totalexperience;
            partner.Experties = experties;
            db.Entry(partner).State = EntityState.Modified;
            db.SaveChanges();
            TempData["successmessage"] = "Cv Successflly Updated";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", partner.Email);
            msg.IsBodyHtml = true;
            msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
            msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
            msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear " + name + ",</h3>";
            msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been updated</p>";
            msg.Body += "</div>";
            msg.Subject = "CV";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
            client.Port = 587;
            client.Host = "mail.ethlongroup.com";
            client.EnableSsl = true;
            client.Send(msg);
            return RedirectToAction("Editpbbacvs", "Home", new { id });
        }

        public ActionResult DeletePcacvs(int id)
        {
            var cv = db.Partnerscvs.Find(id);
            db.Partnerscvs.Remove(cv);
            db.SaveChanges();
            return RedirectToAction("Pcacvs","Home");
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Pbbacvs()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                var pbbacv = db.PartnersGpaViews.OrderByDescending(o => o.Id).ToList();
                
                ViewBag.pbbacv = pbbacv;
                return View();
            }
        }
        public ActionResult DeletePbbacvs(int id)
        {
            var cv = db.Partnerscvs.Find(id);
            db.Partnerscvs.Remove(cv);
            db.SaveChanges();
            return RedirectToAction("Pbbacvs", "Home");
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Ocacvs()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                var ocacv = db.OrganizationQualViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.ocacv = ocacv;
                return View();
            }
        }
        public ActionResult DeleteOcacvs(int id)
        {
            var cv = db.Organizationscvs.Find(id);
            db.Organizationscvs.Remove(cv);
            db.SaveChanges();
            return RedirectToAction("Ocacvs", "Home");
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Obbacvs()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                var obbacv = db.OrganizationGpaViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.obbacv = obbacv;
                return View();
            }
        }
        public ActionResult DeleteObbacvs(int id)
        {
            var cv = db.Organizationscvs.Find(id);
            db.Organizationscvs.Remove(cv);
            db.SaveChanges();
            return RedirectToAction("Obbacvs", "Home");
        }

        [HttpGet]
        public ActionResult EditOcacvs(int id)
        {
            Organizationscv orgcv = db.Organizationscvs.Find(id);
            return View(orgcv);
        }

        public int Numberorgca()
        {
            Random ss = new Random();
            var ran = ss.Next(1, 100001);
            return ran;
        }

        public static string variorgca = "";

        [HttpPost]
        public ActionResult EditOcacvs(int id,string name, string email,
            string city, string dob, string referby,
            string qualification, string qualifystatus,
            string gpa, string doq, string nopl, string university,
            string majorsubject, string other, string experiencestatus,
            string noymexp, string orgname, string educationboard, HttpPostedFileBase file)
        {
            var aa = Numberorgca() + file.FileName;
            variorgca = aa;
            string path = System.IO.Path.Combine("/CVs/Orgcvs/" + aa);
            file.SaveAs(Server.MapPath(path));
            Organizationscv organi = new Organizationscv();
            organi.Id = id;
            organi.Name = name;
            organi.Email = email;
            organi.OrgCVUpload = variorgca;
            organi.City = city;
            organi.DOB = dob;
            organi.EducationBoard = educationboard;
            organi.ReferBy = referby;
            if (qualification == "CA(CAF)" || qualification == "ACCA")
            {
                organi.QName = qualification;
            }
            else if (qualification == "BBA" || qualification == "MBA" || qualification == "B.Engineering")
            {
                organi.GName = qualification;
            }
            organi.QStatus = qualifystatus;
            organi.DOQ = doq;
            organi.NOPL = nopl;
            organi.GPA = gpa;
            organi.University = university;
            organi.MajorSubject = majorsubject;
            organi.OtherSubject = other;
            organi.ExpStatus = experiencestatus;
            organi.NOYMExperience = noymexp;
            organi.OrgName = orgname;
            db.Entry(organi).State = EntityState.Modified;
            db.SaveChanges();
            TempData["successmessage"] = "Cv Successfully Updated";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", organi.Email);
            msg.IsBodyHtml = true;
            msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
            msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
            msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear " + name + ",</h3>";
            msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been updated</p>";
            msg.Body += "</div>";
            msg.Subject = "CV";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
            client.Port = 587;
            client.Host = "mail.ethlongroup.com";
            client.EnableSsl = true;
            client.Send(msg);
            return RedirectToAction("EditOcacvs", "Home",new { id});
        }

        [HttpGet]
        public ActionResult EditObbacvs(int id)
        {
            Organizationscv orgcv = db.Organizationscvs.Find(id);
            return View(orgcv);
        }

        public int Numberorgbba()
        {
            Random ss = new Random();
            var ran = ss.Next(1, 100001);
            return ran;
        }

        public static string variorgbba = "";

        [HttpPost]
        public ActionResult EditObbacvs(int id, string name, string email,
            string city, string dob, string referby,
            string qualification, string qualifystatus,
            string gpa, string doq, string nopl, string university,
            string majorsubject, string other, string experiencestatus,
            string noymexp, string orgname, string educationboard, HttpPostedFileBase file)
        {
            var aa = Numberorgbba() + file.FileName;
            variorgbba = aa;
            string path = System.IO.Path.Combine("/CVs/Orgcvs/" + aa);
            file.SaveAs(Server.MapPath(path));
            Organizationscv organi = new Organizationscv();
            organi.Id = id;
            organi.Name = name;
            organi.Email = email;
            organi.OrgCVUpload = variorgbba;
            organi.City = city;
            organi.DOB = dob;
            organi.EducationBoard = educationboard;
            organi.ReferBy = referby;
            if (qualification == "CA(CAF)" || qualification == "ACCA")
            {
                organi.QName = qualification;
            }
            else if (qualification == "BBA" || qualification == "MBA" || qualification == "B.Engineering")
            {
                organi.GName = qualification;
            }
            organi.QStatus = qualifystatus;
            organi.DOQ = doq;
            organi.NOPL = nopl;
            organi.GPA = gpa;
            organi.University = university;
            organi.MajorSubject = majorsubject;
            organi.OtherSubject = other;
            organi.ExpStatus = experiencestatus;
            organi.NOYMExperience = noymexp;
            organi.OrgName = orgname;
            db.Entry(organi).State = EntityState.Modified;
            db.SaveChanges();
            TempData["successmessage"] = "Cv Successfully Updated";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", organi.Email);
            msg.IsBodyHtml = true;
            msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
            msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
            msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear " + name + ",</h3>";
            msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been updated</p>";
            msg.Body += "</div>";
            msg.Subject = "CV";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
            client.Port = 587;
            client.Host = "mail.ethlongroup.com";
            client.EnableSsl = true;
            client.Send(msg);
            return RedirectToAction("EditObbacvs", "Home", new { id });
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ApplicantCA()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
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
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                var getbbacv = db.ApplicantBBAViews.OrderByDescending(o => o.Id).ToList();
                ViewBag.getbbacv = getbbacv;
                return View();
            }

        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult DeleteApplicant(int id)
        {
            var cv = db.Applicantscvs.Find(id);
            db.Applicantscvs.Remove(cv);
            db.SaveChanges();
            return RedirectToAction("ApplicantCA", "Home");
        }


    }
}