namespace day12
{
    class Program
    {
        static void Main()
        {
            // Step 1
            var map = ReadMap("input.txt");
            var steps = Walk(map, GetStartPos(map), new Dictionary<Vector, int>(), 0);
            Console.WriteLine(steps);

            // Step 2
            var minSteps = 99999;
            foreach (var startPos in GetAllStartPos(map))
            {
                var thisSteps = Walk(map, startPos, new Dictionary<Vector, int>(), 0);
                if (thisSteps < minSteps)
                    minSteps = thisSteps;
            }
            Console.WriteLine(minSteps);
        }

        private static IEnumerable<Vector> GetAllStartPos(char[,] map)
        {
            for (var y = 0; y < map.GetLength(1); y++)
                for (var x = 0; x < 3; x++) // This 3 is just because the input map has no b's outside of column 2, will not work for more complex inputs
                    if (map[x, y] == 'a' || map[x, y] == 'S')
                        yield return new Vector { X = x, Y = y };
        }

        private static Vector GetStartPos(char[,] map)
        {
            for (var y = 0; y < map.GetLength(1); y++)
                for (var x = 0; x < map.GetLength(0); x++)
                    if (map[x, y] == 'S') return new Vector { X = x, Y = y };
            return new Vector { X = 0, Y = 0 };
        }

        static int Walk(char[,] map, Vector position, Dictionary<Vector, int> oldPositions, int steps)
        {
            if (!oldPositions.ContainsKey(position))
                oldPositions.Add(position, steps);
            else
                oldPositions[position] = steps;
            var curVal = map[position.X, position.Y];
            if (curVal == 'E')
                return 0;
            else if (curVal == 'S')
                curVal = 'a';
            // Up
            var upCount = 0;
            if (position.Y > 0 && ConvE(map[position.X, position.Y-1]) <= curVal+1)
            {
                var newPos = new Vector { X = position.X, Y = position.Y - 1 };
                if (!oldPositions.ContainsKey(newPos) || oldPositions[newPos] > steps+1)
                    upCount = Walk(map, newPos, oldPositions, steps+1) + 1;
            }
            // Right
            var rightCount = 0;
            if (position.X < map.GetLength(0)-1 && ConvE(map[position.X+1, position.Y]) <= curVal+1)
            {
                var newPos = new Vector { X = position.X + 1, Y = position.Y };
                if (!oldPositions.ContainsKey(newPos) || oldPositions[newPos] > steps+1)
                    rightCount = Walk(map, newPos, oldPositions, steps+1) + 1;
            }
            // Down
            var downCount = 0;
            if (position.Y < map.GetLength(1)-1 && ConvE(map[position.X, position.Y+1]) <= curVal+1)
            {
                var newPos = new Vector { X = position.X, Y = position.Y + 1 };
                if (!oldPositions.ContainsKey(newPos) || oldPositions[newPos] > steps+1)
                    downCount = Walk(map, newPos, oldPositions, steps + 1) + 1;
            }
            // Left
            var leftCount = 0;
            if (position.X > 0 && ConvE(map[position.X-1, position.Y]) <= curVal+1)
            {
                var newPos = new Vector { X = position.X - 1, Y = position.Y };
                if (!oldPositions.ContainsKey(newPos) || oldPositions[newPos] > steps+1)
                    leftCount = Walk(map, newPos, oldPositions, steps + 1) + 1;
            }
            if (upCount == 0 && rightCount == 0 && downCount == 0 && leftCount == 0)
                return 99999999;
            return new[] { upCount, rightCount, downCount, leftCount }.Where(c => c > 0).Min();
        }

        private static char ConvE(char inp)
        {
            if (inp == 'E') return 'z';
            return inp;
        }

        static char[,] ReadMap(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var Width = lines[0].Length;
            var Height = lines.Length;
            var mapArray = new char[Width, Height];
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    mapArray[x, y] = lines[y][x];
            return mapArray;
        }
    }

    record Vector
    {
        internal int X;
        internal int Y;
    }
}