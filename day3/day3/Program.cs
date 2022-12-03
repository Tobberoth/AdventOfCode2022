// Step 1
var sum = 0;
foreach (var rugsack in File.ReadAllLines("input.txt"))
    sum += GetRugsackPriority(rugsack);
Console.WriteLine("step1:" + sum);

// Step 2
sum = 0;
var lines = File.ReadAllLines("input.txt");
for (var i = 0; i < lines.Length / 3; i++)
    sum += GetRugsackBadgePriority(lines.Skip(i * 3).Take(3).ToArray());
Console.WriteLine("step2:" + sum);

static int GetRugsackBadgePriority(params string[] rugsacks)
{
    var firstItems = rugsacks[0].Intersect(rugsacks[1]);
    var badge = firstItems.Intersect(rugsacks[2]).First();
    return GetPriority(badge);
}

static int GetRugsackPriority(string rugsack)
{
    var splitIndex = rugsack.Length / 2;
    var rug1 = rugsack[..splitIndex];
    var rug2 = rugsack[splitIndex..];
    var item = rug1.Intersect(rug2).First();
    return GetPriority(item);
}

static int GetPriority(char c)
{
    if (char.IsLower(c)) return c - 96;
    return c - 38;
}
