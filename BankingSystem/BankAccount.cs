using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace BankingSystem;

class BankAccount
{
    public static string ConnectionString = @"Server=(localdb)\BankBD;DataBase=Bank";
    public int PersonId { get; set; }
    public int AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public decimal UserDepositMoney { get; set; }
    public decimal UserWithdrawMoney { get; set; }


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
                    bankAccount.PersonId = (int)reader["PersonID"];
                    bankAccount.AccountNumber = (int)reader["AccountNumber"];
                    bankAccount.Balance = (decimal)reader["Balance"];
                }

                connection.Close();
                return bankAccount;
            }

        }
        return null;
    }

    public void CreateAccount(int accNumber)
    {
        while (true)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    // If user enters int correctly...
                    if (!(accNumber <= 10 || accNumber >= 100000))
                    {
                        connection.Open();

                        var queryIfAccountExisit =
                            $"SELECT CASE WHEN EXISTS (SELECT * FROM finanse.bankSystem WHERE AccountNumber = '{accNumber}') THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END;";
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
                Console.WriteLine($"ERROR: {ex.Message}\n");
            }

            break;
        }
    }

    public void Deposit()
    {
        decimal userDepositMoney = 0;
        HistoryInfo dateInfo = new HistoryInfo();
        BankAccount depositInfo = new BankAccount();

        while (true)
        {
            try
            {
                Console.WriteLine("How much money do you want to deposit?");
                userDepositMoney = Convert.ToDecimal(Console.ReadLine());
                var depositResult = Balance += userDepositMoney;

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

                    Console.WriteLine($"Success! Your new balance is now {depositResult}\n");
                    depositInfo.UserDepositMoney = userDepositMoney;

                    var time = DateTime.Now;
                    dateInfo.Date = time.ToString("yyyy-MM-dd HH:mm:ss");

                    var queryDepositInfo =
                        $"INSERT INTO finanse.accountExpenses ([Withdraw/Deposit], BalanceAfter, PersonID, Date) VALUES ('++{depositInfo.UserDepositMoney}', {depositResult}, {PersonId}, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
                    command = new SqlCommand(queryDepositInfo, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                }

                break;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n");
            }
        }
    }


    public void Withdraw()
    {
        decimal userWithdrawMoney = 0;
        HistoryInfo dateInfo = new HistoryInfo();
        BankAccount withdrawInfo = new BankAccount();

        while (true)
        {
            try
            {
                Console.WriteLine("How much money do you want to withdraw?");
                userWithdrawMoney = Convert.ToDecimal(Console.ReadLine());

                // If user enters a value that is higher than its bank account's balance
                if (userWithdrawMoney > Balance)
                {
                    Console.WriteLine(
                        $"\nYou entered a value that is higher than balance.\nYour balance is: {Balance}\n\nPlease enter a lower value.\n");
                }

                else
                {
                    var withdrawResult = Balance -= userWithdrawMoney;
                    var queryWithdraw =
                        $"UPDATE finanse.bankSystem SET Balance = {withdrawResult} WHERE accountNumber = {AccountNumber}";

                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        
                        var command = new SqlCommand(queryWithdraw, connection);
                        command.ExecuteNonQuery();

                        Console.WriteLine($"Success! Your new balance is now {withdrawResult}\n");
                        withdrawInfo.UserWithdrawMoney = userWithdrawMoney;

                        var time = DateTime.Now;
                        dateInfo.Date = time.ToString("yyyy-MM-dd HH:mm:ss");

                        var queryWithdrawInfo =
                            $"INSERT INTO finanse.accountExpenses ([Withdraw/Deposit], BalanceAfter, PersonID, Date) VALUES ('-{withdrawInfo.UserWithdrawMoney}', {withdrawResult}, {PersonId}, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
                        command = new SqlCommand(queryWithdrawInfo, connection);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }

                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n");
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

        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}\n");
        }

        return Balance;
    }

    public void CheckHistory()
    {
        List<HistoryInfo> HistoryInfos_ = new List<HistoryInfo>();

        // 1. Uzytkownik wpisuje numer w menu.
        // 2. Baza danych robi SELECT w accountExpenses.

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var querySelect = $"SELECT * FROM finanse.accountExpenses WHERE PersonID = {PersonId}";
            var command = new SqlCommand(querySelect, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = (int)reader["ExpensesID"];
                    var operation = (decimal)reader["Withdraw/Deposit"];
                    var balanceafter = (decimal)reader["BalanceAfter"];
                    var date = reader.GetDateTime(4).ToString("g");

                    var history = new HistoryInfo
                    { Id = id, Operations = operation, BalanceAfter = balanceafter, Date = date };
                    HistoryInfos_.Add(history);
                }
            }
            connection.Close();
        }

        Console.WriteLine(($"BALANCE HISTORY FOR ACCOUNT: {AccountNumber}:\n" +
                           $"    |  Operations   |  Balance After  |       Date  "));

        if (HistoryInfos_ != null)
        {
            // 3. Zwraca wynik ostatnich operacji, w kolejnosci od najświeższej za pomocą FOREACH. 
            foreach (var item in HistoryInfos_)
            {
                Console.WriteLine($" {item.Id}  |       {item.Operations}      |      {item.BalanceAfter}       |   {item.Date}  ");
                break;
            }
        }
        else
        {
            Console.WriteLine("There is no history!");
        }


        // Taki wzór:
        // History for {accountNumber}:
        //  Number  |  Operation |  Balance After   |  Date

        //    1.   |   -{value}  | {balance after} | {date}   
        //    2.   |  +{value}  | {balance after} | {date}
    }


}