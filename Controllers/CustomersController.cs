//author : Okoro Ikenna Kingsley//


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
using System.Data.Entity;

namespace TravelExperts_Wrkshp_5.Controllers
{
    
    public class CustomersController : Controller
    {
        public bool isUpdated = false;
        private TravelExpertsEntities db = new TravelExpertsEntities();
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
                    string name = customer.CustFirstName;   //get the customer first name from the customer object 
                    string username = customer.CustUsername;  //get the customer username from the customer object
                    string userPassword = customer.CustPassword; //get the customer password from the customer object

                    

                    if (chkUser == null)
                    {
                        //var keyNew = Helper.GenerateSalt(10);  //generate salt
                        //var password = Helper.EncodePassword(customer.CustPassword, keyNew);
                        var password = Helper.HashEncrypt(customer.CustPassword);

                        customer.CustPassword = password;
                        //create a salt table in the database and save the kewNew
                        context.Customers.Add(customer);
                        context.SaveChanges();

                        //call the SendEmail method
                        //send registration email to new customer
                        SendEmail(customer.CustEmail, "Registration Confirmed",
                            $"<p>Hi {name},<br/>Thank you for registering with Travel Experts where you explore, journey, discover and adventure.<br/>" +
                            $"Your username: {username}<br/> Your password: {userPassword}<br/> <br/> Travel Experts</p>");

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


     

        public ActionResult Update(int? id)
        {
            id = Convert.ToInt32(Session["CustomerID"]);  //an alternative way of getting the id from a session variable.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "CustomerId,CustFirstName,CustLastName,CustAddress,CustCity,CustProv,CustPostal,CustCountry,CustHomePhone,CustBusPhone,CustEmail,AgentId,CustUsername,CustPassword")] Customer customer)
        {
           
            if (ModelState.IsValid)
            {
                customer.CustPassword = Helper.HashEncrypt(customer.CustPassword);  //hash the new password just before update into the database
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                Session.Clear();
                Session.Abandon();
                isUpdated = true;
                ViewBag.updateSuccessMessage = "Customer Update completed!!";
                return RedirectToAction("Login");
                
            }
            return View(customer);
        }

    }
}
