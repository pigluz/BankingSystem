using System.Data.SqlClient;

namespace BankingSystem;

public class Program
{
    // Serwer o nazwie "BankDB" ; Baza danych o nazwie "Bank" 


    private static void Main(string[] args)
    {
        var bank = new Bank();
        var exit = false;

        while (!exit)
        {
            Console.WriteLine("Welcome to Test Bank!");
            while (true)
            {
                Console.WriteLine("Menu:\n1.Create Account\n2.Deposit\n3.Withdraw\n4.Check Balance\n5.Exit\n");
                var userMenuInput = Convert.ToInt32(Console.ReadLine());

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
                        Console.WriteLine("\nExiting Program");
                        
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("\nWrong value.\nExiting Program");
                       
                        Environment.Exit(0);
                        break;
                }
            }
        }

    }
}