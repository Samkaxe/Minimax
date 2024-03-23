
class MonteCarloPiEstimator
{
    static Random random = new Random();
    static Dictionary<string, string> counters = new Dictionary<string, string>{
        {"Rock", "Paper"}, {"Paper", "Scissors"}, {"Scissors", "Rock"}
    };

    static string[] choices = new string[] { "Rock", "Paper", "Scissors" };

    static string GetRandomChoice()
    {
       return  choices[random.Next(choices.Length)]; 
       // return  "Paper";
    }

    // The Monte Carlo bot will simply track what the random bot plays and play the counter to the most frequent choice
    static string GetMonteCarloChoice(Dictionary<string, int> history)
    {
        if (history.Count == 0) return GetRandomChoice();
        var mostFrequent = history.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        return counters[mostFrequent];
    }

    static void Main(string[] args)
    {
        int monteCarloWins = 0, randomBotWins = 0, draws = 0;
        Dictionary<string, int> randomBotHistory = new Dictionary<string, int> {
            {"Rock", 0}, {"Paper", 0}, {"Scissors", 0}
        };
        
        for (int i = 0; i < 10; i++)
        {
            string randomBotChoice = GetRandomChoice();
            string monteCarloChoice = GetMonteCarloChoice(randomBotHistory);

            // Update history for Monte Carlo bot
            if (!randomBotHistory.ContainsKey(randomBotChoice))
                randomBotHistory[randomBotChoice] = 0;
            randomBotHistory[randomBotChoice]++;

            // Determine winner
            if (randomBotChoice == monteCarloChoice)
            {
                draws++;
            }
            else if (counters[randomBotChoice] == monteCarloChoice)
            {
                monteCarloWins++;
            }
            else
            {
                randomBotWins++;
            }
            Console.WriteLine("randomBotChoice " +randomBotChoice);
            Console.WriteLine("monteCarloChoice : " + monteCarloChoice);
        }
        
        Console.WriteLine($"After 1000000 rounds:");
        Console.WriteLine($"Monte Carlo Bot wins: {monteCarloWins}");
        Console.WriteLine($"Random Bot wins: {randomBotWins}");
        Console.WriteLine($"Draws: {draws}");
       
    }
}
