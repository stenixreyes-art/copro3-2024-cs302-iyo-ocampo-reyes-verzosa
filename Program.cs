using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Morgan_Thieves.database;

namespace Morgan_Thieves
{
    public class Program
    {
        public static DbContextOptions<PirateGameContext> _dbOptions;

        public static void Main()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PirateGameContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            });

            _dbOptions = optionsBuilder.Options;
            using (var context = new PirateGameContext(_dbOptions))
            {
                context.Database.EnsureCreated();
                Console.WriteLine("Database verified/created successfully.");
            }

            Character? currentCharacter = null;
            bool isRunning = true;

            Console.WriteLine("Server ready. Press ENTER to start the game client...");
            Console.ReadLine();
            LoadingScreen.Show();

            while (isRunning)
            {
                Console.Clear();
                Console.Write("=== Pirate Adventure Game ===\n" +
                    "--- Main Menu ---\n" +
                    "[1] New Game\n" +
                    "[2] Load Game\n" +
                    "[3] Campaign Mode\n" +
                    "[4] Credits\n" +
                    "[0] Exit\n" +
                    "-------\n" +
                    "[]: ");

                string choice = Console.ReadLine();

                if (byte.TryParse(choice, out byte num) && num >= 0 && num <= 4)
                {
                    switch (num)
                    {
                        case 0:
                            Console.Clear();
                            Console.WriteLine("Thank you for playing our game!!");
                            isRunning = false;
                            break;
                        case 1:
                            MenuHandler.runCharacterCreation(null, null);
                            break;
                        case 2:
                            HandleLoadGameMenu(ref currentCharacter);
                            break;
                        case 3:
                            Campaign.displayStory();
                            break;
                        case 4:
                            Credits.displayCredits();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid Input!!\n--- Please enter any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        private static void HandleLoadGameMenu(ref Character? currentCharacter)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Load Game ===\n");

                var savedCharacters = CharacterRepository.GetAllCharacters();

                if (savedCharacters.Count == 0)
                {
                    Console.WriteLine("No saved characters found.");
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Saved Characters:\n");
                for (int i = 0; i < savedCharacters.Count; i++)
                {
                    var c = savedCharacters[i];
                    Console.WriteLine($"[{i + 1}] {c.Name} - {c.Race} {c.CharacterClass} (Level {c.Level})");

                    //NOTE: Removed time, add unnecessary complexity. Region  specific time logic necessary (e.g UT+8:00 for SE Asia). Not part of scope.
                    //If added, time is not correct.

                    Console.WriteLine($"    Modified: {c.LastModified:MMM dd, yyyy}\n");
                }

                Console.Write("\nSelect character (0 to cancel): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int selection) && selection > 0 && selection <= savedCharacters.Count)
                {
                    var selectedEntity = savedCharacters[selection - 1];

                    Console.Clear();
                    Console.WriteLine($"=== Selected: {selectedEntity.Name} ===\n");
                    Console.WriteLine("[1] Play");
                    Console.WriteLine("[2] Delete");
                    Console.WriteLine("[3] Update");
                    Console.WriteLine("[0] Cancel");
                    Console.Write("\n[]: ");

                    string action = Console.ReadLine();

                    if (action == "1") // PLAY
                    {
                        currentCharacter = CharacterRepository.LoadCharacter(selectedEntity.Id);
                        if (currentCharacter != null)
                        {
                            Console.WriteLine($"\n✓ Loaded {currentCharacter._Name} successfully!");
                            Console.WriteLine("Press any key to view character...");
                            Console.ReadKey();
                            currentCharacter.displayCharacterSummary();
                            return;
                        }
                    }
                    else if (action == "2") // DELETE
                    {
                        Console.Write($"\nAre you sure you want to DELETE '{selectedEntity.Name}'? This cannot be undone. [Y/N]: ");
                        string confirm = Console.ReadLine()?.ToUpper();

                        if (confirm == "Y")
                        {
                            if (CharacterRepository.DeleteCharacter(selectedEntity.Id))
                                Console.WriteLine("\n✓ Character deleted.");
                            else
                                Console.WriteLine("\n✗ Failed to delete character.");
                            Console.ReadKey();
                        }
                    }
                    else if (action == "3") // UPDATE
                    {
                        Console.Write($"\nAre you sure you want to UPDATE '{selectedEntity.Name}'? [Y/N]: ");
                        string confirm = Console.ReadLine()?.ToUpper();

                        if (confirm == "Y")
                        {
                            var charToUpdate = CharacterRepository.LoadCharacter(selectedEntity.Id);
                            if (charToUpdate != null)
                            {
                                MenuHandler.runCharacterCreation(charToUpdate, selectedEntity.Id);
                            }
                            else
                            {
                                Console.WriteLine("\n✗ Error loading character data for update.");
                                Console.ReadKey();
                            }
                        }
                    }
                }
                else if (input == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                    Console.ReadKey();
                }
            }
        }
    }
}
