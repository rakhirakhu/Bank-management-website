using Antlr.Runtime.Misc;
using Bank_Management.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services.Description;

namespace Bank_Management.DAL
{
    public class userdal
    {
        string constring = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ToString();
        /// <summary>
        /// Inserts user registration details into the database.
        /// </summary>
        /// <param name="registerDetail">The user registration details.</param>
        /// <returns>True if the insertion is successful; otherwise, false.</returns>
        public bool insertuser(registeruser registerdetail)
        {

                //throw new Exception("testing");
                using (SqlConnection connection = new SqlConnection(constring))
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("sp_registerr", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", registerdetail.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", registerdetail.LastName);
                    cmd.Parameters.AddWithValue("@DOB", registerdetail.DOB);
                    cmd.Parameters.AddWithValue("@Gender", registerdetail.Gender);
                    cmd.Parameters.AddWithValue("@Phone", registerdetail.Phone);
                    cmd.Parameters.AddWithValue("@Email", registerdetail.Email);
                    cmd.Parameters.AddWithValue("@Address", registerdetail.Address);
                    cmd.Parameters.AddWithValue("@State", registerdetail.State);
                    cmd.Parameters.AddWithValue("@City", registerdetail.City);
                    cmd.Parameters.AddWithValue("@Username", registerdetail.Username);
                    cmd.Parameters.AddWithValue("@Password", registerdetail.Password);
                    if (registerdetail.ImageFile != null && registerdetail.ImageFile.ContentLength > 0)
                    {
                        byte[] imageData;
                        using (var binaryReader = new System.IO.BinaryReader(registerdetail.ImageFile.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(registerdetail.ImageFile.ContentLength);
                        }

                        cmd.Parameters.AddWithValue("@ProfileImage", imageData);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ProfileImage", DBNull.Value);
                    }

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// Checks if an email is already used by querying the database.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>
        ///     <c>true</c> if the email is already used; otherwise, <c>false</c>.
        ///     In case of an error, returns <c>false</c> and logs the error details.
        /// </returns>
        public bool IsEmailAlreadyUsed(string email)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_CheckEmailExist", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);

                        int count = (int)(cmd.ExecuteScalar() ?? 0); // Use null coalescing to handle null result
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it accordingly
                    // For simplicity, we're rethrowing the exception here
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a phone number is already used by querying the database.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <returns>
        ///     <c>true</c> if the phone number is already used; otherwise, <c>false</c>.
        ///     In case of an error, returns <c>false</c> and logs the error details.
        /// </returns>
        public bool IsPhoneNumberAlreadyUsed(string phoneNumber)
        {

            using (SqlConnection connection = new SqlConnection(constring))
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("sp_CheckPhoneNumberExist", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Phone", phoneNumber);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;

                }
                finally
                {
                    // Code in the finally block will always execute, whether an exception occurred or not
                    // Close the connection in the finally block to ensure it is closed even if an exception occurred
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
        }

            //}
            /// <summary>
            /// Validates login credentials and retrieves the user type.
            /// </summary>
            /// <param name="username">The username for login.</param>
            /// <param name="password">The password for login.</param>
            /// <param name="usertype">Output parameter to store the user type.</param>
            /// <returns>The user type associated with the login credentials.</returns>
            public bool InsertEnquiry(EnquiryModel enquiry)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SP_EnquiryUser", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", enquiry.Name);
                    cmd.Parameters.AddWithValue("@Email", enquiry.Email);
                    cmd.Parameters.AddWithValue("@Comment", enquiry.Comment);

                    try
                    {
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                    finally
                    {

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates login credentials and retrieves the user type.
        /// </summary>
        /// <param name="username">The username for login.</param>
        /// <param name="password">The password for login.</param>
        /// <param name="usertype">Output parameter to store the user type.</param>
        /// <returns>The user type associated with the login credentials.</returns>
        public string validateLogin(string Username, string Password, out string usertype)
        {
            usertype = "";

            using (SqlConnection connection = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("sp_login", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.Add("@usertype", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    usertype = cmd.Parameters["@usertype"].Value.ToString();
                }
            }

            return usertype;
        }
        /// <summary>
        /// Retrieves the user ID associated with the specified username.
        /// </summary>
        /// <param name="username">The username for which to retrieve the user ID.</param>
        /// <returns>The user ID associated with the username.</returns>
        public int GetUserIDByUsername(string Username)
        {
            // Initialize BankId to 0

            int BankId = 0;
            try
            {

                using (SqlConnection connection = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetUserIDByUsername", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", Username);
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            BankId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetUserIDByUsername: {ex.Message}");
            }

            return BankId;
        }




        /// <summary>
        /// Retrieves a list of bank accounts associated with the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom to retrieve bank accounts.</param>
        /// <returns>A list of BankAccount objects associated with the user.</returns>
        public List<BankAccount> GetUserBankAccounts(int userId)
        {
            List<BankAccount> accounts = new List<BankAccount>();

            try
            {

                using (SqlConnection connection = new SqlConnection(constring))
                {

                    using (SqlCommand cmd = new SqlCommand("sp_getbankaccountbyuserid", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                BankAccount account = new BankAccount
                                {
                                    AccountId = Convert.ToInt32(reader["AccountId"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    AccountHolderName = reader["AccountHolderName"].ToString(),
                                    BranchName = reader["BranchName"].ToString(),
                                    AccountNumber = reader["AccountNumber"].ToString(),
                                    IFSCCode = reader["IFSCCode"].ToString(),
                                    OpenDate = (DateTime)reader["OpenDate"],
                                    Balance = Convert.ToDecimal(reader["Balance"]),
                                };

                                accounts.Add(account);
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetUserBankAccounts: {ex.Message}");
            }

            return accounts;
        }







        /// <summary>
        /// Deposits the specified amount into the user's account.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="amount">The amount to deposit.</param>
        /// <param name="accountNumber">The user's account number.</param>
        /// <param name="ifscCode">The IFSC code of the bank.</param>
        /// <returns>True if the deposit is successful, false otherwise.</returns>
        public bool Deposituser(int userId, decimal amount, string accountNumber, string ifscCode ,string password)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_deposituser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@IFSCCode", ifscCode);
                    command.Parameters.AddWithValue("@Password", password);

                    
                    try
                    {
                        InsertTransaction(userId, "Deposit", amount, accountNumber, ifscCode);

                        int returnstatus = 0;
                        returnstatus= command.ExecuteNonQuery();
                        if(returnstatus>0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                        // After successful deposit, insert a transaction record

                      
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"An error occurred in Deposituser: {ex.Message}");

                        // Handle the exception (log, throw, etc.)
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Withdraws the specified amount from the user's account.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="amount">The amount to withdraw.</param>
        /// <param name="accountNumber">The user's account number.</param>
        /// <param name="ifscCode">The IFSC code of the bank.</param>
        /// <returns>True if the withdrawal is successful, false otherwise.</returns>
        public bool Withdrawal(int userId, decimal amount, string accountNumber, string ifscCode ,string password)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_withdrawuser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@IFSCCode", ifscCode);
                    command.Parameters.AddWithValue("@Password", password);


                    try
                    {
                        int status = command.ExecuteNonQuery();

                        InsertTransaction(userId, "Withdrawal", amount, accountNumber, ifscCode);
                        if (status > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                         // Withdrawal successful
                    }

                    catch (SqlException ex)
                    {
                        // Log or handle the exception as needed
                        Console.WriteLine($"Error: {ex.Message}");
                        return false; // Withdrawal failed
                    }
                }

            }
        }
        /// <summary>
        /// Transfers the specified amount from one user's account to another.
        /// </summary>
        /// <param name="userId">The ID of the user initiating the transfer.</param>
        /// <param name="toUserId">The ID of the user to whom the transfer is made.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <param name="accountNumber">The account number of the user initiating the transfer.</param>
        /// <param name="ifscCode">The IFSC code of the bank of the user initiating the transfer.</param>
        /// <param name="toAccountNumber">The account number of the user to whom the transfer is made.</param>
        /// <param name="toIFSCCode">The IFSC code of the bank of the user to whom the transfer is made.</param>
        /// <returns>True if the transfer is successful, false otherwise.</returns>
        public bool UserTransfer(int UserId, int toUserId, decimal amount, string AccountNumber, string IFSCCode, string toAccountNumber, string toIFSCCode ,string password)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_transferuser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@ToUserId", toUserId);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                    command.Parameters.AddWithValue("@IFSCCode", IFSCCode);
                    command.Parameters.AddWithValue("@ToAccountNumber", toAccountNumber);
                    command.Parameters.AddWithValue("@ToIFSCCode", toIFSCCode);
                    command.Parameters.AddWithValue("@Password", password);


                    try
                    {
                        // Execute the transfer stored procedure
                        int status = command.ExecuteNonQuery();
                        InsertTransaction(UserId, "Transfer", amount, AccountNumber, IFSCCode);
                        InsertTransaction(toUserId, "Credited", amount, toAccountNumber, toIFSCCode);
                        if (status > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Handle the exception (log, throw, etc.)
                        // For now, let's print the error to the console
                        foreach (SqlError error in ex.Errors)
                        {
                            Console.WriteLine($"Error {error.Number}: {error.Message}");
                        }

                        // Transfer failed
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a list of transactions for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom transactions are retrieved.</param>
        /// <returns>A list of transactions for the specified user.</returns>
        public List<Transaction> GetTransactionsByUserId(int userId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_gettransactionsbyuserid", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    TransactionId = (int)reader["TransactionId"],
                                    UserId = (int)reader["UserId"],
                                    Type = reader["Type"].ToString(),
                                    Amount = (decimal)reader["Amount"],
                                    Date = (DateTime)reader["Date"],
                                    AccountNumber = reader["AccountNumber"].ToString(),
                                    IFSCCode = reader["IFSCCode"].ToString(),
                                };

                                transactions.Add(transaction);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Log or handle the exception as needed
                        Console.WriteLine($"Error in GetTransactionsByUserId: {ex.Message}");
                        // You may choose to throw the exception further or handle it gracefully
                    }
                }
            }

            // Return the list of transactions
            return transactions;
        }
        /// <summary>
        /// Inserts a transaction record into the database.
        /// </summary>
        /// <param name="userId">The ID of the user involved in the transaction.</param>
        /// <param name="transactionType">The type of transaction (e.g., Deposit, Withdrawal).</param>
        /// <param name="amount">The amount involved in the transaction.</param>
        /// <param name="accountNumber">The account number associated with the transaction.</param>
        /// <param name="ifscCode">The IFSC code associated with the transaction.</param>
        private void InsertTransaction(int userId, string transactionType, decimal amount, string accountNumber, string ifscCode)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_logtransaction", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@TransactionId", transactionId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Type", transactionType);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@IFSCCode", ifscCode);
                    command.Parameters.AddWithValue("@Date", DateTime.Now);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Log the exception details
                        foreach (SqlError error in ex.Errors)
                        {
                            Console.WriteLine($"Error {error.Number}: {error.Message}");
                        }



                    }
                }
            }
        }


        /// <summary>
        /// Withdraws a specified amount from the user's bank account.
        /// </summary>
        /// <param name="userId">The ID of the user initiating the withdrawal.</param>
        /// <param name="amount">The amount to be withdrawn.</param>
        /// <param name="accountNumber">The account number from which the withdrawal is made.</param>
        /// <param name="ifscCode">The IFSC code associated with the bank account.</param>
        /// <returns>True if the withdrawal is successful, false otherwise.</returns>
        public bool Withdraw(int userId, decimal amount, string accountNumber, string ifscCode)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("sp_withdrawuser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    cmd.Parameters.AddWithValue("@IFSCCode", ifscCode);

                    connection.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        foreach (SqlError error in ex.Errors)
                        {
                            Console.WriteLine($"Error {error.Number}: {error.Message}");
                        }
                        return false;


                    }
                }
            }
        }


        /// <summary>
        /// Transfers funds from the user's account to another user's account.
        /// </summary>
        /// <param name="transferDetails">Details of the fund transfer, including user IDs, amounts, and account information.</param>
        /// <returns>True if the fund transfer is successful, false otherwise.</returns>
        public bool TransferFunds(TransferAmount transferDetails)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_transferfund", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", transferDetails.UserId);
                        cmd.Parameters.AddWithValue("@ToUserId", transferDetails.ToUserId);
                        cmd.Parameters.AddWithValue("@Amount", transferDetails.Amount);
                        cmd.Parameters.AddWithValue("@AccountNumber", transferDetails.AccountNumber);
                        cmd.Parameters.AddWithValue("IFSCCode", transferDetails.IFSCCode);
                        cmd.Parameters.AddWithValue("@ToAccountNumber", transferDetails.ToAccountNumber);
                        cmd.Parameters.AddWithValue("@ToIFSCCode", transferDetails.ToIFSCCode);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true; // Transfer successful
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false; // Transfer failed
            }

        }


        /// <summary>
        /// Retrieves user details by username from the database.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A RegisterUser object containing user details if found, otherwise an object with default values.</returns>
        public registeruser GetUserByUsername(string username)
        {
            registeruser user = new registeruser();
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetUserByUsernameeee", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);

                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user.BankId = Convert.ToInt32(reader["BankId"]);
                                user.FirstName = reader["FirstName"].ToString();
                                user.LastName = reader["LastName"].ToString();
                                user.DOB = Convert.ToDateTime(reader["DOB"]);
                                user.Gender = reader["Gender"].ToString();
                                user.Phone = reader["Phone"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.Address = reader["Address"].ToString();
                                user.State = reader["State"].ToString();
                                user.City = reader["City"].ToString();
                                user.Username = reader["Username"].ToString();
                                user.Password = reader["Password"].ToString();
                                user.ProfileImage = reader["ProfileImage"] != DBNull.Value ? (byte[])reader["ProfileImage"] : null;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetUserByUsername: {ex.Message}");
            }

            // Return the user details
            return user;
        }
        /// <summary>
        /// Updates user details in the database based on the username.
        /// </summary>
        /// <param name="updatedUser">An instance of RegisterUser containing updated user details.</param>
        /// <returns>True if the update is successful, otherwise false.</returns>
        public bool UpdateUserByUsername(registeruser updatedUser)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateUserByUsername", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", updatedUser.Username);
                        cmd.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", updatedUser.LastName);
                        cmd.Parameters.AddWithValue("@DOB", updatedUser.DOB);
                        cmd.Parameters.AddWithValue("@Gender", updatedUser.Gender);
                        cmd.Parameters.AddWithValue("@Phone", updatedUser.Phone);
                        cmd.Parameters.AddWithValue("@Email", updatedUser.Email);
                        cmd.Parameters.AddWithValue("@Address", updatedUser.Address);
                        cmd.Parameters.AddWithValue("@State", updatedUser.State);
                        cmd.Parameters.AddWithValue("@City", updatedUser.City);
                        //cmd.Parameters.AddWithValue("@Password", updatedUser.Password);
                        if (updatedUser.ImageFile != null && updatedUser.ImageFile.ContentLength > 0)
                        {
                            cmd.Parameters.AddWithValue("@ProfileImage", ConvertImageToByteArray(updatedUser.ImageFile));
                        }
                        else
                        {
                            // If no new image, use the existing ProfileImage
                            cmd.Parameters.AddWithValue("@ProfileImage", updatedUser.ProfileImage ?? new byte[0]);
                        }

                        connection.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred in UpdateUserByUsername: {ex.Message}");
            }

            // Return true if at least one row is affected, indicating a successful update
            return rowsAffected > 0;
        }






        // Convert HttpPostedFileBase to byte array
        private byte[] ConvertImageToByteArray(HttpPostedFileBase image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.InputStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Retrieves balance information for the specified user from the database.
        /// </summary>
        /// <param name="userId">The user ID for which to retrieve balance information.</param>
        /// <returns>A list of BalanceEnquiryModel objects representing account balances.</returns>
        public List<BalanceEnquiryModel> GetBalance(int userId)
        {
            List<BalanceEnquiryModel> balances = new List<BalanceEnquiryModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_balanceenquiry", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BalanceEnquiryModel account = new BalanceEnquiryModel
                                {
                                    AccountNumber = reader["AccountNumber"].ToString(),
                                    IFSCCode = reader["IFSCCode"].ToString(),
                                    Balance = Convert.ToDecimal(reader["Balance"])
                                };

                                balances.Add(account); // Add the account to the list
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred in GetBalance: {ex.Message}");
            }

            // Return the list of balances
            return balances;
        }

        /// <summary>
        /// Changes the password for the specified user in the database.
        /// </summary>
        /// <param name="username">The username of the user whose password needs to be changed.</param>
        /// <param name="oldPassword">The old password for verification.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>True if the password is successfully changed, false otherwise.</returns>
        public bool ChangeUserPassword(string username, string oldPassword, string newPassword)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ChangeAdminPassword", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@OldPassword", oldPassword);
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                    connection.Open();
                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Handle SQL exceptions
                        throw new Exception("Error changing password: " + ex.Message);
                    }
                }
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Retrieves the user ID associated with a given account number from the database.
        /// </summary>
        /// <param name="ToAccountNumber">The account number for which to retrieve the user ID.</param>
        /// <returns>
        ///     The user ID associated with the provided account number.
        ///     If the account number is not found or an error occurs, returns 0.
        /// </returns>
        public int GetUserIdByAccountNumber(string ToAccountNumber)
        {
            int UserId = 0; // Initialize with a default value

            using (SqlConnection connection = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetUserIdByAccountNumber", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ToAccountNumber", ToAccountNumber);

                    connection.Open();

                    // Handle the result returned by ExecuteScalar
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        UserId = Convert.ToInt32(result);
                    }
                }
            }

            return UserId;
        }

        public bool ClickLoan(int userId, decimal amount, string accountNumber, string ifscCode, string panCardNumber, out DateTime loanDate, string password)
        {
            loanDate = DateTime.MinValue;

            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_clickloan", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@IFSCCode", ifscCode);
                        command.Parameters.AddWithValue("@PANCardNumber", panCardNumber);
                        command.Parameters.Add("@LoanDate", SqlDbType.Date).Direction = ParameterDirection.Output;
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the stored procedure
                        int returnStatus = command.ExecuteNonQuery();

                        if (returnStatus > 0)
                        {
                            // Check if the output parameter is DBNull before converting
                            object loanDateValue = command.Parameters["@LoanDate"].Value;
                            if (loanDateValue != DBNull.Value)
                            {
                                loanDate = Convert.ToDateTime(loanDateValue);
                            }

                            return true;
                        }
                        else
                        {
                            // Handle the case where the stored procedure did not execute successfully
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or rethrow the exception with more details
                Console.WriteLine($"An error occurred in ClickLoan: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the controller
            }
        }

        public bool HasAppliedForLoanThisMonth(int userId)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                // Assuming your 'Transaction' table has a 'Type' column indicating the transaction type
                // Adjust the table and column names based on your actual database schema
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [Transaction] WHERE UserId = @UserId AND Type = 'Loan' AND MONTH(Date) = MONTH(GETDATE()) AND YEAR(Date) = YEAR(GETDATE())", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
        public void RecordLoanApplication(int userId, decimal amount, string accountNumber, string ifscCode)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO [dbo].[Transaction] (UserId, Type, Amount, Date, AccountNumber, IFSCCode) VALUES (@UserId, 'LoanApplication', @Amount, GETDATE(), @AccountNumber, @IFSCCode)", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@IFSCCode", ifscCode);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}




        
          
       















