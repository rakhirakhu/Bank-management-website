using Bank_Management.DAL;
using Bank_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Bank_Management.Controllers
{
    [Authorize]
    [NoCache]
    public class AdminController : Controller
    {
        // GET: Admin
        adminDAL _Admindal = new adminDAL();
        HashPassword _hashPassword = new HashPassword();
        userdal _Userdal = new userdal();



        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Homes()
        {
            return RedirectToAction("Index", "Home");
        }
      

        public ActionResult RegistrationAdmin()
        {
            ViewBag.Message = "Your application description page";
            return View();
        }
        /// <summary>
        /// Retrieves the admin ID based on the provided username.
        /// </summary>
        /// <param name="Username">The username of the admin.</param>
        /// <returns>The admin ID if found, otherwise 0.</returns>
        [HttpPost]
        public ActionResult RegistrationAdmin(registeruser registerdetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    registerdetail.Password = _hashPassword.Hash(registerdetail.Password);

                    bool isInserted = _Admindal.insertuser(registerdetail);
                    if (isInserted)
                    {
                        TempData["SuccessMessage"] = "Registration successful";
                        return RedirectToAction("");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to register";
                    }
                }
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = "A database error occurred: " + ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return View();
        }


        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        /// <summary>
        /// Handles the HTTP POST request to edit a resource identified by the specified ID.
        /// </summary>
        /// <param name="id">The identifier of the resource to edit.</param>
        /// <param name="collection">A collection of form data representing the updated resource.</param>
        /// <returns>Redirects to the "Index" action if the update is successful; otherwise, returns the current view.</returns>
        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult CreateBankAccount(int userId ,string AccountNumber)
        {
            

            try
            {
                if (AccountNumber !=null)
                {
                    TempData["ErrorMessage"] = "User already have an account ";
                    return RedirectToAction("UserDetail");
                }

                registeruser user = _Admindal.GetUserById(userId);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction("UserDetail");
                }

                BankAccount bankAccount = new BankAccount
                {

                    UserId = userId,
                    AccountHolderName = $"{user.FirstName} {user.LastName}",
                    // Set other properties as needed...
                };

                return View(bankAccount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CreateBankAccount (GET): {ex.Message}");
                return View("Error");
            }
        }



        /// <summary>
        /// Handles the HTTP POST request to create a new bank account.
        /// </summary>
        /// <param name="account">The bank account details submitted via the form.</param>
        /// <returns>
        /// Redirects to the specified action if the creation is successful;
        /// otherwise, returns the current view with appropriate error messages.
        /// </returns>
        [HttpPost]
        public ActionResult CreateBankAccount(BankAccount bankAccount)
        {
            ////////////////////////    if(_Admindal.IsAccountnumberused(account.AccountNumber))
            ////////////////////////    {
            ////////////////////////        ModelState.AddModelError("AccountNumber", "Account number is already in use");
            ////////////////////////        return View(account);
            ////////////////////////    }
            ////////////////////////    try
            ////////////////////////    {
            ////////////////////////        if (ModelState.IsValid)
            ////////////////////////        {
            ////////////////////////            bool isInserted = _Admindal.CreateBankAccount(account);
            ////////////////////////            if (isInserted)
            ////////////////////////            {
            ////////////////////////                TempData["SuccessMessage"] = "Registration successful";
            ////////////////////////                return RedirectToAction("");
            ////////////////////////            }
            ////////////////////////            else
            ////////////////////////            {
            ////////////////////////                TempData["ErrorMessage"] = "Unable to register";
            ////////////////////////            }
            ////////////////////////        }
            ////////////////////////    }
            ////////////////////////    catch (SqlException ex)
            ////////////////////////    {
            ////////////////////////        TempData["ErrorMessage"] = "A database error occurred: " + ex.Message;
            ////////////////////////    }


            ////////////////////////    return View();
            ////////////////////////}



            try
            {
                if (ModelState.IsValid)
                {
                    bool isAccountCreated = _Admindal.CreateBankAccountForUser(bankAccount);

                    if (isAccountCreated)
                    {
                        TempData["SuccessMessage"] = "Bank account created successfully.";
                        return RedirectToAction("UserDetail");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to create bank account.";
                    }
                }

                return View(bankAccount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CreateBankAccount (POST): {ex.Message}");
                return View("Error");
            }
        }
        public ActionResult Accountinfo()
        {
            List<BankAccount> users = _Admindal.GetAccounts();
            return View(users);
        }
        public ActionResult Usertransaction()
        {
           // Assuming you have stored user's BankId in session

            // Retrieve transaction details for the user
            List<Transaction> transactions = _Admindal.getusertransaction();

            return View(transactions);
        }
        public ActionResult AllUsersBalance()
        {
            var userBalances = _Admindal.GetAllUsersBalance();
            return View(userBalances);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        /// <summary>
        /// Handles the change password form submission, changes the user's password, and redirects to the Index view.
        /// </summary>
        /// <param name="oldPassword">The user's old password.</param>
        /// <param name="newPassword">The user's new password.</param>
        /// <returns>Redirects to the Index view.</returns>
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            string username = Session["Username"] as string;

            try
            {
                string hasedOldPassword = _hashPassword.Hash(oldPassword);
                string hashedNewPassword = _hashPassword.Hash(newPassword);

                bool isPasswordChanged = _Admindal.ChangeAdminPassword(username, hasedOldPassword, hashedNewPassword);

                if (isPasswordChanged)
                {
                    TempData["SuccessMessage"] = "Password changed successfully";
                    return RedirectToAction("ChangePassword", "Admin");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to change password. Please check your old password.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return View();
        }
        /// <summary>
        /// Retrieves a list of user details and renders the UserDetail view.
        /// </summary>
        /// <returns>
        ///     The UserDetail view populated with a list of user details.
        /// </returns>
       
        public ActionResult UserDetail()
        {
            try
            {
                List<registeruser> users = _Admindal.GetUsers();

                return View(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in UserDetail: {ex.Message}");

                Logger.Error("Error occurred", ex);

                return View("Error");
            }
        }

        /// <summary>
        /// Retrieves a list of enquiry details and renders the Enquiry view.
        /// </summary>
        /// <returns>
        ///     The Enquiry view populated with a list of enquiry details.
        /// </returns>
        
        public ActionResult Enquiry()
        {
            try
            {
                List<EnquiryModel> enquiries = _Admindal.EnquiryDetails();

                return View(enquiries);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Enquiry: {ex.Message}");

                Logger.Error("Error occurred", ex);

                return View("Error");
            }
        }

        public ActionResult Logout()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            // Clear session or authentication tokens
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            // Redirect to the login page or any other page
            return RedirectToAction("Login", "Home");
        }
        /// <summary>
        /// Handles the HTTP GET request to delete user details based on the provided user ID.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted.</param>
        /// <returns>
        ///     If the ID is not provided, returns an error view.
        ///     If the user has related bank account records, redirects to UserDetail with an error message.
        ///     If the user details are deleted successfully, redirects to UserDetail with a success message.
        ///     If an error occurs during the process, returns an error view.
        /// </returns>
        [HttpPost]

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrorMessage = "User ID is not provided.";
                    return View("Error");
                }

                int userId = id.Value;

                // Check if the user has related records in tbl_bankaccount
                //bool hasRelatedRecords = _Admindal.DeleteUser(userId);

                //if (hasRelatedRecords)
                //{
                //    TempData["ErrorMessage"] = "Cannot delete user with related bank account records.";
                //    return RedirectToAction("UserDetail");
                //}

                bool result = _Admindal.DeleteUser(userId);

                if (result)
                {
                    TempData["SuccessMessage"] = "User details deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the details";
                }

                return RedirectToAction("UserDetail");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }





    }
}
