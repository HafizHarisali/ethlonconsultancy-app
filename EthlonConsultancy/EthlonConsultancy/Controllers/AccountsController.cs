using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EthlonConsultancy;

namespace EthlonConsultancy.Controllers
{
    public class AccountsController : Controller
    {
        EthlonEntities db = new EthlonEntities();
        
        // GET: Accounts

        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(string email, string pass)
        {
            var admin = db.Admins.FirstOrDefault(u => u.Email == email && u.Password == pass && u.AdminType.Name == "Admin");
            var manager = db.Admins.FirstOrDefault(u => u.Email == email && u.Password == pass && u.AdminType.Name == "Manager");
            

            if (admin != null)
            {
                Session["Id"] = admin.Id;
                Session["Name"] = admin.Name;
                Session["Email"] = admin.Email;
                Session["admintype"] = admin.AdminType.Name;
                return RedirectToAction("Index", "Home");
            }
            else if (manager != null)
            {
                Session["Id"] = manager.Id;
                Session["Name"] = manager.Name;
                Session["Email"] = manager.Email;
                Session["managertype"] = manager.AdminType.Name;
                return RedirectToAction("Index", "Home");
            }

            
            else
            {
                TempData["message"] = "Invalid Username or password";
                return RedirectToAction("AdminLogin","Accounts");
            }
            
        }

        public ActionResult UsersLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UsersLogin(string email, string password)
        {
            var partnerapprove = db.Partners.FirstOrDefault(p => p.Email == email && p.Password == password && p.Status.Name == "Approved");
            var partnerpending = db.Partners.FirstOrDefault(p => p.Email == email && p.Password == password && p.Status.Name == "Pending");
            var partnerdenied = db.Partners.FirstOrDefault(p => p.Email == email && p.Password == password && p.Status.Name == "Denied");


                if (partnerapprove != null && partnerapprove.Status.Name == "Approved")
                {
                   Session["Id"] = partnerapprove.Id;
                   Session["Name"] = partnerapprove.Name;
                   Session["FirmName"] = partnerapprove.FirmName;
                   return RedirectToAction("PCA", "Education");
                }

                else if (partnerpending != null && partnerpending.Status.Name == "Pending")
                {
                    TempData["pendingmessage"] = "You are not approved yet";
                    return RedirectToAction("UsersLogin", "Accounts");
                }
                else if (partnerdenied != null && partnerdenied.Status.Name == "Denied")
                {
                    TempData["deniedmessage"] = "Your request has been deleted by Admin";
                    return RedirectToAction("UsersLogin", "Accounts");

                }

                else
                {
                    TempData["message"] = "Incorrect username or password";
                    return View();
                }
        
        }

        public ActionResult ApplicantsLogin()
        {
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
                if(check_app_id != null)
                {
                    return RedirectToAction("EditApplicantPartnerCv", "Applicantinfo", new { id = applicantapprove.Applicantpartnerscvs.FirstOrDefault().Id });
                }
                else if(applicantapprove.Type == "Experienced Professional" && applicantapprove.Status.Name == "Approved")
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
                return View();
            }

        }

        public ActionResult OrganizationsLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrganizationsLogin(string email,string password)
        {
            var orgapprove = db.Organizations.FirstOrDefault(o => o.Email == email && o.Password == password && o.Status.Name == "Approved");
            var orgpending = db.Organizations.FirstOrDefault(o => o.Email == email && o.Password == password && o.Status.Name == "Pending");
            var orgdenied = db.Organizations.FirstOrDefault(o => o.Email == email && o.Password == password && o.Status.Name == "Denied");

            if (orgapprove != null && orgapprove.Status.Name == "Approved")
            {
                Session["Id"] = orgapprove.Id;
                Session["Name"] = orgapprove.Name;
                Session["Email"] = orgapprove.Email;
                Session["OrgId"] = orgapprove.Id;

                return RedirectToAction("CA", "Education");
            }

            else if (orgpending != null && orgpending.Status.Name == "Pending")
            {
                TempData["pendingmessage"] = "You are not approved yet";
                return RedirectToAction("OrganizationsLogin", "Accounts");
            }
            else if (orgdenied != null && orgdenied.Status.Name == "Denied")
            {
                TempData["deniedmessage"] = "Your request has been deleted by Admin";
                return RedirectToAction("OrganizationsLogin", "Accounts");

            }

            else
            {
                TempData["message"] = "Incorrect username or password";
                return View();
            }


            
        }


        public ActionResult AdminLogout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("AdminLogin", "Accounts");
        }

        public ActionResult OrgLogout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("OrganizationsLogin", "Accounts");
        }

        public ActionResult PrtLogout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("UsersLogin", "Accounts");
        }
        public ActionResult ApplicantLogout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("ApplicantsLogin", "Accounts");
        }


        public ActionResult PartnersSignup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PartnersSignup(string name,string country,string city,string firmname,string email,string password,int statusid=1)
        {
            var checkexistance = db.Partners.Where(a=>a.Email==email);
            if (checkexistance.Count() > 0)
            {
                TempData["Checkmail"] = "Email Already Exist!";
                return RedirectToAction("PartnersSignup","Accounts");
            }
            else
            {
                Partner partner = new Partner();
                partner.Name = name;
                partner.Country = country;
                partner.City = city;
                partner.FirmName = firmname;
                partner.Email = email;
                partner.Password = password;
                partner.StatusId = statusid;
                db.Partners.Add(partner);
                db.SaveChanges();
                if (partner != null)
                {
                    TempData["statusmessage"] = "Your request will soon approved by admin";

                }
                return View();
            }
           
        }

        public ActionResult ApplicantsSignup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApplicantsSignup(string name, string country, string city, string firmname,string universityname, string email, string password,string type, int statusid = 2)
        {
            var checkexistance = db.Applicants.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                TempData["Checkmail"] = "Email Already Exist!";
                return RedirectToAction("ApplicantsSignup", "Accounts");
            }
            else
            {
                Applicant applicant = new Applicant();
                applicant.Name = name;
                applicant.Country = country;
                applicant.City = city;
                applicant.FirmName = firmname;
                applicant.UniversityName = universityname;
                applicant.Email = email;
                applicant.Password = password;
                applicant.Type = type;
                applicant.Statusid = statusid;
                db.Applicants.Add(applicant);
                db.SaveChanges();
                TempData["statusmessage"] = "Successfully Registered.You can login now";
                return RedirectToAction("ApplicantsLogin","Accounts");
            }

        }

        public ActionResult OrganizationsSignup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrganizationsSignup(string name, string email, string password, string coordinatorname, string coordinatorphone, string industrynature,string wallet="0", int statusid = 1)
        {
            var checkexistance = db.Organizations.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                TempData["Checkmail"] = "Email Already Exist!";
                return RedirectToAction("OrganizationsSignup", "Accounts");
            }
            else
            {
                Organization organization = new Organization();
                organization.Name = name;
                organization.Email = email;
                organization.Password = password;
                organization.CoordinatorName = coordinatorname;
                organization.CoordinatorPhone = coordinatorphone;
                organization.IndustryNuture = industrynature;
                organization.Wallet = wallet;
                organization.StatusId = statusid;
                db.Organizations.Add(organization);
                db.SaveChanges();
                if (organization != null)
                {
                    TempData["statusmessage"] = "Your request will soon approve by admin";

                }
                return View();
            }
        }

        public ActionResult Forgotpassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forgotpassword(string email)
        {
            var checkexistance = db.Applicants.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                var applicants = db.Applicants.FirstOrDefault(a => a.Email == email);
                string name = applicants.Name;
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", applicants.Email);
                msg.IsBodyHtml = true;
                msg.Body = "<div style='width:auto;height:350px;background-color:orange;border-radius:10px;padding:20px;'>";
                msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
                msg.Body += "<h3 style='text-align:center;margin-top:50px;'>Dear " + name + ",</h3>";
                msg.Body += "<p style='text-align:center;margin-top:30px;font-size:20px;'>click on the following link to change your password</p>";
                msg.Body += "<a href='http://ethlonconsultancy.com/Accounts/Changeapplicantpassword' style='margin-left:370px;margin-top:30px;font-size:18px;background-color:#d11e46;border-radius:0px;color:white;padding:20px;'>Change Password</a>";
                msg.Body += "</div>";
                msg.Subject = "CV";
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
                client.Port = 587;
                client.Host = "mail.ethlongroup.com";
                client.EnableSsl = true;
                client.Send(msg);
                TempData["successmesage"] = "Email sent! Check your mail";
                return View();
            }
            else
            {
                TempData["errormesage"] = "Email address not match";
                return View();
            }
        }

        public ActionResult OrgForgotpassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrgForgotpassword(string email)
        {
            var checkexistance = db.Organizations.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                var organi = db.Organizations.FirstOrDefault(a => a.Email == email);
                string name = organi.Name;
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", organi.Email);
                msg.IsBodyHtml = true;
                msg.Body = "<div style='width:auto;height:350px;background-color:orange;border-radius:10px;padding:20px;'>";
                msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
                msg.Body += "<h3 style='text-align:center;margin-top:50px;'>Dear " + name + ",</h3>";
                msg.Body += "<p style='text-align:center;margin-top:30px;font-size:20px;'>click on the following link to change your password</p>";
                msg.Body += "<a href='http://ethlonconsultancy.com/Accounts/Changeorgpassword' style='margin-left:370px;margin-top:30px;font-size:18px;background-color:#d11e46;border-radius:0px;color:white;padding:20px;'>Change Password</a>";
                msg.Body += "</div>";
                msg.Subject = "CV";
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
                client.Port = 587;
                client.Host = "mail.ethlongroup.com";
                client.EnableSsl = true;
                client.Send(msg);
                TempData["successmesage"] = "Email sent! Check your mail";
                return View();
            }
            else
            {
                TempData["errormesage"] = "Email address not match";
                return View();
            }
        }

        public ActionResult PrtForgotpassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PrtForgotpassword(string email)
        {
            var checkexistance = db.Partners.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                var prt = db.Partners.FirstOrDefault(a => a.Email == email);
                string name = prt.Name;
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("consultancy@ethlongroup.com", prt.Email);
                msg.IsBodyHtml = true;
                msg.Body = "<div style='width:auto;height:350px;background-color:orange;border-radius:10px;padding:20px;'>";
                msg.Body += "<h3 style='text-align:center;color:black;font-size:35px;margin-top:30px;'>Ethlon Consultancy</h3>";
                msg.Body += "<h3 style='text-align:center;margin-top:50px;'>Dear " + name + ",</h3>";
                msg.Body += "<p style='text-align:center;margin-top:30px;font-size:20px;'>click on the following link to change your password</p>";
                msg.Body += "<a href='http://ethlonconsultancy.com/Accounts/Changeprtpassword' style='margin-left:370px;margin-top:30px;font-size:18px;background-color:#d11e46;border-radius:0px;color:white;padding:20px;'>Change Password</a>";
                msg.Body += "</div>";
                msg.Subject = "CV";
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("consultancy@ethlongroup.com", "AX4Pgh2DtB#E");
                client.Port = 587;
                client.Host = "mail.ethlongroup.com";
                client.EnableSsl = true;
                client.Send(msg);
                TempData["successmesage"] = "Email sent! Check your mail";
                return View();
            }
            else
            {
                TempData["errormesage"] = "Email address not match";
                return View();
            }
        }



        public ActionResult Changeapplicantpassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Changeapplicantpassword(string email,string password,string cpassword)
        {
            var checkexistance = db.Applicants.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                var applicant = db.Applicants.Where(a=>a.Email==email);
                if (applicant != null)
                {
                    if (password == cpassword)
                    {
                        applicant.FirstOrDefault().Password = password;
                        db.SaveChanges();
                        TempData["successmessage"] = "Password updated succesfully";
                       
                    }
                    else
                    {
                        TempData["passerrormessage"] = "Password Don't match";
                    }
                }
                else
                {
                    TempData["warningmessage"] = "Something went wrong";
                }
                return View();
            }
            else
            {
                TempData["errormessage"] = "Email not match";
                return View();
            }
        }

        public ActionResult Changeorgpassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Changeorgpassword(string email, string password, string cpassword)
        {
            var checkexistance = db.Organizations.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                var org = db.Organizations.Where(a => a.Email == email);
                if (org != null)
                {
                    if (password == cpassword)
                    {
                        org.FirstOrDefault().Password = password;
                        db.SaveChanges();
                        TempData["successmessage"] = "Password updated succesfully";

                    }
                    else
                    {
                        TempData["passerrormessage"] = "Password Don't match";
                    }
                }
                else
                {
                    TempData["warningmessage"] = "Something went wrong";
                }
                return View();
            }
            else
            {
                TempData["errormessage"] = "Email not match";
                return View();
            }
        }

        public ActionResult Changeprtpassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Changeprtpassword(string email, string password, string cpassword)
        {
            var checkexistance = db.Partners.Where(a => a.Email == email);
            if (checkexistance.Count() > 0)
            {
                var prt = db.Partners.Where(a => a.Email == email);
                if (prt != null)
                {
                    if (password == cpassword)
                    {
                        prt.FirstOrDefault().Password = password;
                        db.SaveChanges();
                        TempData["successmessage"] = "Password updated succesfully";

                    }
                    else
                    {
                        TempData["passerrormessage"] = "Password Don't match";
                    }
                }
                else
                {
                    TempData["warningmessage"] = "Something went wrong";
                }
                return View();
            }
            else
            {
                TempData["errormessage"] = "Email not match";
                return View();
            }
        }




    }
}