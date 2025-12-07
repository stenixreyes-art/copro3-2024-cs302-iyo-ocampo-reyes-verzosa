namespace Morgan_Thieves
{
    public interface IStatsAllocatorMode
    {
        bool chooseStatAllocationMode();
    }

    public class CharacterStats
    {
        public byte Strength { get; set; } = 1;
        public byte Vitality { get; set; } = 1;
        public byte Intelligence { get; set; } = 1;
        public byte Dexterity { get; set; } = 1;
        public byte Luck { get; set; } = 1;

        public void DisplayStats()
        {
            Console.WriteLine("--- Character Statistics ---");
            Console.WriteLine($"1. [STR] Strength    : {Strength}");
            Console.WriteLine($"2. [VIT] Vitality    : {Vitality}");
            Console.WriteLine($"3. [INT] Intelligence: {Intelligence}");
            Console.WriteLine($"4. [DEX] Dexterity   : {Dexterity}");
            Console.WriteLine($"5. [LUK] Luck        : {Luck}");
            Console.WriteLine("----------------------------");
        }
    }

    public class StatAllocator : IStatsAllocatorMode
    {
        public bool chooseStatAllocationMode()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("---- Fate Choice ----\n" +
                    "Shall fate decide your character’s abilities, or do you wish to assign them yourself?\n" +
                    "[1] Yes\n" +
                    "[2] No\n" +
                    "------\n\n" +
                    "[]: ");

                string input = Console.ReadLine();
                if (Byte.TryParse(input, out byte choice) && choice > 0 && choice <= 2)
                {
                    return (choice == 1) ? true : false;
                }
                else
                {
                    Console.WriteLine("There’s no such thing! Fate requires a clear choice.");
                    Console.ReadKey(true);
                }
            }
        }

        public void RunAutomaticAllocation(CharacterStats stats)
        {
            Console.Clear();
            foreach (char c in "So be it...")
            {
                Console.Write(c);
                Thread.Sleep(110);
            }
            Console.ReadKey(true);

            byte availablePoints = 20;

            while (availablePoints > 0)
            {
                Random random = new Random();
                string[] statsName = { "Strength", "Vitality", "Intelligence", "Dexterity", "Luck" };

                int randomStatIndex = random.Next(0, 5);

                switch (statsName[randomStatIndex])
                {
                    case "Strength": stats.Strength++; break;
                    case "Vitality": stats.Vitality++; break;
                    case "Intelligence": stats.Intelligence++; break;
                    case "Dexterity": stats.Dexterity++; break;
                    case "Luck": stats.Luck++; break;
                }
                availablePoints--;

                Console.Clear();
                Console.WriteLine("--- STAT ALLOCATION (Auto) ---");
                Console.WriteLine($"Points Remaining: {availablePoints}");
                stats.DisplayStats();
                Thread.Sleep(50);
            }
            Console.WriteLine("\nFate has spoken. Your journey begins!\n" +
                "Press any key to begin your voyage… Captain.");
            Console.ReadKey(true);
        }

        public void RunAllocation(CharacterStats stats)
        {
            Console.Clear();
            Console.WriteLine("Very well...\n" +
                "Your hands hold the threads of power, weave them carefully.");
            Console.ReadKey(true);
            Console.Clear();

            byte availablePoints = 15;

            stats.Strength = 1;
            stats.Vitality = 1;
            stats.Intelligence = 1;
            stats.Dexterity = 1;
            stats.Luck = 1;

            while (availablePoints > 0)
            {
                Console.Clear();
                Console.WriteLine("--- STAT ALLOCATION (Manual) ---");
                Console.WriteLine($"Points Remaining: {availablePoints}");
                stats.DisplayStats(); // Using the new display method

                Console.Write("Select stat to increase (1-5) or 0 to confirm: ");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 5)
                {
                    switch (choice)
                    {
                        case 1: stats.Strength++; break;
                        case 2: stats.Vitality++; break;
                        case 3: stats.Intelligence++; break;
                        case 4: stats.Dexterity++; break;
                        case 5: stats.Luck++; break;
                    }
                    availablePoints--;
                }
                else if (input == "0")
                {
                    if (availablePoints > 0)
                    {
                        Console.WriteLine("[ERROR] You must spend all points before finishing! Press any key...");
                        Console.ReadKey();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
