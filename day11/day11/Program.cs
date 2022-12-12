using System.Numerics;
using System.Text.RegularExpressions;

namespace day11
{
    class Program
    {
        static void Main()
        {
            var monkeys = InitMonkeys("input.txt", false);
            int rounds = 10000;
            for (var r = 1; r <= rounds; r++)
            {
                foreach (var monkey in monkeys)
                    monkey.CompleteRound(monkeys);
                if (r % 1000 == 0 || r == 20)
                {
                    var count = 0;
                    Console.WriteLine($"At round {r}:");
                    foreach (var monkey in monkeys)
                        Console.WriteLine($"Monkey {count} inspected {monkey.Inspected} times");
                }
            }
            var mostInspected1 = monkeys.OrderByDescending(m => m.Inspected).First();
            var mostInspected2 = monkeys.OrderByDescending(m => m.Inspected).Skip(1).First();
            Console.WriteLine($"Monkey business: {(ulong)mostInspected1.Inspected * (ulong)mostInspected2.Inspected}");
        }

        static List<Monkey> InitMonkeys(string filename, bool inspectRelief)
        {
            List<Monkey> ret = new();
            var maxMod = 1;
            var lines = File.ReadAllLines(filename).Select(s => s.Trim()).ToArray();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Monkey"))
                {
                    var currentMonkey = new Monkey();
                    currentMonkey.InspectRelief = inspectRelief;
                    i++;
                    currentMonkey.StartItems.AddRange(
                        lines[i]
                            .Replace("Starting items: ", "")
                            .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => long.Parse(s)));
                    i++;
                    var data = lines[i].Replace("Operation: new = old ", "").Split(' ');
                    var op = data[0];
                    var val = data[1];
                    if (op == "*")
                    {
                        if (int.TryParse(val, out int value))
                            currentMonkey.Operation = (old) => old * value;
                        else
                            currentMonkey.Operation = (old) => old * old;
                    }
                    if (op == "+")
                    {
                        if (int.TryParse(val, out int value))
                            currentMonkey.Operation = (old) => old + value;
                        else
                            currentMonkey.Operation = (old) => old + old;
                    }
                    i++;
                    var div = int.Parse(Regex.Match(lines[i], @"\d+").Value);
                    maxMod *= div;
                    currentMonkey.Divider = div;
                    currentMonkey.Test = (worrylevel) => worrylevel % (uint)div == 0;
                    i++;
                    currentMonkey.TrueTarget = int.Parse(Regex.Match(lines[i], @"\d+").Value);
                    i++;
                    currentMonkey.FalseTarget = int.Parse(Regex.Match(lines[i], @"\d+").Value);
                    ret.Add(currentMonkey);
                }
            }
            foreach (var m in ret)
                m.MaxMod = maxMod;
            return ret;
        }
    }

    class Monkey
    {
        internal List<long> StartItems { get; set; } = new List<long>();
        internal Func<long, long> Operation { get; set; }
        internal Func<long, bool> Test { get; set; }
        internal int TrueTarget { get; set; }
        internal int FalseTarget { get; set; }
        internal int Inspected { get; set; } = 0;
        internal bool InspectRelief { get; set; }
        internal int Divider { get; set; }
        internal int MaxMod { get; set; }
        internal void CompleteRound(List<Monkey> Monkeys)
        {
            foreach (var item in StartItems)
            {
                var newitem = item;
                newitem = Operation(newitem); // Inspect
                if (InspectRelief)
                    newitem = (long)Math.Floor(newitem / 3.0); // Relief
                else
                    newitem %= MaxMod;
                if (Test(newitem))
                    Monkeys[TrueTarget].StartItems.Add(newitem);
                else
                    Monkeys[FalseTarget].StartItems.Add(newitem);
                Inspected++;
            }
            StartItems.Clear();
        }
    }
}