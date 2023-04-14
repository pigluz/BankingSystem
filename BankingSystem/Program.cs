using System.Data.SqlClient;

namespace BankingSystem;

class Program
{
    private static void Main(string[] args)
    {
            Console.WriteLine("Connecting to the database....");
            using (var connection = new SqlConnection(BankAccount.ConnectionString))
            {
                connection.Open();
                Console.WriteLine("Connected.\n");

                Console.WriteLine("Welcome to Test Bank!");
                while (true)
                {
                    Console.WriteLine("\nMenu:\n1.Create Account\n2.Deposit\n3.Withdraw\n4.Check Balance\n5.Exit\n");
                    var userMenuInput = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Type your account number... (2-6 characters long)");
                    var number = Convert.ToInt32(Console.ReadLine());
                    var accountNr = BankAccount.GetAccount(number);

                    switch (userMenuInput)
                    {
                        case 1:
                            accountNr.CreateAccount(number);
                            break;
                            
                        case 2:
                            accountNr.Deposit();
                            break;

                        case 3:
                            accountNr.Withdraw();
                            break;

                        case 4:
                             var balance = accountNr.CheckBalance();
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