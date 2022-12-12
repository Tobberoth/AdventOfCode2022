namespace day9
{
    internal class Program
    {
        public static (int x, int y) HeadPosition { get; set; } = (0, 0);
        public static (int x, int y) TailPosition { get; set; } = (0, 0);
        public static HashSet<(int x, int y)> TailPositions { get; } = new() { (0, 0) };
        public static HashSet<(int x, int y)> TailPositions2 { get; } = new() { (0, 0) };

        public static List<(int x, int y)> Rope { get; } = new()
        {
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0)
        };

        static void Main()
        {
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                var direction = line.Split(' ')[0];
                var steps = int.Parse(line.Split(' ')[1]);
                for (var i = 0; i < steps; i++)
                {
                    Update(direction); // Step 1
                    UpdateRope(direction, Rope); // Step 2
                }
            }
            // Step 1
            Console.WriteLine(TailPositions.Count);

            // Step 2
            Console.WriteLine(TailPositions2.Count);
        }

        private static void UpdateRope(string direction, List<(int x, int y)> rope)
        {
            var (x, y) = Rope[0];
            Rope[0] = direction switch
            {
                "U" => (x, y - 1),
                "R" => (x + 1, y),
                "D" => (x, y + 1),
                "L" => (x - 1, y),
                _ => throw new ArgumentOutOfRangeException($"unknown direction: {direction}")
            };
            var prev = Rope[0];
            for (var i = 1; i < rope.Count; i++)
            {
                if (!IsTouching(rope[i], prev))
                {
                    rope[i] = MoveTail(rope[i], prev);
                    if (i == 9)
                        TailPositions2.Add(rope[i]);
                }
                prev = rope[i];
            }
        }

        static void Update(string direction)
        {
            HeadPosition = direction switch
            {
                "U" => (HeadPosition.x, HeadPosition.y - 1),
                "R" => (HeadPosition.x + 1, HeadPosition.y),
                "D" => (HeadPosition.x, HeadPosition.y + 1),
                "L" => (HeadPosition.x - 1, HeadPosition.y),
                _ => throw new ArgumentOutOfRangeException($"unknown direction: {direction}")
            };
            if (!IsTouching(TailPosition, HeadPosition))
            {
                TailPosition = MoveTail(TailPosition, HeadPosition);
                TailPositions.Add(TailPosition);
            }
        }

        private static (int x, int y) MoveTail((int x, int y) tailPosition, (int x, int y) headPosition)
        {
            // Same row
            if (tailPosition.y == headPosition.y)
            {
                if (tailPosition.x < headPosition.x)
                    return (tailPosition.x + 1, tailPosition.y);
                return (tailPosition.x - 1, tailPosition.y);
            }
            // Same column
            if (tailPosition.x == headPosition.x)
            {
                if (tailPosition.y < headPosition.y)
                    return (tailPosition.x, tailPosition.y + 1);
                return (tailPosition.x, tailPosition.y - 1);
            }
            // Diagonals
            if (headPosition.y < tailPosition.y && headPosition.x > tailPosition.x)
                return (tailPosition.x + 1, tailPosition.y - 1);
            if (headPosition.y < tailPosition.y && headPosition.x < tailPosition.x)
                return (tailPosition.x - 1, tailPosition.y - 1);
            if (headPosition.y > tailPosition.y && headPosition.x > tailPosition.x)
                return (tailPosition.x + 1, tailPosition.y + 1);
            if (headPosition.y > tailPosition.y && headPosition.x < tailPosition.x)
                return (tailPosition.x - 1, tailPosition.y + 1);
            return tailPosition;
        }

        private static bool IsTouching((int x, int y) tailPosition, (int x, int y) headPosition)
        {
            var xDist = Math.Abs(tailPosition.x - headPosition.x);
            var yDist = Math.Abs(tailPosition.y - headPosition.y);
            return (xDist == 0 && yDist == 0) || (xDist == 1 && (yDist == 1 || yDist == 0)) || (yDist == 1 && (xDist == 1 || xDist == 0));
        }
    }
}