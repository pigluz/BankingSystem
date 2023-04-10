using System.Dynamic;
class BankAccount
{
    public int AccountNumber { get; set; }
    public decimal Balance { get; set; }
}

class Bank
{
    int remainingAttempts;
    private List<BankAccount> accounts = new();

    public void CreateAccount()
    {
        Console.WriteLine("Enter a new account number...");
        int accountNumber = Convert.ToInt32(Console.ReadLine());

        // Check if an account with this number already exists
        if (accounts.Any(a => a.AccountNumber == accountNumber))
        {
            Console.WriteLine("An account with this number already exists. Please try again.");
            return;
        }

        // If no existing account was found, create a new account
        BankAccount account = new BankAccount();
        account.AccountNumber = accountNumber;
        account.Balance = 0;
        accounts.Add(account);

        Console.WriteLine($"Success! Your account with a number {account.AccountNumber} has been created.\n");
    }

    public void Deposit()
    {
        CheckBankAccount(out var account);

        Console.WriteLine("How much money do you want to deposit?");
        decimal userDepositMoney = Convert.ToDecimal(Console.ReadLine());
        account.Balance += userDepositMoney;
        Console.WriteLine($"Deposit successful. Your new balance is: {account.Balance}\n");
    }

    public void Withdraw()
    {
        CheckBankAccount(out var account);

        Console.WriteLine("How much money do you want to withdraw?");

        while (true)
        {
            decimal userWithdrawMoney = Convert.ToDecimal(Console.ReadLine());
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
    
    public void CheckBalance()
    {
        CheckBankAccount(out var account);

        Console.WriteLine($"Your balance is: {account.Balance}\n");
        
        
    }

  
    public void CheckBankAccount(out BankAccount? account)
    {
        remainingAttempts = 5;
        Console.WriteLine("Enter your account's number...");
        account = null;

        while (remainingAttempts > 0)
        {
            int userInput = Convert.ToInt32(Console.ReadLine());

            account = accounts.FirstOrDefault(a => a.AccountNumber == userInput);

            if (account == null)
            {
                Console.WriteLine("Wrong number... Please try again.");
                remainingAttempts--;
                if (remainingAttempts == 0)
                {
                    Console.WriteLine("You failed to enter account number.\nExiting Program...\n");
                    Environment.Exit(12345);
                }
            }
            else
            {
                break;
            }
        }
    }
};

class Program
{
    static void Main(string[] args)
    {
        Bank obj = new Bank();
        
        Console.WriteLine("Welcome to Test Bank!");
        while (true)
        {
            Console.WriteLine("Menu:\n1.Create Account\n2.Deposit\n3.Withdraw\n4.Check Balance\n5.Exit");
            int userMenuInput = Convert.ToInt32(Console.ReadLine());

            switch (userMenuInput)
            {
                case 1:
                    obj.CreateAccount();
                    break;
            
                case 2:
                    obj.Deposit();
                    break;
            
                case 3:
                    obj.Withdraw();
                    break;
                case 4:
                    obj.CheckBalance();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Wrong value.\nExiting Program");
                    break;
            }
        }
    }
}