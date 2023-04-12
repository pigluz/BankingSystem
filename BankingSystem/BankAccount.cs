using System.Data.SqlClient;

namespace BankingSystem;

class BankAccount
{
    public int AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public Bank bank = new();
    
        public void Deposit() {
            bank.CheckBankAccount(out var accountNumber, out bool validateBankAccount, out BankAccount? account);

        bool validNumber = false;

        if (validateBankAccount)
        {
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
    }

    public void Withdraw()
    {
        bank.CheckBankAccount(out var accountNumber, out bool validateBankAccount, out BankAccount? account);

        bool validNumber = false;

        while (validNumber == false)
        {
            if (validateBankAccount)
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
    }

    public decimal CheckBalance()
    {
        
        // ERROR
        bank.CheckBankAccount(out var accountNumber, out bool validateBankAccount, out BankAccount? account);
        try
        {
            if (validateBankAccount)
            {
                using (var connection = new SqlConnection(Bank.connectionString))
                {
                    connection.Open();

                    // Do debugowania:
                    Console.WriteLine($"Podany numer konta to: {accountNumber}");
                    
                    var queryCheckBalance = $"SELECT balance FROM finanse.bankSystem WHERE accountNumber = {accountNumber}";
                    var command = new SqlCommand(queryCheckBalance, connection);

                    using (var reader = command.ExecuteReader())
                    {
                    
                        while (reader.Read())
                        {
                            // tutaj pewnie jakis błąd odnośnie 
                            //  "Object reference not set to an instance of an object"
                            decimal balance = reader.GetDecimal(0);
                            var decimall = new BankAccount { Balance = balance };
                        }
                    
                    }
                    Console.WriteLine($"Your balance is: {account.Balance}\n");
                    connection.Close();
                }
            }
        } 
        
        catch (Exception ex) { 
            Console.WriteLine(ex.Message);
        }

        return Balance;

    }
}