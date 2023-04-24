using System.Data.SqlClient;

namespace BankingSystem
{
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
                    Console.WriteLine(
                        "\nMenu:\n1.Create Account\n2.Deposit\n3.Withdraw\n4.Check Balance\n5.Check History\n6.Exit\n");

                    try
                    {
                        var userMenuInput = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Type your account number... (2-6 characters long)");
                        var number = Convert.ToInt32(Console.ReadLine());

                        switch (userMenuInput)
                        {
                            case 1:
                                var accountCase1 = new BankAccount();
                                accountCase1.CreateAccount(number);
                                break;

                            case 2:
                                var accountCase2 = BankAccount.GetAccount(number);
                                accountCase2.Deposit();
                                break;

                            case 3:
                                var accountCase3 = BankAccount.GetAccount(number);
                                accountCase3.Withdraw();
                                break;

                            case 4:
                                var accountCase4 = BankAccount.GetAccount(number);
                                var balance = accountCase4.CheckBalance();
                                break;

                            case 5:
                                var accountCase5 = BankAccount.GetAccount(number);
                                accountCase5.CheckHistory();
                                break;
                            case 6:
                                Console.WriteLine("\nExiting Program");
                                Environment.Exit(0);
                                break;

                            default:
                                Console.WriteLine("\nWrong value.\nExiting Program");
                                Environment.Exit(0);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: {ex.Message}\n");
                    }
                }
            }
        }
    }
}