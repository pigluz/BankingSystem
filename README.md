# Banking System
C# Program that implements a simple banking system. With several methods to perform basic banking operations, such as creating an account, depositing money, and withdrawing money. It uses ADO.NET to interact with a SQL Server database.

## Requirements
 <ul>
  <li>.NET IDE</li>
  <li> SQL Server Managment Studio </li>
  <li> A working Microsoft SQL Server LocalBD named "BankBD" and with a database called "Bank"</li>
  <li> A backup of the <a href="DB_backup.bak">tables</a></li>
</ul>

## How to use?
1. Clone the repository `git clone https://github.com/pigluz/BankingSystem.git`
2. Restore the backup in SQL Server Managment Studio
3. Start the server
4. Build the code with `dotnet build`
5. Run `dotnet run --project BankingSystem`

## Functions
Working functions:
  <ul>
    <li> Creating the account</li>
    <li> Depositing/Withdrawing </li>
    <li> Checking the history </li>
    <li> Checking the balance </li>
  </ul>
  
 ## To do's
 <ul>
  <li>Better validation</li>
  <li>Better error handling</li>
  <li>More user-friendly interface</li>
  <li>Convert to WPF</li>
 </ul>

<br> Started: 01.04.2023
