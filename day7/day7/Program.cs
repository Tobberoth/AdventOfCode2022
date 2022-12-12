var data = File.ReadAllLines("input.txt");
FileNode root = null;
FileNode currentDir = null;
for (var rowIndex = 0; rowIndex < data.Length; rowIndex++)
{
    if (data[rowIndex].StartsWith("$ cd ") && !data[rowIndex].Contains(".."))
    {
        var dirName = data[rowIndex].Replace("$ cd ", "");
        if (currentDir == null)
        {
            root = new FileNode(dirName);
            currentDir = root;
        }
        else
            currentDir = currentDir.Children.First(c => c.Name == dirName);
    }
    else if (data[rowIndex] == "$ cd ..")
    {
        currentDir = currentDir.ParentDir;
    }
    else if (data[rowIndex] == "$ ls")
    {
        rowIndex++;
        while (!data[rowIndex].StartsWith("$"))
        {
            if (data[rowIndex].StartsWith("dir"))
            {
                var newDir = new FileNode(data[rowIndex].Replace("dir ", "")) { ParentDir = currentDir };
                currentDir.Children.Add(newDir);
            }
            else
            {
                currentDir.Children.Add(new FileNode(data[rowIndex].Split(' ')[1]) { Size = int.Parse(data[rowIndex].Split(' ')[0]), ParentDir = currentDir });
            }
            rowIndex++;
            if (rowIndex >= data.Length) break;
        }
        rowIndex--; // went too far
    }
}
Console.WriteLine(GetDirsBelow(root, 100000).Sum(d => d.CalcSize));

// Part 2
// Calc needed space
var spaceTotal = 70000000;
var spaceNeeded = 30000000;
var spaceLeft = spaceTotal - root.CalcSize;
var spaceMissing = spaceNeeded - spaceLeft;
// Find fitting dir
Console.WriteLine(GetDirsAbove(root, spaceMissing).OrderBy(d => d.CalcSize).First().CalcSize);

IEnumerable<FileNode> GetDirsBelow(FileNode dir, int maxSize)
{
    if (dir.Children.Count == 0) yield break;
    if (dir.CalcSize < maxSize) yield return dir;
    List<FileNode> ret = new();
    foreach (var child in dir.Children)
        ret.AddRange(GetDirsBelow(child, maxSize));
    foreach (var c in ret)
        yield return c;
}

IEnumerable<FileNode> GetDirsAbove(FileNode dir, int minSize)
{
    if (dir.Children.Count == 0) yield break;
    if (dir.CalcSize > minSize) yield return dir;
    List<FileNode> ret = new();
    foreach (var child in dir.Children)
        ret.AddRange(GetDirsAbove(child, minSize));
    foreach (var c in ret)
        yield return c;
}

public class FileNode
{
    public string Name { get; set; }
    public int CalcSize { get => Children.Sum(c => c.CalcSize) + Size; }
    public int Size { get; set; }
    public FileNode? ParentDir { get; set; }
    public List<FileNode> Children { get; set; } = new();
    public FileNode(string name) => Name = name;
}
