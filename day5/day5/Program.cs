using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
DoStep(lines, 1);
DoStep(lines, 2);

void DoStep(string[] lines, int step)
{
    Console.WriteLine("\r\nStep 1:");
    var stacks = ReadStacks(lines);
    PerformMoves(lines, stacks, step);
    foreach (var stack in stacks)
    {
        Console.Write(stack.Peek());
    }
}

List<Stack<string>> ReadStacks(string[] lines)
{
    Dictionary<int, List<string>> Stacks = new();
    foreach (var line in lines)
    {
        if (!line.Contains('['))
            break;
        for (var i = 0; i < line.Length / 4 + 1; i++)
        {
            var pos = i * 4 + 1;
            if (line[pos] != ' ')
            {
                if (!Stacks.ContainsKey(i))
                    Stacks.Add(i, new List<string>());
                Stacks[i].Insert(0, line[pos].ToString());
            }
        }
    }
    return Stacks.OrderBy(s => s.Key).Select(s => new Stack<string>(s.Value)).ToList();
}

void PerformMoves(string[] lines, List<Stack<string>> stacks, int step)
{
    foreach (var line in lines.Where(l => l.StartsWith("move")))
    {
        var match = Regex.Match(line, "(\\d+).*?(\\d+).*?(\\d+)");
        var amount = int.Parse(match.Groups[1].Value);
        var from = int.Parse(match.Groups[2].Value) - 1;
        var to = int.Parse(match.Groups[3].Value) - 1;
        if (step == 1)
            Step1(stacks, amount, to, from);
        else
            Step2(stacks, amount, to, from);
    }
}

void Step1(List<Stack<string>> stacks, int amount, int to, int from)
{
    for (var i = 0; i < amount; i++)
    {
        stacks[to].Push(stacks[from].Pop());
    }
}

void Step2(List<Stack<string>> stacks, int amount, int to, int from)
{
        Stack<string> tempStack = new();
        for (var i = 0; i < amount; i++)
        {
            tempStack.Push(stacks[from].Pop());
        }
        while (tempStack.Count > 0)
            stacks[to].Push(tempStack.Pop());
}
