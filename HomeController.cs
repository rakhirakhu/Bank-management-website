using Bank_Management.DAL;
using Bank_Management.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Bank_Management.Controllers
{
    public class HomeController : Controller
    {
        userdal _Userdal = new userdal();
        adminDAL _Admindal = new adminDAL();
        HashPassword _hashPassword = new HashPassword();
        public ActionResult Index()
        {
            try
            {
                //int data = 1 + 1;
                //Logger.info("Information on " + data);
            }
            catch (Exception ex)
            {
                Logger.Error("Error on: ", ex);
            }
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        /// <summary>
        /// Handles the submission of an enquiry form.
        /// </summary>
        /// <param name="enquiry">The model containing enquiry details.</param>
        /// <returns>Returns a view based on the success or failure of the enquiry submission.</returns>
        
        [HttpPost]
        public ActionResult Contact(EnquiryModel enquiry)
        {
            if (ModelState.IsValid)
            {
                bool isEnquiryInserted = _Userdal.InsertEnquiry(enquiry);

                if (isEnquiryInserted)
                {
                    TempData["SuccessMessage"] = "Enquiry submitted successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to submit enquiry. Please try again.";
                }
            }

            return View(enquiry);
        }



      
        public ActionResult Registration()
        {
            ViewBag.Message = "Your application description page";
            return View();
        }
        /// <summary>
        /// Handles the user registration process.
        /// </summary>
        /// <param name="registerdetail">The model containing user registration details.</param>
        /// <returns>Returns a view based on the success or failure of the registration process.</returns>
        [HttpPost]
        public ActionResult Registration(registeruser registerdetail)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (_Userdal.IsPhoneNumberAlreadyUsed(registerdetail.Phone))
                    {
                        ModelState.AddModelError("Phone", "Phone number is already in use");
                        return View(registerdetail);
                    }
                    if (_Userdal.IsEmailAlreadyUsed(registerdetail.Email))
                    {
                        ModelState.AddModelError("Email", "Email is already in use");
                        return View(registerdetail);
                    }

                    registerdetail.Password = _hashPassword.Hash(registerdetail.Password);
                    
                    bool isInserted = _Userdal.insertuser(registerdetail);

                    if (isInserted)
                    {
                        TempData["SuccessMessage"] = "Registration successful";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to register";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Displays the login view.
        /// </summary>
        /// <returns>Returns the login view.</returns>

        [HttpPost]

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]

        public ActionResult Login(userlogin login)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            try
            {
                if (ModelState.IsValid)
                {

                    login.Password = _hashPassword.Hash(login.Password);

                    string usertype = "";
                    usertype = _Userdal.validateLogin(login.Username, login.Password, out usertype);

                    if (!string.IsNullOrEmpty(usertype))
                    {
                        if (usertype.Equals("user", StringComparison.OrdinalIgnoreCase))
                        {

                            int bankId = _Userdal.GetUserIDByUsername(login.Username);
                            Session["BankId"] = bankId;
                            Session["Username"] = login.Username;
                            FormsAuthentication.SetAuthCookie(login.Username, true);
                            Session["Username"] = login.Username;

                            return RedirectToAction("Index", "User"); // Redirect to User view
                        }
                        else if (usertype.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            int bankId = _Admindal.GetAdminIDByUsername(login.Username);
                            Session["BankId"] = bankId;
                            Session["Username"] = login.Username;
                            FormsAuthentication.SetAuthCookie(login.Username, true);

                            return RedirectToAction("Index", "Admin"); // Redirect to Admin view
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid username or password.";
                        return View("Login"); // Redirect back to the login page
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request: " + ex.Message;
                // Log the exception or handle it accordingly
            }

            return View("Login");
        }

      
    }

}