using System.Text;

namespace day10
{
    internal class Program
    {
        static void Main()
        {
            var videoSystem = new VideoSystem("input.txt");
            int signalStrengthSum = 0;
            while (!videoSystem.Done)
            {
                videoSystem.Cycle();
                if ((videoSystem.TotalCycles - 20) % 40 == 0)
                    signalStrengthSum += videoSystem.RegisterX * videoSystem.TotalCycles;
            }
            Console.WriteLine($"Signal sum: {signalStrengthSum}");
            Console.WriteLine("---");
            Console.WriteLine(videoSystem.Video);
        }
    }

    class VideoSystem
    {
        public bool Done { get; private set; }
        public int RegisterX { get; private set; } = 1;
        public Command CurrentCommand { get => Program[ProgramPointer]; }
        public int TotalCycles { get; private set; }
        public string Video
        {
            get
            {
                StringBuilder sb = new();
                for (var y = 0; y < 6; y++)
                {
                    for (var x = 0; x < 40; x++)
                        sb.Append(VideoRam[x, y] ? "#" : ".");
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
        }

        int ProgramPointer { get; set; }
        Command[] Program { get; init; }
        int CurrentCommandCycles { get; set; }
        bool[,] VideoRam { get; set; } = new bool[40, 6];

        public VideoSystem(string programFile)
        {
            Program = File.ReadAllLines(programFile).Select(l => new Command(l)).ToArray();
        }
        public void Cycle()
        {
            if (CurrentCommandCycles >= CurrentCommand.Cycles)
            {
                RegisterX += CurrentCommand.Amount;
                ProgramPointer++;
                if (ProgramPointer >= Program.Length)
                {
                    Done = true;
                    return;
                }
                CurrentCommandCycles = 1;
            }
            else
            {
                CurrentCommandCycles++;
            }
            CrtDraw();
            TotalCycles++;
        }

        void CrtDraw()
        {
            var currentCol = (TotalCycles % 40);
            int currentRow = (int)Math.Floor(TotalCycles / 40.0);
            if (RegisterX >= currentCol - 1 && RegisterX <= currentCol + 1)
                VideoRam[currentCol, currentRow] = true;
            else
                VideoRam[currentCol, currentRow] = false;
        }
    }

    class Command
    {
        public string Name { get; private set; }
        public int Amount { get; private set; }
        public int Cycles { get; private set; }
        public Command(string input)
        {
            var data = input.Split(' ');
            if (data.Length < 2)
            {
                Name = input;
                Cycles = 1;
            }
            else
            {
                Name = data[0];
                Amount = int.Parse(data[1]);
                Cycles = 2;
            }
        }
    }
}