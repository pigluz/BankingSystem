using System.Data.SqlClient;

namespace BankingSystem;

class Program
{
    private static void Main(string[] args)
    {
            Console.WriteLine("Łączenie z bazą danych...");
            using (var connection = new SqlConnection(BankAccount.ConnectionString))
            {
                connection.Open();
                Console.WriteLine("Połączono.\n");

                Console.WriteLine("Welcome to Test Bank!");
                while (true)
                {
                    Console.WriteLine("\nMenu:\n1.Create Account\n2.Deposit\n3.Withdraw\n4.Check Balance\n5.Exit\n");
                    var userMenuInput = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Type your account number...");
                    var number = Convert.ToInt32(Console.ReadLine());
                    var accountNr = BankAccount.GetAccount(number);
                    
                    switch (userMenuInput)
                    {
                        case 1:
                            accountNr.CreateAccount();
                            break;

                        case 2:
                            accountNr.Deposit();
                            break;

                        case 3:
                            accountNr.Withdraw();
                            break;

                        case 4:
                             var balance = accountNr.CheckBalance(accountNr.AccountNumber);
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