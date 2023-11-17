using Bank_Management.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Bank_Management.DAL
{
    public class adminDAL
    {
        string constring = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ToString();
        /// <summary>
        /// Inserts a new user into the system using the provided registration details.
        /// </summary>
        /// <param name="registerdetail">The registration details of the user.</param>
        /// <returns>Returns true if the user is successfully inserted; otherwise, returns false.</returns>
        public bool insertuser(registeruser registerdetail)
        {
            int rowsAffected;


            using (SqlConnection connection = new SqlConnection(constring))
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_adminregister", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
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

                    connection.Open();
                    rowsAffected = cmd.ExecuteNonQuery();

                    connection.Close();


                    // Check the number of rows affected to determine the success of the operation
                    return rowsAffected > 0;
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
        /// Retrieves a list of users from the system.
        /// </summary>
        /// <returns>A list of registeruser objects representing users.</returns>
        public List<registeruser> GetUsers()
        {
            List<registeruser> users = new List<registeruser>();


            using (SqlConnection connection = new SqlConnection(constring))
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GetUsersAndAccountNumbers", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                registeruser user = new registeruser
                                {
                                    BankId = Convert.ToInt32(reader["BankId"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DOB = reader["DOB"] != DBNull.Value ? (DateTime)reader["DOB"] : DateTime.MinValue,
                                    Gender = reader["Gender"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    State = reader["State"].ToString(),
                                    City = reader["City"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    ProfileImage = reader["ProfileImage"] != DBNull.Value ? (byte[])reader["ProfileImage"] : null,
                                    AccountNumber = reader["AccountNumber"].ToString()

                                    // ... Map other properties
                                };

                                users.Add(user);
                            }
                        }
                    }
                }
                finally
                {

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            return users;
        }
        /// <summary>
        /// Retrieves a list of enquiry details from the system.
        /// </summary>
        /// <returns>A list of EnquiryModel objects representing enquiry details.</returns>
        public List<EnquiryModel> EnquiryDetails()
        {
            List<EnquiryModel> users = new List<EnquiryModel>();

            using (SqlConnection connection = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EnquiryDetails", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnquiryModel user = new EnquiryModel
                            {
                                EnquiryId = Convert.ToInt32(reader["EnquiryId"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Comment = reader["Comment"].ToString(),

                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
        /// <summary>
        /// Creates a new bank account for a user in the system.
        /// </summary>
        /// <param name="account">The BankAccount object containing account details.</param>
        /// <returns>True if the bank account creation is successful; otherwise, false.</returns>
        //////////////////////////public bool CreateBankAccount(BankAccount account)
        //////////////////////////{
        //////////////////////////    try
        //////////////////////////    {
        //////////////////////////        using (SqlConnection connection = new SqlConnection(constring))
        //////////////////////////        {
        //////////////////////////            using (SqlCommand cmd = new SqlCommand("sp_createbankaccount", connection))
        //////////////////////////            {
        //////////////////////////                cmd.CommandType = CommandType.StoredProcedure;
        //////////////////////////                cmd.Parameters.AddWithValue("@UserId", account.UserId);
        //////////////////////////                cmd.Parameters.AddWithValue("@AccountHolderName", account.AccountHolderName);
        //////////////////////////                cmd.Parameters.AddWithValue("@BranchName", account.BranchName);
        //////////////////////////                cmd.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
        //////////////////////////                cmd.Parameters.AddWithValue("@IFSCCode", account.IFSCCode);
        //////////////////////////                cmd.Parameters.AddWithValue("@OpenDate", account.OpenDate);
        //////////////////////////                cmd.Parameters.AddWithValue("@Balance", account.Balance);


        //////////////////////////                connection.Open();
        //////////////////////////                int rowsAffected = cmd.ExecuteNonQuery();

        //////////////////////////                return rowsAffected > 0;
        //////////////////////////            }
        //////////////////////////        }
        //////////////////////////    }
        //////////////////////////    catch (Exception ex)
        //////////////////////////    {
        //////////////////////////        // Log or handle the exception as needed
        //////////////////////////        Console.WriteLine($"An error occurred in CreateBankAccount: {ex.Message}");
        //////////////////////////        // Optionally rethrow the exception if needed
        //////////////////////////        // throw;
        //////////////////////////        return false;
        //////////////////////////    }
        //////////////////////////}



        public bool CreateBankAccountForUser(BankAccount bankAccount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("SpCreateBankAccountss", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@UserId", bankAccount.UserId);
                        cmd.Parameters.AddWithValue("@BranchName", bankAccount.BranchName);
                        cmd.Parameters.AddWithValue("@AccountNumber", bankAccount.AccountNumber);
                        cmd.Parameters.AddWithValue("@IFSCCode", bankAccount.IFSCCode);
                        cmd.Parameters.AddWithValue("@OpenDate", bankAccount.OpenDate);
                        cmd.Parameters.AddWithValue("@Balance", bankAccount.Balance);

                        connection.Open();
                        int result = cmd.ExecuteNonQuery();

                        // Check the result to determine if the bank account was successfully created
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred in CreateBankAccountForUser: {ex.Message}");
                // Optionally rethrow the exception if needed
                // throw;
                return false;
            }
        }

        public registeruser GetUserById(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getuserbyid", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new registeruser
                                {
                                    BankId = Convert.ToInt32(reader["BankId"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    // Include other properties as needed
                                };
                            }
                        }
                    }
                }

                return null; // Return null if user not found
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred in GetUserById: {ex.Message}");
                // Optionally rethrow the exception if needed
                throw;
            }
        }


        public bool IsAccountnumberused(string accountnumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("sp_CheckAccountNumberExists", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AccountNumber", accountnumber);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in IsEmailAlreadyUsed: {ex.Message}");
                Logger.Error("Error happened", ex);
                return false; // Handle the exception according to your needs
            }
        }


        /// <summary>
        /// Retrieves a list of bank accounts from the database.
        /// </summary>
        /// <returns>List of BankAccount objects representing user bank accounts.</returns>
        public List<BankAccount> GetAccounts()
        {
            List<BankAccount> users = new List<BankAccount>();
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getbankbyid", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BankAccount user = new BankAccount
                                {

                                    AccountId = Convert.ToInt32(reader["AccountId"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    AccountHolderName = reader["AccountHolderName"].ToString(),
                                    BranchName = reader["BranchName"].ToString(),
                                    AccountNumber = reader["AccountNumber"].ToString(),
                                    IFSCCode = reader["IFSCCode"].ToString(),
                                    OpenDate = (DateTime)reader["OpenDate"],
                                    Balance = Convert.ToDecimal(reader["Balance"]),




                                    // ... Map other properties
                                };

                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred in GetAccounts: {ex.Message}");
                // Optionally rethrow the exception if needed
                // throw;
            }

            return users;
        }
        /// <summary>
        /// Retrieves a list of transactions for a user from the database.
        /// </summary>
        /// <returns>List of Transaction objects representing user transactions.</returns>
        public List<Transaction> getusertransaction()
        {
            List<Transaction> transactions = new List<Transaction>();
            try
            {
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_getusertransaction", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;



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
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred in GetUserTransactions: {ex.Message}");
                // Optionally rethrow the exception if needed
                // throw;
            }

            return transactions;
        }
        /// <summary>
        /// Inserts a transaction log into the database for a specific user.
        /// </summary>
        /// <param name="userId">User ID associated with the transaction.</param>
        /// <param name="transactionType">Type of the transaction (e.g., Withdrawal, Deposit).</param>
        /// <param name="amount">Transaction amount.</param>
        /// <param name="accountNumber">Account number involved in the transaction.</param>
        /// <param name="ifscCode">IFSC code associated with the account.</param>
        private void InsertTransaction(int userId, string transactionType, decimal amount, string accountNumber, string ifscCode)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_logtransaction", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
        /// Retrieves the balance details of all users from the database.
        /// </summary>
        /// <returns>A list of <see cref="BalanceEnquiryModel"/> containing user balance information.</returns>
        public List<BalanceEnquiryModel> GetAllUsersBalance()
        {
            List<BalanceEnquiryModel> userBalances = new List<BalanceEnquiryModel>();

            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_adminbalanceenquiry", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BalanceEnquiryModel account = new BalanceEnquiryModel
                                {
                                    UserId = (int)reader["UserId"],
                                    AccountNumber = reader["AccountNumber"].ToString(),
                                    IFSCCode = reader["IFSCCode"].ToString(),
                                    Balance = (decimal)reader["Balance"]
                                };

                                userBalances.Add(account);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Log the exception details
                        foreach (SqlError error in ex.Errors)
                        {
                            Console.WriteLine($"Error {error.Number}: {error.Message}");
                        }
                        // Optionally handle the exception or rethrow it
                    }
                }
            }

            return userBalances;
        }
        /// <summary>
        /// Changes the password for an admin user.
        /// </summary>
        /// <param name="username">The username of the admin user.</param>
        /// <param name="oldPassword">The old password of the admin user.</param>
        /// <param name="newPassword">The new password to set for the admin user.</param>
        /// <returns>True if the password change is successful, otherwise false.</returns>
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
        /// Retrieves the admin ID based on the provided username.
        /// </summary>
        /// <param name="Username">The username of the admin.</param>
        /// <returns>The admin ID if found, otherwise 0.</returns>
        public int GetAdminIDByUsername(string Username)
        {
            int BankId = 0;
            using (SqlConnection connection = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAdminByUserName", connection))
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
            return BankId;
        }

        /// <summary>
        /// Changes the password for the specified user in the database.
        /// </summary>
        /// <param name="username">The username of the user whose password needs to be changed.</param>
        /// <param name="oldPassword">The old password for verification.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>True if the password is successfully changed, false otherwise.</returns>
        public bool ChangeAdminPassword(string username, string oldPassword, string newPassword)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ChangeUserPassword", connection))
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
        /// Deletes a user from the database based on the provided bank ID.
        /// </summary>
        /// <param name="bankId">The bank ID of the user to be deleted.</param>
        /// <returns>
        ///     <c>true</c> if the user is successfully deleted; otherwise, <c>false</c>.
        ///     Throws an exception in case of an error.
        /// </returns>
        public bool DeleteUser(int bankId)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteUser", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BankId", bankId);

                        int result = cmd.ExecuteNonQuery();
                        return result < 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred in DeleteUser: {ex.Message}");
                    //Logger.Error("error occoured", ex);
                    throw;
                }
            }
        }

    }


}



