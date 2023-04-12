using System.Data.SqlClient;

namespace BankingSystem;

public class Program
{

    private static void Main(string[] args)
    {
        var bank = new Bank();
        var bankAcc = new BankAccount(); ;
        
            Console.WriteLine("Łączenie z bazą danych...");
            using (var connection = new SqlConnection(Bank.connectionString))
            {
                connection.Open();
                Console.WriteLine("Połączono.\n");

                Console.WriteLine("Welcome to Test Bank!");
                while (true)
                {
                    Console.WriteLine("\nMenu:\n1.Create Account\n2.Deposit\n3.Withdraw\n4.Check Balance\n5.Exit\n");
                    var userMenuInput = Convert.ToInt32(Console.ReadLine());

                    switch (userMenuInput)
                    {
                        case 1:
                            bank.CreateAccount();
                            break;

                        case 2:
                            bankAcc.Deposit();
                            break;

                        case 3:
                            bankAcc.Withdraw();
                            break;

                        case 4:
                            bankAcc.CheckBalance();
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