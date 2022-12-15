using System.Collections;

namespace day14
{
    class Program
    {
        static void Main()
        {
            // Step 1
            var sim = new Simulation("input.txt", false);
            sim.Run();
            Console.WriteLine($"Step 1 Landed sands: {sim.LandedSand}");
            // Step 2
            sim = new Simulation("input.txt", true);
            sim.Run();
            Console.WriteLine($"Step 2 Landed sands: {sim.LandedSand}");
        }
    }

    class Simulation
    {
        internal int LandedSand { get; set; } = 0;

        int MaxX { get; set; } = 0;
        int MinX { get; set; } = 9999;
        int MaxY { get; set; } = 0;
        char[,] Map { get; set; }
        bool SandFalling { get; set; } = false;
        (int x, int y) SandPosition { get; set; }
        bool HasFloor { get; set; }

        internal Simulation(string filename, bool hasFloor)
        {
            HasFloor = hasFloor;
            var paths = File.ReadAllLines(filename).Select(l => new Path(l)).ToList();
            foreach (var path in paths)
            {
                if (path.MinX < MinX) MinX = path.MinX;
                if (path.MaxX > MaxX) MaxX = path.MaxX;
                if (path.MaxY > MaxY) MaxY = path.MaxY;
            }
            if (HasFloor)
                (MinX, MaxX, MaxY) = (0, MaxX + 1000, MaxY + 2);
            Map = new char[MaxX+1, MaxY+1];
            foreach (var path in paths)
            {
                (int x, int y) previousPoint = (-1, -1);
                foreach ((int x, int y) point in path)
                {
                    if (previousPoint == (-1, -1))
                    {
                        previousPoint = point;
                        continue;
                    }
                    if (previousPoint.x == point.x) // vertical
                    {
                        var miny = Math.Min(previousPoint.y, point.y);
                        var maxy = Math.Max(previousPoint.y, point.y);
                        var diff = maxy - miny;
                        for (var dy = 0; dy <= diff; dy++)
                            Map[point.x, miny+dy] = '#';
                    }
                    else // horizontal
                    {
                        var minx = Math.Min(previousPoint.x, point.x);
                        var maxx = Math.Max(previousPoint.x, point.x);
                        var diff = maxx - minx;
                        for (var dx = 0; dx <= diff; dx++)
                            Map[minx+dx, point.y] = '#';
                    }
                    previousPoint = point;
                }
            }
        }

        internal void Run()
        {
            var isDone = false;
            while (!isDone)
                isDone = Tick();
        }

        bool Tick()
        {
            if (!SandFalling)
            {
                SandPosition = (500, 0);
                SandFalling = true;
            }

            (int x, int y) fallPosition;
            fallPosition = CheckFall(SandPosition.x, SandPosition.y + 1);
            if (fallPosition == (-1, -1))
                return true;
            if (fallPosition == (0,0))
                fallPosition = CheckFall(SandPosition.x - 1, SandPosition.y + 1);
            if (fallPosition == (-1, -1))
                return true;
            if (fallPosition == (0,0))
                fallPosition = CheckFall(SandPosition.x + 1, SandPosition.y + 1);
            if (fallPosition == (-1, -1))
                return true;
            if (fallPosition == (0, 0)) // landed
            {
                Map[SandPosition.x, SandPosition.y] = 'O';
                LandedSand++;
                if (SandPosition == (500, 0))
                    return true;
                SandFalling = false;
            }
            else
                SandPosition = fallPosition;
            return false;
        }

        (int x, int y) CheckFall(int x, int y)
        {
            if (HasFloor)
            {
                if (y >= MaxY || x < MinX || x > MaxX)
                    return (0, 0);
            }
            else
            {
                if (y > MaxY || x < MinX || x > MaxX)
                    return (-1, -1); // OOB
            }
            if (Map[x, y] == 0) return (x, y); // NEW POS
            return (0, 0);
        }
    }

    record Path : IEnumerable<(int x, int y)>
    {
        internal int MinX => Points.Min(p => p.x);
        internal int MaxX => Points.Max(p => p.x);
        internal int MinY => Points.Min(p => p.y);
        internal int MaxY => Points.Max(p => p.y);

        (int x, int y)[] Points { get; set; }

        internal Path(string input)
        {
            Points = input.Split(" -> ").Select(p => (int.Parse(p.Split(',')[0]), int.Parse(p.Split(',')[1]))).ToArray();
        }

        IEnumerator<(int x, int y)> IEnumerable<(int x, int y)>.GetEnumerator() =>
            Points.OfType<(int x, int y)>().GetEnumerator();

        public IEnumerator GetEnumerator() => GetEnumerator();
    }
}