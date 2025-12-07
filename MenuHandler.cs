using System.Text.RegularExpressions;
using Morgan_Thieves.database;

namespace Morgan_Thieves
{
    internal class MenuHandler
    {
        static bool isCharacterCreationDone = false;

        // Track if race was changed during update to force stat reset
        static bool hasRaceChanged = false;

        public static void runCharacterCreation(Character? currentCharacter, int? dbId = null)
        {
            isCharacterCreationDone = false;
            hasRaceChanged = false;

            if (currentCharacter != null && dbId.HasValue)
            {
                // UPDATE MODE
                DisplayCharacterMenu(ref currentCharacter, dbId);
            }
            else
            {
                // NEW GAME MODE
                // We pass ref just in case, though usually null here
                AskForRace(ref currentCharacter, dbId);
            }
        }

        // Changed signature to 'ref' because changing Race creates a NEW object instance
        public static void AskForRace(ref Character currentCharacter, int? dbId)
        {
            bool selectionMade = false;
            while (!selectionMade)
            {
                Console.Clear();
                Console.WriteLine("--- Character Creation: Race ---");

                string selectedRace = Options.promptSelection("Choose your Race:", Options.race);

                // Preserve old data if we are switching races
                string oldName = currentCharacter?._Name;
                string oldGender = currentCharacter?._Gender;
                string oldClass = currentCharacter?._CharacterClass;

                switch (selectedRace)
                {
                    case "0": return;
                    case "Human": currentCharacter = new HumanCharacter(); break;
                    case "Merfolk": currentCharacter = new MerfolkCharacter(); break;
                    case "Birdfolk": currentCharacter = new BirdFolkCharacter(); break;
                }

                // If we were updating an existing character, restore basic info
                if (oldName != null)
                {
                    currentCharacter._Name = oldName;
                    currentCharacter._Gender = oldGender;
                    currentCharacter._CharacterClass = oldClass;
                    hasRaceChanged = true; // Flag this for the stat allocator later
                }

                selectionMade = true;

                // Flow Control: If Update Mode, return to Menu. If New Game, continue to Name.
                if (dbId.HasValue)
                {
                    return;
                }
                else
                {
                    AskForName(currentCharacter, dbId);
                    isCharacterCreationDone = true;
                }
            }
        }

        public static void AskForName(Character currentCharacter, int? dbId)
        {
            while (!isCharacterCreationDone)
            {
                Console.Clear();
                Console.Write("--- Enter Character Name ---\nRules: No spaces, max 2 numbers, no special characters, length 6-14.\nType 0 to return.\n[]: ");
                string input = Console.ReadLine();

                if (input.Equals("0")) { return; }

                if (IsValidName(input))
                {
                    currentCharacter._Name = input;
                    AskForGender(currentCharacter, dbId);
                    return;
                }
            }
        }

        public static void AskForGender(Character currentCharacter, int? dbId)
        {
            Console.Clear();
            Console.WriteLine("--- Character Creation ---");
            string input = Options.promptSelection("Choose a Gender: ", Options.gender);

            if (input.Equals("0")) { return; }

            currentCharacter._Gender = input;
            AskForClass(currentCharacter, dbId);
        }

        public static void AskForClass(Character currentCharacter, int? dbId)
        {
            Console.Clear();
            Console.WriteLine("--- Character Creation ---");
            string input = Options.promptSelection("Choose a Class: ", Options.characterClass);

            if (input.Equals("0")) { return; }

            currentCharacter._CharacterClass = input;
            DisplayCharacterMenu(ref currentCharacter, dbId);
        }
                
        public static void DisplayCharacterMenu(ref Character currentCharacter, int? dbId)
        {
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                string headerInfo = (currentCharacter != null)
                    ? $"{currentCharacter._Name} | {currentCharacter._Race} | {currentCharacter._CharacterClass}"
                    : "Unknown";

                Console.WriteLine($"--- Character Menu [ {headerInfo} ] ---");
                Console.Write(
                    "[1] Edit Appearance (Head)\n" +
                    "[2] Edit Body\n" +
                    "[3] Garments & Accessories\n" +
                    "[4] Edit Flag\n" +
                    "[5] Change Race\n" + // New Option
                    "[6] View Stats\n" +  // New Option
                    "\n" +
                    "[7] View Full Summary\n" +
                    "[8] Finish & Save\n" +
                    "[0] Back\n" +
                    "-------\n" +
                    "[]: ");

                string choice = Console.ReadLine();

                if (byte.TryParse(choice, out byte num))
                {
                    try
                    {
                        switch (num)
                        {
                            case 0: inMenu = false; return;
                            case 1: DisplayHeadMenu(currentCharacter); break;
                            case 2: DisplayBodyMenu(currentCharacter); break;
                            case 3: DisplayGarmentsMenu(currentCharacter); break;
                            case 4: DisplayFlagMenu(currentCharacter); break;
                            case 5:
                                // Change Race Logic
                                AskForRace(ref currentCharacter, dbId);
                                break;
                            case 6:
                                // REQ: Display Stats independently
                                Console.Clear();
                                currentCharacter.stats.DisplayStats();
                                Console.WriteLine("\nPress any key to return...");
                                Console.ReadKey();
                                break;
                            case 7: currentCharacter.displayCharacterSummary(); break;
                            case 8: // FINISH
                                if (string.IsNullOrEmpty(currentCharacter._Name))
                                    throw new InvalidPirateException("Cannot finish! Character must have a name!");

                                inMenu = false; // Break loop
                                isCharacterCreationDone = true;

                                // --- STAT ALLOCATION LOGIC ---
                                bool runAllocator = true;

                                if (dbId.HasValue) // UPDATE MODE
                                {
                                    Console.Clear();
                                    Console.WriteLine("--- Update Stats ---");

                                    // If race changed, we MUST reset.
                                    if (hasRaceChanged)
                                    {
                                        Console.WriteLine("Race changed! Stats have been reset to racial defaults automatically.");
                                        ResetStatsToRaceBase(currentCharacter);
                                        runAllocator = true;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Do you want to re-roll your stats? (WARNING: Current stats will be RESET)");
                                        Console.WriteLine("[Y] Yes (Reset & Re-allocate)");
                                        Console.WriteLine("[N] No (Keep existing stats)");
                                        Console.Write("[]: ");
                                        string statChoice = Console.ReadLine()?.ToUpper();

                                        if (statChoice == "N")
                                        {
                                            runAllocator = false; // Keep DB values
                                        }
                                        else
                                        {
                                            ResetStatsToRaceBase(currentCharacter);
                                            Console.WriteLine("Stats reset to racial defaults.");
                                        }
                                    }
                                }
                                 
                                
                                if (runAllocator)
                                {                                    
                                    StatAllocator statsAllocator = new StatAllocator();
                                    // Optionally allow user to choose mode, or default to one
                                    if (statsAllocator.chooseStatAllocationMode())
                                        statsAllocator.RunAutomaticAllocation(currentCharacter.stats);
                                    else
                                        statsAllocator.RunAllocation(currentCharacter.stats);
                                    AskForAllegiance(currentCharacter);
                                }
                                // -----------------------------

                                // Final Summary before Save
                                currentCharacter.displayCharacterSummary();

                                Console.Clear();
                                string actionType = dbId.HasValue ? "UPDATE" : "SAVE";
                                Console.WriteLine($"Are you sure you want to {actionType} the following info?");
                                Console.Write("Yes or No [Y/N]: ");
                                string saveChoice = Console.ReadLine()?.ToUpper();

                                if (saveChoice == "Y")
                                {
                                    Console.WriteLine($"\nProcessing {actionType}...");

                                    if (CharacterRepository.SaveCharacter(currentCharacter, dbId))
                                        Console.WriteLine($"✓ Character {actionType}D successfully!");
                                    else
                                        Console.WriteLine($"✗ Failed to {actionType} character.");

                                    Console.WriteLine("\nPress any key to continue...");
                                    Console.ReadKey();
                                }
                                return;
                        }
                    }
                    catch (InvalidPirateException ex)
                    {
                        Console.WriteLine($"\nERROR: {ex.Message}");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid Input!!");
                    Console.ReadKey();
                }
            }
        }

        // Helper to reset stats to base so we don't confound points
        private static void ResetStatsToRaceBase(Character character)
        {
            Character temp = null;
            // Create a fresh instance to get base stats defined in the class constructor
            switch (character._Race)
            {
                case "Human": temp = new HumanCharacter(); break;
                case "Merfolk": temp = new MerfolkCharacter(); break;
                case "Birdfolk": temp = new BirdFolkCharacter(); break;
                default: temp = new HumanCharacter(); break;
            }

            character.stats.Strength = temp.stats.Strength;
            character.stats.Vitality = temp.stats.Vitality;
            character.stats.Intelligence = temp.stats.Intelligence;
            character.stats.Dexterity = temp.stats.Dexterity;
            character.stats.Luck = temp.stats.Luck;
        }

        // ... [Rest of the helper methods (DisplayHeadMenu, IsValidName, etc.) remain unchanged] ...
        // Ensure you copy the rest of your existing display methods below here.
        public static void DisplayHeadMenu(Character currentCharacter)
        {
            bool inHeadMenu = true;
            while (inHeadMenu)
            {
                Console.Clear();
                Console.Write("--- Head Customization ---\n[1] Eye Color\n[2] Eye Shape\n[3] Hair Style\n[4] Hair Color\n[5] Expression\n[0] Back\n-------\n[]: ");
                string choice = Console.ReadLine();
                if (byte.TryParse(choice, out byte num))
                {
                    switch (num)
                    {
                        case 0: inHeadMenu = false; break;
                        case 1: currentCharacter._EyeColor = Options.promptSelection("Choose Eye Color:", Options.eyeColor, currentCharacter._EyeColor); break;
                        case 2: currentCharacter._EyeShape = Options.promptSelection("Choose Eye Shape:", Options.eyeShape, currentCharacter._EyeShape); break;
                        case 3: currentCharacter._HairStyle = Options.promptSelection("Choose Hair Style:", Options.hairStyle, currentCharacter._HairStyle); break;
                        case 4: currentCharacter._HairColor = Options.promptSelection("Choose Hair Color:", Options.hairColor, currentCharacter._HairColor); break;
                        case 5: currentCharacter._Expression = Options.promptSelection("Choose Expression:", Options.expression, currentCharacter._Expression); break;
                    }
                }
            }
        }

        public static void DisplayBodyMenu(Character currentCharacter)
        {
            bool inBodyMenu = true;
            while (inBodyMenu)
            {
                Console.Clear();
                Console.Write("--- Body Customization ---\n[1] Height\n[2] Body Type\n[3] Skin Tone\n[4] Physical Marks\n[5] Tattoo\n[0] Back\n-------\n[]: ");
                string choice = Console.ReadLine();
                if (byte.TryParse(choice, out byte num))
                {
                    switch (num)
                    {
                        case 0: inBodyMenu = false; break;
                        case 1: currentCharacter._Height = Options.promptSelection("Choose Height:", Options.height, currentCharacter._Height); break;
                        case 2: currentCharacter._BodyType = Options.promptSelection("Choose Body Type:", Options.bodyType, currentCharacter._BodyType); break;
                        case 3: currentCharacter._SkinTone = Options.promptSelection("Choose Skin Tone:", Options.skinTone, currentCharacter._SkinTone); break;
                        case 4: currentCharacter._PhysicalMark = Options.promptSelection("Choose Physical Mark:", Options.physicalMark, currentCharacter._PhysicalMark); break;
                        case 5: currentCharacter._Tattoo = Options.promptSelection("Choose Tattoo:", Options.tattoo, currentCharacter._Tattoo); break;
                    }
                }
            }
        }

        public static void DisplayGarmentsMenu(Character currentCharacter)
        {
            bool inGarmentsMenu = true;
            while (inGarmentsMenu)
            {
                Console.Clear();
                Console.Write("--- Garments & Accessories ---\n[1] Top Clothing\n[2] Bottom Clothing\n[3] Footwear\n[4] Headgear\n[5] Accessory\n[0] Back\n-------\n[]: ");
                string choice = Console.ReadLine();
                if (byte.TryParse(choice, out byte num))
                {
                    switch (num)
                    {
                        case 0: inGarmentsMenu = false; break;
                        case 1: currentCharacter._TopClothing = Options.promptSelection("Choose Top Clothing:", Options.topClothing, currentCharacter._TopClothing); break;
                        case 2: currentCharacter._BotClothing = Options.promptSelection("Choose Bottom Clothing:", Options.botClothing, currentCharacter._BotClothing); break;
                        case 3: currentCharacter._Footwear = Options.promptSelection("Choose Footwear:", Options.footwear, currentCharacter._Footwear); break;
                        case 4: currentCharacter._Headgear = Options.promptSelection("Choose Headgear:", Options.headGear, currentCharacter._Headgear); break;
                        case 5: currentCharacter._Accessory = Options.promptSelection("Choose Accessory:", Options.accessory, currentCharacter._Accessory); break;
                    }
                }
            }
        }

        public static void DisplayFlagMenu(Character currentCharacter)
        {
            bool inFlagMenu = true;
            while (inFlagMenu)
            {
                Console.Clear();
                Console.Write("--- Flag Customization ---\n[1] Background Color\n[2] Flag Symbol\n[3] Symbol Color\n[0] Back\n-------\n[]: ");
                string choice = Console.ReadLine();
                if (byte.TryParse(choice, out byte num))
                {
                    switch (num)
                    {
                        case 0: inFlagMenu = false; break;
                        case 1: currentCharacter.flag.backgroundColor = Options.promptSelection("Choose Background Color:", Options.backgroundColorFlag, currentCharacter.flag.backgroundColor); break;
                        case 2: currentCharacter.flag.flagSymbol = Options.promptSelection("Choose Flag Symbol:", Options.flagSymbol, currentCharacter.flag.flagSymbol); break;
                        case 3: currentCharacter.flag.symbolColor = Options.promptSelection("Choose Symbol Color:", Options.flagSymbolColor, currentCharacter.flag.symbolColor); break;
                    }
                }
            }
        }

        public static bool IsValidName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name)) throw new InvalidPirateException("Name cannot be empty!");
                if (name.Length < 6 || name.Length > 14) throw new InvalidPirateException("Name must be at least 6 to 14 characters long!");
                if (name.Contains(' ')) throw new InvalidPirateException("Name cannot contain spaces!");
                if (Regex.IsMatch(name, @"[^a-zA-Z0-9]")) throw new InvalidPirateException("Name cannot contain special characters!");
                int digitCount = name.Count(c => char.IsDigit(c));
                if (digitCount > 2) throw new InvalidPirateException("Name cannot contain more than 2 numbers!");
                return true;
            }
            catch (InvalidPirateException ex)
            {
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ReadKey();
                return false;
            }
        }

        public static void AskForAllegiance(Character character)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("--- Allegiance ---\n" +
                    $"Do you {character._Name} swear loyalty to the Crown? \n" +
                    "[1] Yes\n" +
                    "[2] No\n\n" +
                    "------\n" +
                    "[]: ");

                if (Byte.TryParse(Console.ReadLine(), out byte choice) && choice > 0 && choice < 3)
                {
                    character._IsLoyal = (choice == 1) ? true : false;
                    return;
                } else { 
                    Console.WriteLine("The Queen demands clarity! Answer properly.");
                    Console.ReadKey(true);    
                }
            }
        }
    }
}
