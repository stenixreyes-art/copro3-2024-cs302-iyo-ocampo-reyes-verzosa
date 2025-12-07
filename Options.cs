namespace Morgan_Thieves
{
    internal class Options
    {
        // Choices
        public static string[] gender = { "Male", "Female", "Non-Binary" };
        public static string[] race = { "Human", "Merfolk", "Birdfolk" };
        public static string[] characterClass = { "Swashbuckler (Assassin)", "Bucaneer (Swordsman)", "Gunslinger (Ranger)", "Martial Arts (Fist Fighter)", "Witchdoctor (Sorcerer)" };
        public static string[] eyeColor = { "Black", "Brown", "Amber", "Blue", "Green" };
        public static string[] eyeShape = { "Almond", "Rounded", "Droopy", "Half-Lidded", "Up Turned" };
        public static string[] hairStyle = { "Bald", "Shorthair", "Longhair", "Braided", "Curly" };
        public static string[] hairColor = { "Black", "Blue", "Blonde", "Green", "Red" };
        public static string[] expression = { "Neutral", "Happy", "Sad", "Angry", "Playful" };
        public static string[] height = { "Very short", "Short", "Average", "Tall", "Very Tall" };
        public static string[] bodyType = { "Slim", "Average", "Large", "Muscular" };
        public static string[] skinTone = { "Pale", "Tanned", "Olive", "Bronze", "Ebony" };
        public static string[] physicalMark = { "None", "Facial Scars", "Stitched Wound", "Burn Mark", "Rope Burns" };
        public static string[] tattoo = { "None", "Jolly Roger", "Anchor", "Compass Rose", "Bleeding Heart" };
        public static string[] topClothing = { "Shirt", "Waistcoat", "Sleeveless Waistcoat", "Coat", "Topless" };
        public static string[] botClothing = { "Pants", "Knee-Breeches", "Long Breeches", "Loose Trouser", "Shorts" };
        public static string[] footwear = { "Boots", "Leather shoes", "Slippers", "Buckle shoes", "Barefoot" };
        public static string[] headGear = { "None", "Tricorn Hat", "Feathered Tricorn Hat", "Bandanna", "Head Scarf" };
        public static string[] accessory = { "None", "Earrings", "Eye Patch", "Necklace", "Hook" };
        public static string[] backgroundColorFlag = { "Black", "White", "Blue", "Red", "Yellow" };
        public static string[] flagSymbol = { "Skulls", "Guns", "Cross Cutlass", "Bleeding Heart", "X" };
        public static string[] flagSymbolColor = { "Black", "White", "Blue", "Red", "Yellow" };

        // METHOD OVERLOADING - Multiple versions of promptSelection

        // Version 1: Basic prompt without current choice (for initial selection)
        public static string promptSelection(string question, string[] options)
        {
            return promptSelection(question, options, "Not Set", false);
        }

        // Version 2: Prompt with current choice displayed
        public static string promptSelection(string prompt, string[] options, string currentChoice)
        {
            return promptSelection(prompt, options, currentChoice, true);
        }

        // Version 3: Most detailed version with control over showing current choice
        public static string promptSelection(string prompt, string[] options, string currentChoice, bool showCurrentChoice)
        {
            while (true)
            {
                Console.Clear();
                if (showCurrentChoice)
                {
                    Console.WriteLine(prompt + $" (Currently: {currentChoice})");
                }
                else
                {
                    Console.WriteLine(prompt);
                }

                for (byte i = 1; i <= options.Length; i++)
                {
                    Console.WriteLine($"[{i}] {options[i - 1]}");
                }
                Console.WriteLine("\n[0] Back");
                Console.Write("-------\n[]: ");

                string input = Console.ReadLine();

                if (byte.TryParse(input, out byte choice) && choice >= 0 && choice <= options.Length)
                {
                    if (choice == 0)
                    {
                        return showCurrentChoice ? currentChoice : "0";
                    }
                    else
                    {
                        return options[choice - 1];
                    }
                }

                Console.WriteLine("Error: Invalid Input!!\n---Please press a key to try again.");
                Console.ReadKey();
            }
        }

        // Version 4: Overloaded with custom error message
        public static string promptSelection(string prompt, string[] options, string currentChoice, string errorMessage)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(prompt + $" (Currently: {currentChoice})");

                for (byte i = 1; i <= options.Length; i++)
                {
                    Console.WriteLine($"[{i}] {options[i - 1]}");
                }
                Console.WriteLine("\n[0] Back");
                Console.Write("-------\n[]: ");

                string input = Console.ReadLine();

                if (byte.TryParse(input, out byte choice) && choice >= 0 && choice <= options.Length)
                {
                    if (choice == 0)
                    {
                        return currentChoice;
                    }
                    else
                    {
                        return options[choice - 1];
                    }
                }

                Console.WriteLine(errorMessage);
                Console.ReadKey();
            }
        }

        // Backward compatibility method
        public static string promptBackgroundSelection(string question, string[] options)
        {
            return promptSelection(question, options, "0", false);
        }
    }
}