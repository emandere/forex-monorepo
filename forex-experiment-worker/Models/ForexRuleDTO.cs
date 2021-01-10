using System.Text.Json.Serialization;
namespace forex_experiment_worker.Models
{
    public  class ForexRuleDTO
    {
        [JsonPropertyName("rulename")]
        public string RuleName { get; set; }
        [JsonPropertyName("ismet")]
        public bool IsMet { get; set; } 
        [JsonPropertyName("window")]  
        public int window { get; set; }
        [JsonPropertyName("indicator")]  
        public double indicator { get; set; }
    }   
}