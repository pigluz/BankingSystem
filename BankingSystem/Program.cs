namespace BankingSystem;

class Program
{
    static void Main(string[] args)
    {
        Bank bank = new Bank();
        
        Console.WriteLine("Welcome to Test Bank!");
        while (true)
        {
            Console.WriteLine("Menu:\n1.Create Account\n2.Deposit\n3.Withdraw\n4.Check Balance\n5.Exit");
            int userMenuInput = Convert.ToInt32(Console.ReadLine());

            switch (userMenuInput)
            {
                case 1:
                    bank.CreateAccount();
                    break;
            
                case 2:
                    bank.Deposit();
                    break;
            
                case 3:
                    bank.Withdraw();
                    break;
                
                case 4:
                    bank.CheckBalance();
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