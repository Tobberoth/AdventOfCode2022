var values = File.ReadAllLines("input.txt")
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l => new Value(l))
    .ToList();

// Step 1
var couples = values
    .Select((v, i) => new { Index = i, Value = v })
    .GroupBy(x => x.Index / 2)
    .Select(g => new { Index = g.Key, V1 = g.First().Value, V2 = g.Skip(1).First().Value }).ToList();
var indexSum = 0;
foreach (var couple in couples)
    if (couple.V1.CompareTo(couple.V2) < 0)
        indexSum += couple.Index + 1;
Console.WriteLine($"Correct index sum: {indexSum}");

// Step 2
values.Add(new Value("[[2]]", true));
values.Add(new Value("[[6]]", true));
values.Sort();
var divIndexes = new List<int>();
for (var i = 0; i < values.Count; i++)
    if (values[i].IsDivPack)
        divIndexes.Add(i + 1);
Console.WriteLine($"Decode key: {divIndexes.Aggregate(1, (acc, x) => acc * x )}");

class Value : IComparable
{
    internal int? IntegerValue { get; set; }
    internal List<Value> Values { get; set; } = new();
    internal bool IsDivPack { get; set; }

    internal Value(string val, bool isDivPack = false)
    {
        IsDivPack = isDivPack;
        if (string.IsNullOrEmpty(val)) return;
        if (char.IsDigit(val[0]))
        {
            IntegerValue = int.Parse(val);
            return;
        }

        if (val.StartsWith("["))
            Values.AddRange(SplitValues(val[1..^1]).ToList().Select(v => new Value(v)));
    }

    public int CompareTo(object? obj)
    {
        var left = this;
        if (obj is not Value right) return 1;
        if (left.IntegerValue.HasValue && right.IntegerValue.HasValue)
        {
            if (left.IntegerValue < right.IntegerValue)
                return -1;
            if (left.IntegerValue > right.IntegerValue)
                return 1;
            return 0;
        }
        if (left.IntegerValue.HasValue)
            left = new Value($"[{left.IntegerValue}]");
        if (right.IntegerValue.HasValue)
            right = new Value($"[{right.IntegerValue}]");
        for (var i = 0; i < Math.Max(left.Values.Count, right.Values.Count); i++)
        {
            if (i >= left.Values.Count) return -1;
            if (i >= right.Values.Count) return 1;
            var res = left.Values[i].CompareTo(right.Values[i]);
            if (res != 0) return res;
        }
        return 0;
    }

    static IEnumerable<string> SplitValues(string s)
    {
        var currentNum = "";
        var currentList = "";
        var listDepth = 0;
        foreach (var c in s)
        {
            if (c == ',')
            {
                if (!string.IsNullOrEmpty(currentNum))
                {
                    yield return currentNum;
                    currentNum = "";
                    continue;
                }
                if (listDepth == 0)
                {
                    yield return currentList;
                    currentList = "";
                    continue;
                }
            }
            if (char.IsDigit(c))
            {
                if (string.IsNullOrEmpty(currentList))
                {
                    currentNum += c;
                    continue;
                }
            }
            if (c == '[')
            {
                listDepth++;
                currentList += c;
                continue;
            }
            if (c == ']')
                listDepth--;
            currentList += c;
        }
        if (!string.IsNullOrEmpty(currentNum))
            yield return currentNum;
        if (!string.IsNullOrEmpty(currentList))
            yield return currentList;
    }
}