namespace BankingSystem;

public class HistoryInfo
{
    public int ID { get; set; }
    public decimal Operations { get; set; }
    public decimal BalanceAfter { get; set; }
    
    public string Date { get; set; }

    public static List<HistoryInfo> _historyInfos = new();
}