namespace BankingSystem;

public class HistoryInfo
{
    public int Id { get; set; }
    public decimal Operations { get; set; }
    public decimal BalanceAfter { get; set; }
    
    public string Date { get; set; }
    
    public static List<HistoryInfo> HistoryInfos_ = null;
}