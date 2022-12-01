string inputFile = "input.txt";
List<int> elfCalories = new();
int count = 0;
foreach (var line in File.ReadAllLines(inputFile))
{
    if (string.IsNullOrEmpty(line))
    {
        elfCalories.Add(count);
        count = 0;
    }
    else
        count += int.Parse(line);
}
elfCalories.Sort();
elfCalories.Reverse();
// Step 1
Console.WriteLine(elfCalories[0]);
// Step 2
Console.WriteLine(elfCalories.Take(3).Sum());