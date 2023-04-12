using System.Data.SqlClient;

namespace BankingSystem;
class Bank
{

    public static string connectionString = @"Server=(localdb)\BankBD;DataBase=Bank";
    private List<BankAccount> _account = new();
    int _remainingAttempts;
    
    public void CreateAccount()
    {
        bool validNumber = false;
        
        while (validNumber == false)
        {
            Console.WriteLine("Enter a new account number... (2-6 characters long)");
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // User enters a value...
                    int accountNumber = Convert.ToInt32(Console.ReadLine());

                    // If user enters int correctly...
                    if (!(accountNumber < 10 || accountNumber > 100000))
                    {
                        var queryIfAccountExisit =
                            $"SELECT CASE WHEN EXISTS (SELECT * FROM finanse.bankSystem WHERE accountNumber = '{accountNumber}') THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END;";
                        var command = new SqlCommand(queryIfAccountExisit, connection);

                        bool ifAccountExisitResult = (bool)command.ExecuteScalar();


                        // Check if an account with this number already exists...
                        if (ifAccountExisitResult)
                        {
                            Console.WriteLine("An account with this number already exists. Please try again.\n");
                        }
                        // If no existing account was found, create a new account...
                        else
                        {
                            BankAccount account = new BankAccount();
                            account.AccountNumber = accountNumber;

                            var queryAccountAdd =
                                $"INSERT INTO finanse.bankSystem (accountNumber) VALUES ({account.AccountNumber})";
                            command = new SqlCommand(queryAccountAdd, connection);
                            command.ExecuteNonQuery();

                            Console.WriteLine(
                                $"Success! Your account with a number {account.AccountNumber} has been created.\n");
                            validNumber = true;
                        }
                    }
                    // If user enters an int that is shorter than 2 or longer than 6 characters...
                    else
                    {
                        Console.WriteLine("You entered too short or too long number. Please try again\n");
                        validNumber = false;
                    }
                    connection.Close();
                }
            }

            // Error message
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                validNumber = false;
                break;
            }
        }
    }
    
    public bool CheckBankAccount(out BankAccount? account, out bool validateBankAccount)
    {
        _remainingAttempts = 5;
        account = null;
        validateBankAccount = false;

        while (validateBankAccount == false)
        {
            while (_remainingAttempts > 0)
            {
                Console.WriteLine("Enter your account's number...");
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        // User enters a value...
                        int userInput = Convert.ToInt32(Console.ReadLine());

                        var queryIfAccountExisit = $"SELECT CASE WHEN EXISTS (SELECT * FROM finanse.bankSystem WHERE accountNumber = '{userInput}') THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END;";
                        var command = new SqlCommand(queryIfAccountExisit, connection);
                        bool ifAccountExisitResult = (bool)command.ExecuteScalar();

                        // If there is no matching bank account's number...
                        if (ifAccountExisitResult == false)
                        {
                            Console.WriteLine("Wrong number... Please try again.\n");
                            _remainingAttempts--;

                            // If user fails to enter valid accont number...
                            if (_remainingAttempts == 0)
                            {
                                Console.WriteLine("You failed to enter account number.\nExiting Program...\n");
                                Environment.Exit(0);
                            }
                            else
                            {
                                break;
                            }
                        }
                        // If there is a match...
                        else
                        {
                            Console.WriteLine("Success!");
                            validateBankAccount = true;
                            break;
                        }
                        connection.Close();
                    }
                }

                // If user enter an invalid type of value...
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    validateBankAccount = false;
                    _remainingAttempts--;

                    // If user fails to enter valid account number../
                    if (_remainingAttempts == 0)
                    {
                        Console.WriteLine("You failed to enter account number.\nExiting Program...\n");
                        Environment.Exit(0);
                    }
                }
            }
        }
        
        return true;
    }
};
