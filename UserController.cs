using Bank_Management.DAL;
using Bank_Management.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Bank_Management.Controllers
{
    [Authorize]
    [NoCache]
    public class UserController : Controller
    {
        userdal _Userdal = new userdal();
        HashPassword _hashPassword = new HashPassword();


        // GET: User
        public ActionResult Index()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return View();
        }
        /// <summary>
        /// Inserts user registration details into the database.
        /// </summary>
        /// <param name="registerDetail">The user registration details.</param>
        /// <returns>True if the insertion is successful; otherwise, false.</returns>
        public ActionResult UserBankAccounts()
        {
            try
            {
                // Retrieve the userId from the session
                int userId = (int)Session["BankId"]; // Assuming you have stored user's BankId in session

                // Get the user's bank accounts using the DAL method
                List<BankAccount> userAccounts = _Userdal.GetUserBankAccounts(userId);

                // Return the bank accounts to the view
                return View(userAccounts);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred in UserBankAccounts: {ex.Message}");
                // Optionally rethrow the exception if needed
                // throw;

                // Redirect to an error view or perform other error handling
                return RedirectToAction("Error");
            }
        }


        // GET: User/Create
        public ActionResult Deposit()
        {
            return View();
        }

        /// <summary>
        /// Handles the HTTP POST request to deposit an amount.
        /// </summary>
        /// <param name="amount">The amount to deposit.</param>
        /// <param name="accountNumber">The account number to deposit to.</param>
        /// <param name="ifscCode">The IFSC code of the account.</param>
        /// <returns>Redirects to the UserBankAccounts action or Deposit view based on the result.</returns>
        [HttpPost]
        public ActionResult Deposit(decimal? amount, string accountNumber, string ifscCode, string password)
        {
            try
            {

                int userId = (int)Session["BankId"];
                string hashPassword = _hashPassword.Hash(password);



                bool isSuccess = _Userdal.Deposituser(userId, amount.Value, accountNumber, ifscCode, hashPassword);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Deposit successful";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to process deposit. Please try again.";
                }
                if (!amount.HasValue)
                {
                    TempData["ErrorMessage"] = "Amount is required for Deposit.";
                    return RedirectToAction("Deposit");

                }


                return RedirectToAction("Deposit");
            }

            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // For now, set an error message and redirect to the Deposit view
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Deposit");
            }
        }
        /// <summary>
        /// Displays the TransactionDetails view with the transaction details for the logged-in user.
        /// </summary>
        /// <returns>The TransactionDetails view.</returns>
        public ActionResult TransactionDetails()
        {
            try
            {
                int userId = (int)Session["BankId"];

                List<Transaction> transactions = _Userdal.GetTransactionsByUserId(userId);

                return View(transactions);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;

                return RedirectToAction("Error");
            }
        }


        public ActionResult Withdraw()
        {

            return View();

        }
        /// <summary>
        /// Handles the withdrawal form submission, processes the withdrawal, and redirects to the UserBankAccounts view.
        /// </summary>
        /// <param name="amount">The withdrawal amount.</param>
        /// <param name="accountNumber">The account number for withdrawal.</param>
        /// <param name="ifscCode">The IFSC code for withdrawal.</param>
        /// <returns>Redirects to the UserBankAccounts view.</returns>

        [HttpPost]

        public ActionResult Withdraw(decimal? amount, string accountNumber, string ifscCode, string password)
        {
            try
            {

                int userId = (int)Session["BankId"];
                string hashPassword = _hashPassword.Hash(password);


                bool isSuccess = _Userdal.Withdrawal(userId, amount.Value, accountNumber, ifscCode, hashPassword);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Withdrawel successful";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to process Withdraw. Please try again.";
                }
                if (!amount.HasValue)
                {
                    TempData["ErrorMessage"] = "Amount is required for withdrawal.";
                    return RedirectToAction("Withdraw");
                }

                return RedirectToAction("Withdraw");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Withdraw");
            }
        }



        public ActionResult Transfer()
        {
            return View(new TransferAmount());
        }
        /// <summary>
        /// Handles the funds transfer form submission, processes the transfer, and redirects to the Index view.
        /// </summary>
        /// <param name="transferDetails">The transfer details including source and destination account information.</param>
        /// <returns>Redirects to the Index view.</returns>
        [HttpPost]
        public ActionResult Transfer(TransferAmount transferDetails)
        {
            try
            {

                int UserId = (int)Session["BankId"];
                string hashPassword = _hashPassword.Hash(transferDetails.Password);



                transferDetails.UserId = UserId;
                transferDetails.ToUserId = _Userdal.GetUserIdByAccountNumber(transferDetails.ToAccountNumber);

                bool transferSuccess = _Userdal.UserTransfer(transferDetails.UserId, transferDetails.ToUserId, transferDetails.Amount, transferDetails.AccountNumber, transferDetails.IFSCCode, transferDetails.ToAccountNumber, transferDetails.ToIFSCCode, hashPassword);

                if (transferSuccess)
                {
                    TempData["SuccessMessage"] = "Funds transfer successful";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occurred during funds transfer. Check the console for details.";
                }

                return RedirectToAction("Transfer");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Transfer");
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

                bool isPasswordChanged = _Userdal.ChangeUserPassword(username, hasedOldPassword, hashedNewPassword);

                if (isPasswordChanged)
                {
                    TempData["SuccessMessage"] = "Password changed successfully";
                    return RedirectToAction("ChangePassword", "User");
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
        /// Displays the user details based on the current session's username.
        /// </summary>
        /// <returns>Returns a view containing the user details.</returns>
        public ActionResult UserDetails()
        {
            string username = Session["Username"] as string;

            try
            {
                // Check if the username is not null or empty
                if (!string.IsNullOrEmpty(username))
                {
                    // Call the DAL method to retrieve the user details
                    registeruser user = _Userdal.GetUserByUsername(username);

                    // Check if the user details are retrieved successfully
                    if (user != null)
                    {
                        // Return the UserDetails view with the user details
                        return View(user);
                    }
                }

                TempData["ErrorMessage"] = "User details not found.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                // throw;

                // Redirect to the Home/Index view in case of an error
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Edit()
        {
            string username = Session["Username"] as string;

            registeruser users = _Userdal.GetUserByUsername(username);



            return View(users);
        }
        /// <summary>
        /// Handles the HTTP POST request to edit user details.
        /// </summary>
        /// <param name="updatedUser">The updated user details.</param>
        /// <returns>Returns a redirect to the Index action of the User controller on success; otherwise, returns the Edit view.</returns>

        [HttpPost]

        public ActionResult Edit(registeruser updatedUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //updatedUser.Password = _hashPassword.Hash(updatedUser.Password);

                    if (updatedUser.ImageFile != null && updatedUser.ImageFile.ContentLength > 0)
                    {
                        using (BinaryReader reader = new BinaryReader(updatedUser.ImageFile.InputStream))
                        {
                            updatedUser.ProfileImage = reader.ReadBytes(updatedUser.ImageFile.ContentLength);
                        }
                    }


                    bool isUpdated = _Userdal.UpdateUserByUsername(updatedUser);

                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "User details updated successfully";
                        return RedirectToAction("UserDetails", "User");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update user details";
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

            return View("Edit", updatedUser);
        }


        /// <summary>
        /// Displays the balance enquiry page for the logged-in user.
        /// </summary>
        /// <returns>Returns the BalanceEnquiry view with the user's account balances.</returns>
        public ActionResult BalanceEnquiry()
        {
            try
            {
                // Retrieve the userId from the session
                int userId = (int)Session["BankId"];

                // Get the user's account balances using the DAL method
                var balances = _Userdal.GetBalance(userId);

                // Return the BalanceEnquiry view with the user's account balances
                return View(balances);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred in BalanceEnquiry: {ex.Message}");
                // Optionally rethrow the exception if needed
                // throw;

                return RedirectToAction("Error");
            }
        }
        public ActionResult Loan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Loan(decimal? amount, string accountNumber, string ifscCode, string panCardNumber, string password)
        {
            try
            {
                int userId = (int)Session["BankId"];
                string hashPassword = _hashPassword.Hash(password);
                DateTime loanDate;

                // Check if the user has already applied for a loan in the current month
                if (_Userdal.HasAppliedForLoanThisMonth(userId))
                {
                    TempData["ErrorMessage"] = "You have already applied for a loan this month. Please try again next month.";
                    return RedirectToAction("Loan");
                }

                bool isSuccess = _Userdal.ClickLoan(userId, amount.Value, accountNumber, ifscCode, panCardNumber, out loanDate, hashPassword);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Loan successfully approved";

                    // Record the loan application in the database
                    _Userdal.RecordLoanApplication(userId, amount.Value, accountNumber, ifscCode);
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to process the loan. Please try again.";
                }

                if (!amount.HasValue)
                {
                    TempData["ErrorMessage"] = "Amount is required for the loan.";
                    return RedirectToAction("Loan");
                }

                return RedirectToAction("Loan");
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // For now, set an error message and redirect to the Loan view
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Loan");
            }
        }

    }
}



