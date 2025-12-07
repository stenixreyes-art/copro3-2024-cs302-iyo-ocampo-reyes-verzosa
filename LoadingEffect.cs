namespace Morgan_Thieves
{
    public class LoadingScreen
    {
        public static void Show()
        {
            // --- 1. Setup the Console ---
            Console.CursorVisible = false; // Hide cursor for clean loading screen
            Console.Clear();               // Clear any previous output

            // --- 2. The ASCII Art Title ---
            // Each line of the ASCII art is a string in this array
            string[] title = new string[]
            {
                @"   ▄▄▄     ▄▄▄                                ",
                @"   ███▄ ▄███                                  ",
                @"   ██ ▀█▀ ██         ▄       ▄▄       ▄       ",
                @"   ██     ██   ▄███▄ ████▄▄████ ▄▀▀█▄ ████▄   ",
                @"   ██     ██   ██ ██ ██   ██ ██ ▄█▀██ ██ ██   ",
                @" ▀██▀     ▀██▄▄▀███▀▄█▀  ▄▀████▄▀█▄██▄██ ▀█   ",
                @"                             ██               ",
                @"                           ▀▀▀                ",
                @"                                              ",
                @"  ▄▄▄▄▄▄▄                                     ",
                @" █▀▀██▀▀▀▀ █▄                                 ",
                @"    ██     ██    ▀▀                           ",
                @"    ██     ████▄ ██ ▄█▀█▄▀█▄ ██▀▄█▀█▄ ▄██▀█   ",
                @"    ██     ██ ██ ██ ██▄█▀ ██▄██ ██▄█▀ ▀███▄   ",
                @"    ▀██▄  ▄██ ██▄██▄▀█▄▄▄  ▀█▀ ▄▀█▄▄▄█▄▄██▀   "};

            int windowWidth = Console.WindowWidth;

            // --- 2a. Print ASCII Art Title with Earth-like Colors ---
            // Green = land, Blue = water, DarkYellow = sand/mountains
            ConsoleColor[] earthColors = { ConsoleColor.DarkGray, ConsoleColor.White, ConsoleColor.Gray };

            Console.CursorTop = 5;
            foreach (string line in title)
            {
                int leftPadding = (windowWidth - line.Length) / 2; // Center the ASCII art
                if (leftPadding > 0) Console.SetCursorPosition(leftPadding, Console.CursorTop);

                int colorIndex = 0;
                foreach (char c in line)
                {
                    if (c == ' ') // Keep spaces empty
                    {
                        Console.Write(' ');
                        continue;
                    }

                    // Alternate colors for an "Earth-like" effect
                    Console.ForegroundColor = earthColors[colorIndex % earthColors.Length];
                    Console.Write(c);
                    colorIndex++;
                }
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("\n");

            // --- 3. The Loading Bar Loop ---
            int totalBlocks = 30; // Length of the loading bar

            for (int i = 0; i <= 100; i++)
            {
                double percent = i / 100.0;
                int filledBlocks = (int)(totalBlocks * percent);

                Console.Write("\r   Loading: [");

                // Earth-themed loading bar: green for land, blue for water
                for (int j = 0; j < filledBlocks; j++)
                {
                    Console.ForegroundColor = (j % 2 == 0) ? ConsoleColor.DarkGray : ConsoleColor.White;
                    Console.Write('█');
                }

                // Empty part of the bar in dark gray
                Console.ForegroundColor = ConsoleColor.DarkGray;
                for (int j = filledBlocks; j < totalBlocks; j++)
                {
                    Console.Write(' ');
                }

                Console.ResetColor();
                Console.Write($"] {i}%");
                Thread.Sleep(50); // Controls speed of the bar filling
            }

            // --- 4. Finished State ---
            Console.WriteLine("\n\n   Complete! Press any key to start your journey...");
            Console.ReadKey();

            // --- 5. Reset Console before starting game ---
            Console.ResetColor();
            Console.Clear();
            Console.CursorVisible = true;
        }
    }
}