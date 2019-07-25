using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EthlonConsultancy;


namespace EthlonConsultancy.Controllers
{
    public class OrginfoController : Controller
    {
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
        public ActionResult Index()
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Accounts");
            }
            else
            {
                return View();
            } 
        }
       
        [HttpPost]
        public ActionResult Index(string name,string email,
            string city,string dob,string referby,
            string qualification,string qualifystatus,
            string gpa,string doq,string nopl,string university,
            string majorsubject,string other, string experiencestatus,
            string noymexp,string orgname,string educationboard, HttpPostedFileBase file)
        {
            var aa = Number() + file.FileName;
            vari = aa;
            string path = System.IO.Path.Combine("/CVs/Orgcvs/" + aa);
            file.SaveAs(Server.MapPath(path));
            Organizationscv organi = new Organizationscv();
            organi.Name = name;
            organi.Email = email;
            organi.OrgCVUpload = vari;
            organi.City = city;
            organi.DOB = dob;
            organi.EducationBoard = educationboard;
            organi.ReferBy = referby;
            if(qualification=="CA(CAF)" || qualification=="ACCA")
            {
                organi.QName = qualification;
            }
            else if(qualification=="BBA" || qualification=="MBA" ||qualification=="B.Engineering")
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
            db.Organizationscvs.Add(organi);
            db.SaveChanges();
            TempData["successmessage"] = "Cv Successfully Uploaded";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", organi.Email);
            msg.IsBodyHtml = true;
            msg.Body = "<div style='width:auto;height:300px;background-color:orange;border-radius:10px;padding:20px;'>";
            msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
            msg.Body += "<h3 style='text-align:center;margin-top:70px;'>Dear "+name+",</h3>";
            msg.Body += "<p style='text-align:center;margin-top:50px;font-size:20px;'>Your CV has been submitted</p>";
            msg.Body += "</div>";
            msg.Subject = "CV";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");        
            client.Port = 587;
            client.Host = "mail.ethlongroup.com";
            client.EnableSsl = true;
            client.Send(msg);
            return RedirectToAction("Index", "Orginfo");
        }

        public FileResult DownloadFile(int id)
        {
            var us = db.Organizationscvs.Find(id);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/CVs/Orgcvs/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + us.OrgCVUpload);
            string fileName = us.OrgCVUpload;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}