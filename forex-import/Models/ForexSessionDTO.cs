using System.Text.Json.Serialization;

namespace forex_import.Models
{
    public  class ForexSessionDTO
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("Idinfo")]
        public string idinfo { get; set; }
        
        [JsonPropertyName("SessionType")]
        public string SessionType { get; set; }
        
        [JsonPropertyName("ExperimentId")]
        public string ExperimentId { get; set; }
        
        [JsonPropertyName("StartDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        public string EndDate { get; set; }

        [JsonPropertyName("LastUpdatedTime")]
        public string LastUpdatedTime { get; set; }

        [JsonPropertyName("CurrentTime")]
        public string CurrentTime { get; set; }
        
        [JsonPropertyName("RealizedPL")]
        public double RealizedPL{ get; set; }
        
        [JsonPropertyName("Strategy")]
        public StrategyDTO Strategy { get; set; }
        
        [JsonPropertyName("sessionUser")]
        public SessionUserDTO SessionUser { get; set; }
        
        [JsonPropertyName("Balance")]
        public double Balance { get;set;}
        
        [JsonPropertyName("PercentComplete")]
        public string PercentComplete { get; set; }
        
        [JsonPropertyName("elapsedTime")]
        public string elapsedTime { get; set; }
        
        [JsonPropertyName("beginSessionTime")]
        public string beginSessionTime { get; set; }
        
        [JsonPropertyName("endSessionTime")]
        public string endSessionTime { get; set; }
    }

    public  class SessionUserDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("idinfo")]
        public string idinfo { get; set; }
        
        [JsonPropertyName("status")]
        public object Status { get; set; }
        
        [JsonPropertyName("Accounts")]
        public AccountsDTO Accounts { get; set; }
        
        [JsonPropertyName("balance")]
        public double Balance{ get; set; }
        
        [JsonPropertyName("closedTrades")]
        public int ClosedTrades{ get; set; }
        
        [JsonPropertyName("openTrades")]
        public int OpenTrades{ get; set; }
        
        [JsonPropertyName("percentProfitableClosed")]
        public double PercentProfitableClosed{ get; set; }
        
        [JsonPropertyName("percentProfitableOpen")]
        public double PercentProfitableOpen{ get; set; }
        
        [JsonPropertyName("realizedPL")]
        public double RealizedPL { get; set; }

        
    }

    public  class AccountsDTO
    {
        [JsonPropertyName("primary")]
        public AccountDTO Primary { get; set; }
        
        [JsonPropertyName("secondary")]
        public AccountDTO Secondary { get; set; }
    }

    public  class AccountDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("idinfo")]
        public string idinfo { get; set; }
        
        [JsonPropertyName("cash")]
        public double Cash { get; set; }
        
        [JsonPropertyName("margin")]
        public long Margin { get; set; }
        
        [JsonPropertyName("marginRatio")]
        public long MarginRatio { get; set; }
        
        [JsonPropertyName("realizedPL")]
        public double RealizedPL { get; set; }
        
        [JsonPropertyName("trades")]
        public TradeDTO[] Trades { get; set; }
        
        [JsonPropertyName("orders")]
        public OrderDTO[] Orders { get; set; }
        
        [JsonPropertyName("closedTrades")]    
        public TradeDTO[] ClosedTrades { get; set; }
        
        [JsonPropertyName("balanceHistory")]
        public BalanceHistoryDTO[] BalanceHistory { get; set; }
        
        [JsonPropertyName("idcount")]
        public long Idcount { get; set; }
    }


    public  class BalanceHistoryDTO
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Amount")]
        public double Amount { get; set; }
    }

    public  class TradeDTO
    {
        [JsonPropertyName("Id")]
        public long? Id { get; set; }

        [JsonPropertyName("idinfo")]
        public long? idinfo { get; set; }

        [JsonPropertyName("Pair")]
        public string Pair { get; set; }

        [JsonPropertyName("Units")]
        public long Units { get; set; }

        [JsonPropertyName("OpenDate")]
        public string OpenDate { get; set; }

        [JsonPropertyName("CloseDate")]
        public string CloseDate { get; set; }

        [JsonPropertyName("Long")]
        public bool Long { get; set; }

        [JsonPropertyName("OpenPrice")]
        public double OpenPrice { get; set; }

        [JsonPropertyName("ClosePrice")]
        public double ClosePrice { get; set; }

        [JsonPropertyName("StopLoss")]
        public double? StopLoss { get; set; }

        [JsonPropertyName("TakeProfit")]
        public double? TakeProfit { get; set; }

        [JsonPropertyName("Init")]
        public bool Init { get; set; }

        [JsonPropertyName("PL")]
        public double PL { get; set; }
        
    }

    public  class OrderDTO
    {
        [JsonPropertyName("ExpirationDate")]
        public object ExpirationDate { get; set; }

        [JsonPropertyName("Triggerprice")]
        public double Triggerprice { get; set; }

        [JsonPropertyName("Expired")]
        public bool Expired { get; set; }

        [JsonPropertyName("Above")]
        public bool Above { get; set; }

        [JsonPropertyName("Trade")]
        public TradeDTO Trade { get; set; }
    }

    public class StrategyDTO
   {
       [JsonPropertyName("window")]
        public int window{get;set;}

        [JsonPropertyName("ruleName")]
        public string ruleName{get;set;}

        [JsonPropertyName("position")]
        public string position{get;set;}

        [JsonPropertyName("units")]
        public int units{get;set;}

        [JsonPropertyName("stopLoss")]
        public double stopLoss{get;set;}

        [JsonPropertyName("takeProfit")]
        public double takeProfit{get;set;}

   } 

}
