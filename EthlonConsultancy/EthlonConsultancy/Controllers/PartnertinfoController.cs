using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EthlonConsultancy;

namespace EthlonConsultancy.Controllers
{
    public class PartnertinfoController : Controller
    {
        EthlonEntities db = new EthlonEntities();


        public int Number()
        {
            Random ss = new Random();
            var ran = ss.Next(1, 100001);
            return ran;
        }

        public static string vari = "";
        // GET: Partnertinfo
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
            string doq,string nopl,string firmname,
            string expcity,string jdate,string ldate,
            string industry,string totalexperience, string experties,
            HttpPostedFileBase file)
        {
            var user = db.Partnerscvs.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                TempData["cvalreadyexit"] = "CV is already uploaded ";
                return RedirectToAction("Index", "Partnertinfo");

            }
            else { 
           
                var aa = Number() + file.FileName;
                vari = aa;
                string path = System.IO.Path.Combine(@"~/CVs/PartnerCV/" + aa);
                file.SaveAs(Server.MapPath(path));
                Partnerscv partner = new Partnerscv();
                partner.Name = name;
                partner.Email = email;
                partner.PrtCVUpload = vari;
                partner.City = city;
                partner.DOB = dob;
                partner.ReferBy = referby;
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
                partner.JoinDate = jdate;
                partner.LeaveDateOPW = ldate;
                partner.Industry = industry;
                partner.TotalExp = totalexperience;
                partner.Experties = experties;
                db.Partnerscvs.Add(partner);
                db.SaveChanges();
                TempData["successmessage"] = "Cv Successfully Uploaded";
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", partner.Email);
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
                return RedirectToAction("Index", "Partnertinfo");

            }

        }


       
        public FileResult DownloadFile(int id)
        {
            var us = db.Partnerscvs.Find(id);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/CVs/PartnerCV/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + us.PrtCVUpload);
            string fileName = us.PrtCVUpload;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }


}
