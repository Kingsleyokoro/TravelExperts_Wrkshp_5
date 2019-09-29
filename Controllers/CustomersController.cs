using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelExperts_Wrkshp_5.Models;
using TravelExperts_Wrkshp_5.Helpers;

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
                    if (chkUser == null)
                    {
                        //var keyNew = Helper.GeneratePassword(10);
                        //var password = Helper.EncodePassword(customer.CustPassword, keyNew);
                        var password = Helper.HashEncrypt(customer.CustPassword);

                        customer.CustPassword = password;
                        
                        context.Customers.Add(customer);
                        context.SaveChanges();
                        ModelState.Clear();
                        ViewBag.SuccessMessage = "Registration Successful";
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
    }
}
