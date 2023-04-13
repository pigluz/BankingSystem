using System.Data.SqlClient;

namespace BankingSystem;

class BankAccount
{
    public static string ConnectionString = @"Server=(localdb)\BankBD;DataBase=Bank";
    public int AccountNumber { get; set; }
    public decimal Balance { get; set; }
    
    
     public void CreateAccount()
    {
        Console.WriteLine("Enter a new account number... (2-6 characters long)");
        while (true)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
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
                            BankAccount account = new();
                            account.AccountNumber = accountNumber;

                            var queryAccountAdd =
                                $"INSERT INTO finanse.bankSystem (accountNumber) VALUES ({account.AccountNumber})";
                            command = new SqlCommand(queryAccountAdd, connection);
                            command.ExecuteNonQuery();

                            Console.WriteLine(
                                $"Success! Your account with a number {account.AccountNumber} has been created.\n");
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
                break;
            }
        }
    }

    
     public static BankAccount GetAccount(int accountNumber)
     {
         var queryIfAccountExisit = $"SELECT * FROM finanse.bankSystem WHERE accountNumber = {accountNumber}";
         BankAccount bankAccount = new();
         using (var connection = new SqlConnection(ConnectionString))
         {
             connection.Open();
             var command = new SqlCommand(queryIfAccountExisit, connection);
             var reader = command.ExecuteReader();

             while (reader.Read())
             {
                 bankAccount.AccountNumber = (int)reader["accountNumber"];
                 bankAccount.Balance = (decimal)reader["balance"];
             }
             connection.Close();
             Console.WriteLine($"{bankAccount.AccountNumber}, {bankAccount.Balance}");
         }

         return bankAccount;
     }
     
        public void Deposit()
        {
            BankAccount bankAccount = new BankAccount();
            while (true)
            {
                try
                {
                    Console.WriteLine("How much money do you want to deposit?");
                    decimal userDepositMoney = Convert.ToDecimal(Console.ReadLine());
                    var depositResult = bankAccount.Balance += userDepositMoney;
                    Console.WriteLine($"Deposit successful. Your new balance is: {bankAccount.Balance}\n");
                    
                        // FINISH THIS FUNCTION
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("You entered invalid type of value. Try again...\n");
                }
            }
        }
        
    public void Withdraw()
    {
        BankAccount bankAccount = new();
        while (true)
        {
            try
            {
                Console.WriteLine("How much money do you want to withdraw?");
                while (true)
                {
                    decimal userWithdrawMoney = Convert.ToDecimal(Console.ReadLine());

                    // If user enters a value that is higher than its bank account's balance
                    if (userWithdrawMoney > bankAccount.Balance)
                    {
                        Console.WriteLine(
                            $"You entered a value that is higher than balance.\nYour balance is: {bankAccount.Balance}\nPlease enter a lower value.\n");
                    }

                    else
                    {
                        var withdrawResult = bankAccount.Balance -= userWithdrawMoney;
                        Console.WriteLine($"Success! Your new balance is now {bankAccount.Balance}\n");
                        
                        // FINISH THIS FUNCTION
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("You entered invalid type of value. Try again...\n");
            }
        }
    }

    public decimal CheckBalance(int accountNumber)
    {
        BankAccount bankAccount = GetAccount(accountNumber);
        try
        {

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Do debugowania:
                    Console.WriteLine($"Inputted number: {bankAccount.AccountNumber}");
                    
                    var queryCheckBalance = $"SELECT balance FROM finanse.bankSystem WHERE accountNumber = {bankAccount.AccountNumber}";
                    var command = new SqlCommand(queryCheckBalance, connection);

                    using (var reader = command.ExecuteReader())
                    {
                    
                        while (reader.Read())
                        {
                            bankAccount.Balance = (decimal)reader["Balance"];
                        }
                    
                    }
                    
                    Console.WriteLine($"Your balance is: {bankAccount.Balance}\n");
                    connection.Close();
                }
        } 
        
        catch (Exception ex) { 
            Console.WriteLine(ex.Message);
        }

        return bankAccount.Balance;
    } 

}