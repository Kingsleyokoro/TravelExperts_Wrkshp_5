using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelExperts_Wrkshp_5.Models;
using TravelExperts_Wrkshp_5.Helpers;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace TravelExperts_Wrkshp_5.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        
        

        public ActionResult Registration()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Registration(Customer customer)
        {
            try
            {
                
                using (var context = new TravelExpertsEntities())
                {
                    var chkUser = (from s in context.Customers where s.CustUsername == customer.CustUsername select s).FirstOrDefault();
                    string name = customer.CustFirstName;
                    string username = customer.CustUsername;
                    string userPassword = customer.CustPassword;

                    //call the SendEmail method
                    SendEmail(customer.CustEmail, "Registration Confirmed", 
                        $"<p>Hi {name},<br/>Thank you for registering with Travel Experts where you explore, journey, discover and adventure.<br/>" +
                        $"Your username: {username}<br/> Your password: {userPassword}<br/> <br/> Travel Experts</p>");

                    if (chkUser == null)
                    {
                        //var keyNew = Helper.GenerateSalt(10);  //generate salt
                        //var password = Helper.EncodePassword(customer.CustPassword, keyNew);
                        var password = Helper.HashEncrypt(customer.CustPassword);

                        customer.CustPassword = password;
                        //create a salt table in the database and save the kewNew
                        context.Customers.Add(customer);
                        context.SaveChanges();
                        ModelState.Clear();
                       
                        ViewBag.SuccessMessage = "Registration Successful!\nA Confirmation email has been sent to your Email address.";
                        //return RedirectToAction("LogIn", "Login");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Username Already Exists! Please enter a new username.";
                    }
                    
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Some exception occured" + e;
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LogIn(string userName, string password)
        {
            try
            {
                using (var context = new TravelExpertsEntities())
                {
                    var getUser = (from s in context.Customers where s.CustUsername == userName select s).FirstOrDefault();
                    if (getUser != null)
                    {
                        //var hashCode = Helper.GeneratePassword(10);   //get the salt from the database
                        //Password Hasing Process Call Helper Class Method    
                        // var encodingPasswordString = Helper.EncodePassword(password, hashCode);  //has the input password again the salt stored in the database
                        var encodingPasswordString = Helper.HashEncrypt(password);  //encrypt pass word before checking database
                        Session["CustomerID"] = getUser.CustomerId.ToString();
                        Session["CustomerName"] = getUser.CustFirstName;

                        //Check Login Detail User Name Or Password    
                        var query = (from s in context.Customers where (s.CustUsername == userName) && s.CustPassword.Equals(encodingPasswordString) select s).FirstOrDefault();
                        if (query != null)
                        {
                            //RedirectToAction("Details/" + id.ToString(), "FullTimeEmployees");    
                            //return View("../Admin/Registration"); url not change in browser    
                            //return RedirectToAction("Registration", "Customers");
                            ViewBag.SuccessMessage = $"Login Completed with Customer ID of {Session["CustomerID"]}";
                        }
                        ViewBag.ErrorMessage = "Invallid User Name or Password";
                        return View();
                    }
                    ViewBag.ErrorMessage = "Invallid User Name or Password";
                    return View();
                }
            }
            catch (Exception )
            {
                ViewBag.ErrorMessage = " Some database error ocurred, Please try again";
                return View();
            }
        }



        public ActionResult Logout()
        {

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login");

        }

        
        //this method returns a true or false if email send was successfull
        //
        private bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                string senderEmail = "mrtompujnr@gmail.com";
                string senderPassword = "Kingsley15";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);



                return true;
            }
            catch (Exception)
            {
                
                return false;
            }
        }

        //public ActionResult SendEmail()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult SendEmail(string receiver, string subject, string message)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var senderEmail = new MailAddress("mrtompujnr@gmail.com", "Jamil");
        //            var receiverEmail = new MailAddress(receiver, "mrtompujnr@gmail.com");
        //            var password = "Kingsley15";
        //            var sub = subject;
        //            var body = message;
        //            var smtp = new SmtpClient
        //            {
        //                Host = "smtp.gmail.com",
        //                Port = 587,
        //                EnableSsl = true,
        //                DeliveryMethod = SmtpDeliveryMethod.Network,
        //                UseDefaultCredentials = false,
        //                Credentials = new NetworkCredential(senderEmail.Address, password)
        //            };
        //            using (var mess = new MailMessage(senderEmail, receiverEmail)
        //            {
        //                Subject = subject,
        //                Body = body
        //            })
        //            {
        //                smtp.Send(mess);
        //            }
        //            return View();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        ViewBag.Error = "Some Error";
        //    }
        //    return View();
        //}
    }
}
