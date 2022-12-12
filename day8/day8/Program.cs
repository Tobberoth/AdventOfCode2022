var map = ReadMatrix("input.txt");
var cleanMap = RemoveInvisible(map);
// Step 1
Console.WriteLine("Count: " + CountVisible(cleanMap));

// Step 2
var max = 0;
for (var y = 0; y < map.GetLength(1); y++)
{
    for (var x = 0; x < map.GetLength(0); x++)
    {
        var el = EvaluatePosition(map, x, y);
        if (el > max)
            max = el;
    }
}
Console.WriteLine("Max eval: " + max);


int EvaluatePosition(int[,] map, int x, int y)
{
    var height = map[x, y];
    // Look up, right, down, left
    // Up
    var upCount = 0;
    for (var z = y-1; z >= 0; z--)
    {
        upCount++;
        if (map[x, z] >= height)
            break;
    }
    // Right
    var rightCount = 0;
    for (var z = x+1; z < map.GetLength(0); z++)
    {
        rightCount++;
        if (map[z, y] >= height)
            break;
    }
    // Down
    var downCount = 0;
    for (var z = y+1; z < map.GetLength(1); z++)
    {
        downCount++;
        if (map[x, z] >= height)
            break;
    }
    // Left
    var leftCount = 0;
    for (var z = x-1; z >= 0; z--)
    {
        leftCount++;
        if (map[z, y] >= height)
            break;
    }
    return upCount * rightCount * downCount * leftCount;
}

int CountVisible(int[,] cleanMap)
{
    var count = 0;
    foreach (var val in cleanMap)
    {
        if (val > -1) count++;
    }
    return count;
}

int[,] RemoveInvisible(int[,] map)
{
    var cleanMap = new int[map.GetLength(0), map.GetLength(1)];
    for (var y = 0; y < map.GetLength(1); y++)
    {
        for (var x = 0; x < map.GetLength(0); x++)
        {
            if (!Visible(map, x, y))
                cleanMap[x, y] = -1;
            else
                cleanMap[x, y] = map[x,y];
        }
    }
    return cleanMap;
}

bool Visible(int[,] map, int x, int y)
{
    if (x == 0 || y == 0) return true;
    if (x == map.GetLength(0)) return true;
    if (y == map.GetLength(1)) return true;
    var height = map[x,y];
    // Check top, right, bottom, left
    // check top
    var visibleTop = true;
    for (var z = 0; z < y; z++)
    {
        if (map[x, z] >= height)
        {
            visibleTop = false;
            break;
        }
    }
    if (visibleTop) return true;

    // check right
    var visibleRight = true;
    for (var z = map.GetLength(0)-1; z > x; z--)
    {
        if (map[z, y] >= height)
        {
            visibleRight = false;
            break;
        }
    }
    if (visibleRight) return true;

    // check bottom
    var visibleBottom = true;
    for (var z = map.GetLength(1)-1; z > y; z--)
    {
        if (map[x, z] >= height)
        {
            visibleBottom = false;
            break;
        }
    }
    if (visibleBottom) return true;

    // check left
    var visibleLeft = true;
    for (var z = 0; z < x; z++)
    {
        if (map[z, y] >= height)
        {
            visibleLeft = false;
            break;
        }
    }
    if (visibleLeft) return true;

    return false;
}

int[,] ReadMatrix(string inputFile)
{
    var lines = File.ReadAllLines(inputFile);
    int[,] ret = new int[lines[0].Length, lines.Length];
    for (var y = 0; y < lines.Length; y++)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; x++)
        {
            ret[x, y] = int.Parse(line[x].ToString());
        }
    }
    return ret;
}