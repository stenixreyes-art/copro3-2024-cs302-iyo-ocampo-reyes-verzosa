namespace Morgan_Thieves.database
{
    public class CharacterRepository
    {
        private static PirateGameContext GetContext()
        {
            return new PirateGameContext(Program._dbOptions);
        }

        public static bool SaveCharacter(Character character, int? id = null)
        {
            try
            {
                using var context = GetContext();
                CharacterEntity entity;

                if (id.HasValue)
                {
                    // UPDATE: Find existing
                    entity = context.Characters.Find(id.Value);
                    if (entity == null)
                    {
                        Console.WriteLine("Error: Character ID not found for update.");
                        return false;
                    }
                }
                else
                {
                    // INSERT: New entity
                    entity = new CharacterEntity();
                    context.Characters.Add(entity);
                }

                // Map fields
                entity.Name = character._Name;
                entity.Gender = character._Gender;
                entity.Race = character._Race;
                entity.CharacterClass = character._CharacterClass;
                entity.EyeColor = character._EyeColor;
                entity.EyeShape = character._EyeShape;
                entity.HairStyle = character._HairStyle;
                entity.HairColor = character._HairColor;
                entity.Expression = character._Expression;
                entity.Height = character._Height;
                entity.BodyType = character._BodyType;
                entity.SkinTone = character._SkinTone;
                entity.PhysicalMark = character._PhysicalMark;
                entity.Tattoo = character._Tattoo;
                entity.TopClothing = character._TopClothing;
                entity.BotClothing = character._BotClothing;
                entity.Footwear = character._Footwear;
                entity.Headgear = character._Headgear;
                entity.Accessory = character._Accessory;
                entity.FlagBackgroundColor = character.flag.backgroundColor;
                entity.FlagSymbol = character.flag.flagSymbol;
                entity.FlagSymbolColor = character.flag.symbolColor;
                entity.Strength = character.stats.Strength;
                entity.Vitality = character.stats.Vitality;
                entity.Intelligence = character.stats.Intelligence;
                entity.Dexterity = character.stats.Dexterity;
                entity.Luck = character.stats.Luck;
                entity.isLoyal = character._IsLoyal;

                // Only update creation date if it's new
                if (!id.HasValue)
                {
                    entity.DateCreated = character.pirateInfo.DateCreated;
                    entity.Level = character.pirateInfo.Level;
                }

                entity.LastModified = DateTime.Now;

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving character: {ex.Message}");
                if (ex.InnerException != null) Console.WriteLine($"Inner: {ex.InnerException.Message}");
                return false;
            }
        }

        public static List<CharacterEntity> GetAllCharacters()
        {
            try
            {
                using var context = GetContext();
                return context.Characters.OrderByDescending(c => c.LastModified).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading characters: {ex.Message}");
                return new List<CharacterEntity>();
            }
        }

        public static Character LoadCharacter(int id)
        {
            try
            {
                using var context = GetContext();
                var entity = context.Characters.Find(id);
                if (entity == null) return null;

                Character character;
                switch (entity.Race)
                {
                    case "Human": character = new HumanCharacter(); break;
                    case "Merfolk": character = new MerfolkCharacter(); break;
                    case "Birdfolk": character = new BirdFolkCharacter(); break;
                    default: character = new HumanCharacter(); break;
                }

                character._Name = entity.Name;
                character._Gender = entity.Gender;
                character._Race = entity.Race;
                character._CharacterClass = entity.CharacterClass;
                character._EyeColor = entity.EyeColor;
                character._EyeShape = entity.EyeShape;
                character._HairStyle = entity.HairStyle;
                character._HairColor = entity.HairColor;
                character._Expression = entity.Expression;
                character._Height = entity.Height;
                character._BodyType = entity.BodyType;
                character._SkinTone = entity.SkinTone;
                character._PhysicalMark = entity.PhysicalMark;
                character._Tattoo = entity.Tattoo;
                character._TopClothing = entity.TopClothing;
                character._BotClothing = entity.BotClothing;
                character._Footwear = entity.Footwear;
                character._Headgear = entity.Headgear;
                character._Accessory = entity.Accessory;

                character.flag.backgroundColor = entity.FlagBackgroundColor;
                character.flag.flagSymbol = entity.FlagSymbol;
                character.flag.symbolColor = entity.FlagSymbolColor;

                character.stats.Strength = entity.Strength;
                character.stats.Vitality = entity.Vitality;
                character.stats.Intelligence = entity.Intelligence;
                character.stats.Dexterity = entity.Dexterity;
                character.stats.Luck = entity.Luck;

                character.pirateInfo = new PirateInfo(entity.Level, entity.DateCreated);
                character._IsLoyal = entity.isLoyal;

                return character;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading character: {ex.Message}");
                return null;
            }
        }

        public static bool DeleteCharacter(int id)
        {
            try
            {
                using var context = GetContext();
                var entity = context.Characters.Find(id);
                if (entity == null) return false;

                context.Characters.Remove(entity);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting character: {ex.Message}");
                return false;
            }
        }
    }
}
