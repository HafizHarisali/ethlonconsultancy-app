using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthlonConsultancy.Controllers
{
    public class ApplicantinfoController : Controller
    {

        // GET: Applicantinfo
        EthlonEntities db = new EthlonEntities();

        public int Number()
        {
            Random ss = new Random();
            var ran = ss.Next(1, 100001);
            return ran;
        }
        public static string vari = "";
        // GET: Api
       

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Applicantpartners()
        {
            if (Session["AppId"] == null)
            {
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Applicantpartners(string name, string email,string country,
           string city, string dob, string referby,
           string qualification, string qualifystatus,
           string doq, string nopl, string firmname,
           string expcity, string jdate, string ldate,
           string industry, string totalexperience, string experties,
           string relevantindustry,
           HttpPostedFileBase file,int Appid)
        {
            var user = db.Applicantpartnerscvs.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                TempData["cvalreadyexit"] = "CV is already uploaded ";
                return RedirectToAction("Applicantpartners", "Applicantinfo");

            }
            else
            {

                var aa = Number() + file.FileName;
                vari = aa;
                string path = System.IO.Path.Combine(@"~/CVs/Applicantcvs/" + aa);
                file.SaveAs(Server.MapPath(path));
                Applicantpartnerscv aplicantpart = new Applicantpartnerscv();
                aplicantpart.Name = name;
                aplicantpart.Email = email;
                aplicantpart.AppCVUpload = vari;
                aplicantpart.Country = country;
                aplicantpart.City = city;
                aplicantpart.DOB = dob;
                aplicantpart.ReferBy = referby;
                if (qualification == "CA" || qualification == "ACCA")
                {
                    aplicantpart.QName = qualification;
                    aplicantpart.QStatus = qualifystatus;
                }
                else if (qualification == "Graduation")
                {
                    aplicantpart.GName = qualification;
                    aplicantpart.GStatus = qualifystatus;
                }
                aplicantpart.DOQ = doq;
                aplicantpart.NOPL = nopl;
                aplicantpart.FirmName = firmname;
                aplicantpart.ExpCity = expcity;
                aplicantpart.JoinDate = jdate;
                aplicantpart.LeaveDateOPW = ldate;
                aplicantpart.Industry = industry;
                aplicantpart.TotalExp = totalexperience;
                aplicantpart.Experties = experties;
                aplicantpart.RelevantIndustry = relevantindustry;
                aplicantpart.Applicantid = Appid;
                db.Applicantpartnerscvs.Add(aplicantpart);
                db.SaveChanges();
                TempData["successmessage"] = "Cv Successfully Uploaded";
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", aplicantpart.Email);
                msg.IsBodyHtml = true;
                msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
                msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
                msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear " + name + ",</h3>";
                msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been submitted</p>";
                msg.Body += "</div>";
                msg.Subject = "CV";
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
                client.Port = 587;
                client.Host = "mail.ethlongroup.com";
                client.EnableSsl = true;
                client.Send(msg);
                return RedirectToAction("Applicantpartners", "Applicantinfo");

            }

        }

        public ActionResult EditApplicantPartnerCv(int id)
        {
            if (Session["AppId"] != null)
            {
                var cv = db.Applicantpartnerscvs.Where(c => c.Id == id);
                ViewBag.cv = cv;
                return View();
            }
            else
            {
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
        }

        [HttpPost]
        public ActionResult EditApplicantPartnerCv(int id, int appid, string name, string email, string country,
           string city, string dob, string referby,
           string qualification, string qualifystatus,
           string doq, string nopl, string firmname,
           string expcity, string jdate, string ldate,
           string industry, string totalexperience, string experties,
           string relevantindustry,
           HttpPostedFileBase file)
        {
                var aplicantpart = db.Applicantpartnerscvs.Find(id);
                var aa = Number() + file.FileName;
                vari = aa;
                string path = System.IO.Path.Combine(@"~/CVs/Applicantcvs/" + aa);
                file.SaveAs(Server.MapPath(path));
                aplicantpart.Name = name;
                aplicantpart.Email = email;
                aplicantpart.AppCVUpload = vari;
                aplicantpart.Country = country;
                aplicantpart.City = city;
                aplicantpart.DOB = dob;
                aplicantpart.ReferBy = referby;
                if (qualification == "CA" || qualification == "ACCA")
                {
                    aplicantpart.QName = qualification;
                    aplicantpart.QStatus = qualifystatus;
                }
                else if (qualification == "Graduation")
                {
                    aplicantpart.GName = qualification;
                    aplicantpart.GStatus = qualifystatus;
                }
                aplicantpart.DOQ = doq;
                aplicantpart.NOPL = nopl;
                aplicantpart.FirmName = firmname;
                aplicantpart.ExpCity = expcity;
                aplicantpart.JoinDate = jdate;
                aplicantpart.LeaveDateOPW = ldate;
                aplicantpart.Industry = industry;
                aplicantpart.TotalExp = totalexperience;
                aplicantpart.Experties = experties;
                aplicantpart.RelevantIndustry = relevantindustry;
                db.SaveChanges();
                TempData["successmessage"] = "Cv Successfully Uploaded";
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", aplicantpart.Email);
                msg.IsBodyHtml = true;
                msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
                msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
                msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear " + name + ",</h3>";
                msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been submitted</p>";
                msg.Body += "</div>";
                msg.Subject = "CV";
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
                client.Port = 587;
                client.Host = "mail.ethlongroup.com";
                client.EnableSsl = true;
                client.Send(msg);
                return RedirectToAction("Applicantpartners", "Applicantinfo");
            
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            if (Session["AppId"] == null)
            {
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(int id,string name, string email,
            string address,string contactno, string dob, string referby,
            string schoolname,string collegename, string academicqualification,
            string advanceacademicqualification, string qualification, 
            string qualifystatus,
            string gpa, string doq, string nopl, string university,
            string majorsubject, string other,string extracuri,
            string achievements, string experiencestatus,
            string noymexp, string orgname, string check)
        {
            //var aa = Number() + file.FileName;
            //vari = aa;
            //string path = System.IO.Path.Combine("/CVs/Orgcvs/" + aa);
            //file.SaveAs(Server.MapPath(path));
            Applicantscv applicant = new Applicantscv();
            applicant.Name = name;
            applicant.Email = email;
            applicant.Address = address;
            applicant.Contactno = contactno;
            applicant.Age = dob;
            applicant.ReferBy = referby;
            applicant.Schoolname = schoolname;
            applicant.Collegename = collegename;
            applicant.AcadQual = academicqualification;
            applicant.AdvAcadQual = advanceacademicqualification;
            if (qualification == "CA(CAF)" || qualification == "ACCA")
            {
                applicant.Qualname = qualification;
            }
            else if (qualification == "BBA" || qualification == "MBA" || qualification == "B.Engineering")
            {
                applicant.GName = qualification;
            }
            applicant.Qualstatus = qualifystatus;
            applicant.DOQ = doq;
            applicant.NOPL = nopl;
            applicant.GPA = gpa;
            applicant.University = university;
            applicant.MajorSubjects = majorsubject;
            applicant.OtherSubjects = other;
            applicant.ExpStatus = experiencestatus;
            applicant.NOYExperience = noymexp;
            applicant.OrgName = orgname;
            applicant.Extracuri = extracuri;
            applicant.Achivements = achievements;
            applicant.Agree = check;
            applicant.Applicantid = id;
            db.Applicantscvs.Add(applicant);
            db.SaveChanges();
            int cvid = applicant.Id;
            TempData["generatemessage"] = "Cv Successfully Generated";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", applicant.Email);
            msg.IsBodyHtml = true;
            msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
            msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
            msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear " + name + ",</h3>";
            msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been submitted</p>";
            msg.Body += "</div>";
            msg.Subject = "CV";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
            client.Port = 587;
            client.Host = "mail.ethlongroup.com";
            client.EnableSsl = true;
            client.Send(msg);
            return RedirectToAction("Generatecv", "Applicantinfo", new { id=cvid});
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Generatecv(int id)
        {
            if(Session["AppId"] != null)
            {
                var cv = db.Applicantscvs.Where(c => c.Id == id);
                ViewBag.cv = cv;
                return View();
            }
            else
            {
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
            
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult EditCV(int id)
        {
            if(Session["AppId"] != null)
            {
                ViewBag.info = db.Applicantscvs.Where(c => c.Id == id);
                return View();
            }
            else
            {
                return RedirectToAction("ApplicantsLogin", "Accounts");
            }
            
        }
        [HttpPost]
        public ActionResult EditCV(int id,int appid, string name, string email,
            string address, string contactno, string dob, string referby,
            string schoolname, string collegename, string academicqualification,
            string advanceacademicqualification, string qualification,
            string qualifystatus,
            string gpa, string doq, string nopl, string university,
            string majorsubject, string other, string extracuri,
            string achievements, string experiencestatus,
            string noymexp, string orgname, string check)
        {

            var applicant = db.Applicantscvs.Find(id);
            //var aa = Number() + file.FileName;
            //vari = aa;
            //string path = System.IO.Path.Combine("/CVs/Orgcvs/" + aa);
            //file.SaveAs(Server.MapPath(path));
            
            applicant.Name = name;
            applicant.Email = email;
            applicant.Address = address;
            applicant.Contactno = contactno;
            applicant.Age = dob;
            applicant.ReferBy = referby;
            applicant.Schoolname = schoolname;
            applicant.Collegename = collegename;
            applicant.AcadQual = academicqualification;
            applicant.AdvAcadQual = advanceacademicqualification;
            if (qualification == "CA(CAF)" || qualification == "ACCA")
            {
                applicant.Qualname = qualification;
                applicant.GName = "";
            }
            else if (qualification == "BBA" || qualification == "MBA" || qualification == "B.Engineering")
            {
                applicant.GName = qualification;
                applicant.Qualname = "";
            }
            applicant.Qualstatus = qualifystatus;
            applicant.DOQ = doq;
            applicant.NOPL = nopl;
            applicant.GPA = gpa;
            applicant.University = university;
            applicant.MajorSubjects = majorsubject;
            applicant.OtherSubjects = other;
            applicant.ExpStatus = experiencestatus;
            applicant.NOYExperience = noymexp;
            applicant.OrgName = orgname;
            applicant.Extracuri = extracuri;
            applicant.Achivements = achievements;
            applicant.Agree = check;
            //applicant.UploadCV = vari;
            db.SaveChanges();
            TempData["successmessage"] = "Cv Successflly Updated";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", applicant.Email);
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
            return RedirectToAction("Generatecv", "Applicantinfo", new { id = id });
        }

        public ActionResult Uploadcv(int id)
        {
            ViewBag.get = db.Applicantscvs.Where(a => a.Id == id);
            return View();
        }

        [HttpPost]
        public ActionResult UploadCv(int id,HttpPostedFileBase file)
        {
            var applicant = db.Applicantscvs.Find(id);
            var aa = Number() + file.FileName;
            vari = aa;
            string path = System.IO.Path.Combine("/CVs/Orgcvs/" + aa);
            file.SaveAs(Server.MapPath(path));
            applicant.UploadCV = vari;
            db.SaveChanges();
            TempData["uploadmessage"] = "Cv Successfully Uploaded";
            return RedirectToAction("Generatecv","Applicantinfo",new { id=applicant.Id});
        }

        public FileResult DownloadFile(int id)
        {
            var us = db.Applicantscvs.Find(id);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/CVs/Orgcvs/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + us.UploadCV);
            string fileName = us.UploadCV;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadApplicantCV(int id)
        {
            var us = db.Applicantpartnerscvs.Find(id);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/CVs/Applicantcvs/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + us.AppCVUpload);
            string fileName = us.AppCVUpload;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}