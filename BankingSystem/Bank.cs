﻿using System.Data.SqlClient;

namespace BankingSystem;
class Bank
{

    public static string connectionString = @"Server=(localdb)\BankBD;DataBase=Bank";
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
    
    public void Deposit()
    {
        CheckBankAccount(out var account);

        bool validNumber = false;

        while (validNumber == false)
        {
            try
            {
                Console.WriteLine("How much money do you want to deposit?");
                decimal userDepositMoney = Convert.ToDecimal(Console.ReadLine());
                account.Balance += userDepositMoney;
                Console.WriteLine($"Deposit successful. Your new balance is: {account.Balance}\n");
                break;
            }
            catch (Exception)
            {
                Console.WriteLine("You entered invalid type of value. Try again...\n");
                validNumber = false;
            }
        }


    }

    public void Withdraw()
    {
        CheckBankAccount(out var account);

        bool validNumber = false;

        while (validNumber == false)
        {
            try
            {
                Console.WriteLine("How much money do you want to withdraw?");

                while (true)
                {
                    validNumber = true;
                    decimal userWithdrawMoney = Convert.ToDecimal(Console.ReadLine());

                    // If user enters a value that is higher than its bank account's balance
                    if (userWithdrawMoney > account.Balance)
                    {
                        Console.WriteLine(
                            $"You entered a value that is higher than balance.\nYour balance is: {account.Balance}\nPlease enter a lower value.\n");
                    }

                    else
                    {
                        account.Balance -= userWithdrawMoney;
                        Console.WriteLine($"Success! Your new balance is now {account.Balance}\n");
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("You entered invalid type of value. Try again...\n");
                validNumber = false;
            }
        }
    }

    public void CheckBalance()
    {
        CheckBankAccount(out var account);
        try
        {
            Console.WriteLine("Enter your account number to check balance!");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Zrobic tak, aby pobieralo userInput z metody "CheckBankAccount" i na tej podstawie sprawdzać saldo.
                int userInput = Convert.ToInt32(Console.ReadLine());

                var queryCheckBalance = $"SELECT balance FROM finanse.bankSystem WHERE accountNumber = {userInput}";
                var command = new SqlCommand(queryCheckBalance, connection);

                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        decimal balance = reader.GetDecimal(0);
                        var decimall = new BankAccount { Balance = balance };
                    }
                    
                }
                Console.WriteLine($"Your balance is: {account.Balance}\n");
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void CheckBankAccount(out BankAccount? account)
    {
        _remainingAttempts = 5;
        account = null;
        bool validateBankAccount = false;

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
    }
};
