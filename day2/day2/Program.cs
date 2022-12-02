var score = 0;
foreach (var line in File.ReadAllLines("input.txt").Select(l => l.Split(' ')))
{
    if (line is [string opponent, string strategy])
    {
        // Step 1
        //var own = TranslateMove(strategy);
        // Step 2
        var own = GetMoveFromStrategy(opponent, strategy);
        score += MoveToScore(own);
        score += ResultToScore(opponent, own);
    }
}
Console.WriteLine(score);

string TranslateMove(string strategy)
{
    return strategy switch { "X" => "A", "Y" => "B", "Z" => "C", _ => "A" };
}
string GetMoveFromStrategy(string opponent, string strategy)
{
    switch (opponent)
    {
        case "A":
            return strategy switch { "X" => "C", "Y" => "A", "Z" => "B", _ => "A" };
        case "B":
            return strategy switch { "X" => "A", "Y" => "B", "Z" => "C", _ => "A" };
        case "C":
            return strategy switch { "X" => "B", "Y" => "C", "Z" => "A", _ => "A" };
        default:
            return "A";
    }
}

int MoveToScore(string move)
{
    return move switch { "A" => 1, "B" => 2, "C" => 3, _ => 0 };
}

int ResultToScore(string opponent, string own)
{
    if (opponent == own) return 3;
    if (opponent == "A" && own == "B" ||
        opponent == "B" && own == "C" ||
        opponent == "C" && own == "A") return 6;
    return 0;
}