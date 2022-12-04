var s1count = 0;
var s2count = 0;
foreach (var line in File.ReadAllLines("input.txt"))
{
    var r1 = new Range(line.Split(',')[0]);
    var r2 = new Range(line.Split(',')[1]);
    if (r1.Contains(r2) || r2.Contains(r1))
        s1count++;
    if (r1.Overlap(r2) || r2.Overlap(r1))
        s2count++;
}
Console.WriteLine("step 1:" + s1count);
Console.WriteLine("step 2:" + s2count);

public class Range
{
    public int Length { get => End - Start; }
    public int Start { get; set; }
    public int End { get; set; }
    public Range(string input)
    {
        Start = int.Parse(input.Split('-')[0]);
        End = int.Parse(input.Split('-')[1]);
    }
    public bool Contains(Range other)
    {
        return Start <= other.Start && End >= other.End;
    }
    public bool Overlap(Range other)
    {
        return (Start <= other.Start && End >= other.Start) || (End <= other.End && End >= other.End);
    }
}