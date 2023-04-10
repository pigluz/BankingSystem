﻿using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace BankingSystem;

class Bank
{
    int _remainingAttempts;
    private List<BankAccount> _accounts = new();
    
    public void CreateAccount()
    {
        bool validNumber = false;
        
        while (validNumber == false)
        {
            //
            // NO WORKEYYYYYYYYYYYYYYYYYYYYYYYYY....................
            //
            
            Console.WriteLine("Enter a new account number... (2-6 characters long)");
            try
            {
                int accountNumber = Convert.ToInt32(Console.ReadLine());

                // If user enters int correctly...
                if (!(accountNumber < 10 || accountNumber > 100000))
                {
                    var queryIfAccountExisit =
                        $"SELECT CASE WHEN EXISTS (SELECT * FROM finanse.bankSystem WHERE accountNumber = '{accountNumber}') THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END;";
                    SqlCommand command = new SqlCommand(queryIfAccountExisit, Program.connection);
                    int ifAccountExisitResult = (int)command.ExecuteScalar();

                    // Check if an account with this number already exists...
                    if (ifAccountExisitResult == 1)
                    {
                        Console.WriteLine("An account with this number already exists. Please try again.\n");
                    }
                    
                    // If no existing account was found, create a new account...
                    else
                    {
                        BankAccount account = new BankAccount();
                        account.AccountNumber = accountNumber;
                        account.Balance = 0;
                        _accounts.Add(account);

                        Console.WriteLine($"Success! Your account with a number {account.AccountNumber} has been created.\n");
                        validNumber = true;
                    }
                    
                }
                
                // If user enters an int that is shorter than 2 or longer than 6 characters...
                else
                {
                    Console.WriteLine("You entered too short or too long number. Please try again\n");
                    validNumber = false;
                }
            }
            
            // If user enters value other than int...
            catch (Exception)
            {
                Console.WriteLine("You entered invalid type of value. Try again...\n");
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

        Console.WriteLine($"Your balance is: {account.Balance}\n");
        
        
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
                try
                {
                    Console.WriteLine("Enter your account's number...");
                    int userInput = Convert.ToInt32(Console.ReadLine());

                    account = _accounts.FirstOrDefault(a => a.AccountNumber == userInput);

                    
                    // If there is no matching bank account's number...
                    if (account == null)
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
                        validateBankAccount = true;
                        break;
                    }
                }
                
                // If user enter an invalid type of value...
                catch (Exception)
                {
                    Console.WriteLine("You entered invalid type of value. Try again...\n");
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
