﻿using System.Data.SqlClient;

namespace BankingSystem;

class BankAccount
{
    public static string ConnectionString = @"Server=(localdb)\BankBD;DataBase=Bank";
    public int AccountNumber { get; set; }
    public decimal Balance { get; set; }
    
    
    public static BankAccount GetAccount(int accountNumber)
    {
        BankAccount bankAccount = new BankAccount();

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var queryIfAccountExisit =
                $"SELECT CASE WHEN EXISTS (SELECT * FROM finanse.bankSystem WHERE accountNumber = '{accountNumber}') THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END;";
            var command = new SqlCommand(queryIfAccountExisit, connection);
            bool ifAccountExisitResult = (bool)command.ExecuteScalar();

            if (ifAccountExisitResult)
            {
                var querySelect = $"SELECT * FROM finanse.bankSystem WHERE accountNumber = {accountNumber}";

                command = new SqlCommand(querySelect, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    bankAccount.AccountNumber = (int)reader["AccountNumber"];
                    bankAccount.Balance = (decimal)reader["Balance"];
                }

                connection.Close();
                return bankAccount;
            }

            // ?????????????????????????????????????????????????????????????????
            new BankAccount() = accountNumber;

        }
        return bankAccount;
    }
    
    
     public void CreateAccount(int accNumber)
     {
         while (true) {
             try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    // If user enters int correctly...
                    if (!(accNumber <= 10 || accNumber >= 100000))
                    {
                        connection.Open();
                        
                        var queryIfAccountExisit =
                            $"SELECT CASE WHEN EXISTS (SELECT * FROM finanse.bankSystem WHERE accountNumber = '{accNumber}') THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END;";
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
                            
                            var queryAccountAdd =
                                $"INSERT INTO finanse.bankSystem (accountNumber) VALUES ({accNumber})";
                            command = new SqlCommand(queryAccountAdd, connection);
                            command.ExecuteNonQuery();

                            Console.WriteLine(
                                $"Success! Your account with a number {accNumber} has been created.\n");
                            break;
                        }
                    }
                    // If user enters an int that is shorter than 2 or longer than 6 characters...
                    else
                    {
                        Console.WriteLine("You entered too short or too long number. Please try again\n");
                    }
                    connection.Close();
                }
            }

            // Error message
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            break;
        }
    }
     
        public void Deposit()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("How much money do you want to deposit?");
                    decimal userDepositMoney = Convert.ToDecimal(Console.ReadLine());
                    var depositResult = Balance += userDepositMoney;
                    Console.WriteLine($"Deposit successful. Your new balance is: {Balance}\n");

                    if (userDepositMoney <= 0)
                    {
                        Console.WriteLine("Cannot deposit less than 0. Please try again");
                        break;
                    }

                    var queryDeposit =
                        $"UPDATE finanse.bankSystem SET balance = {depositResult} WHERE accountNumber = {AccountNumber}";

                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand(queryDeposit, connection);
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine($"Success! Your new balance is now {Balance}\n");
                    break;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }
        
    public void Withdraw()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("How much money do you want to withdraw?");
                decimal userWithdrawMoney = Convert.ToDecimal(Console.ReadLine());

                    // If user enters a value that is higher than its bank account's balance
                    if (userWithdrawMoney > Balance)
                    {
                        Console.WriteLine(
                            $"You entered a value that is higher than balance.\nYour balance is: {Balance}\nPlease enter a lower value.\n");
                    }

                    else
                    {
                        var withdrawResult = Balance -= userWithdrawMoney;
                        var queryWithdraw =
                            $"UPDATE finanse.bankSystem SET balance = {withdrawResult} WHERE accountNumber = {AccountNumber}";

                        using (var connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();
                            var command = new SqlCommand(queryWithdraw, connection);
                            command.ExecuteNonQuery();
                        }
                        Console.WriteLine($"Success! Your new balance is now {Balance}\n");
                        break;
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
    }

    public decimal CheckBalance()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var queryCheckBalance = $"SELECT balance FROM finanse.bankSystem WHERE accountNumber = {AccountNumber}";
                var command = new SqlCommand(queryCheckBalance, connection);

                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        Balance = (decimal)reader["Balance"];
                    }
                    
                }
                    
                Console.WriteLine($"Your balance is: {Balance}\n");
                connection.Close();
            }
        } 
        
        catch (Exception ex) { 
            Console.WriteLine(ex.Message + "\n");
        }

        return Balance;
    } 

}