using System;
using Newtonsoft.Json;

namespace com.microsoft;

public class GameResult
{
    [JsonProperty("solved")]
    public bool Solved { get; set; }
    
    [JsonProperty("solution")]
    public string Solution { get; set; }
    
    [JsonProperty("solvedDate")]
    public DateTime SolvedDate { get; set; }

    static string[] possibleResults = {"trove","thorn","other","tacit","swill","dodge","shake"};

    public static GameResult GenerateResult()
    {
        return new GameResult {
            Solved = Random.Shared.Next(0, 2) == 0,
            Solution = possibleResults[Random.Shared.Next(0,7)],
            SolvedDate = DateTime.Now.AddDays(Random.Shared.Next(-3,1))
        };
    }
}