using System;

namespace Morgan_Thieves
{
    // Base class for POLYMORPHISM
    public abstract class Character
    {
        // Private backing fields (Encapsulation)
        private string name, gender, race, characterClass;
        private string eyeColor, eyeShape, hairStyle, hairColor, expression;
        private string height, bodyType, skinTone, physicalMark, tattoo;
        private string topClothing, botClothing, footwear, headgear, accessory;
        private bool isLoyal;

        // Public Properties using 'this' keyword
        public string _Name { get { return this.name; } set { this.name = value; } }
        public string _Gender { get { return this.gender; } set { this.gender = value; } }
        public string _Race { get { return this.race; } set { this.race = value; } }
        public string _CharacterClass { get { return this.characterClass; } set { this.characterClass = value; } }
        public string _EyeShape { get { return this.eyeShape; } set { this.eyeShape = value; } }
        public string _EyeColor { get { return this.eyeColor; } set { this.eyeColor = value; } }
        public string _HairStyle { get { return this.hairStyle; } set { this.hairStyle = value; } }
        public string _HairColor { get { return this.hairColor; } set { this.hairColor = value; } }
        public string _Expression { get { return this.expression; } set { this.expression = value; } }
        public string _Height { get { return this.height; } set { this.height = value; } }
        public string _BodyType { get { return this.bodyType; } set { this.bodyType = value; } }
        public string _SkinTone { get { return this.skinTone; } set { this.skinTone = value; } }
        public string _PhysicalMark { get { return this.physicalMark; } set { this.physicalMark = value; } }
        public string _Tattoo { get { return this.tattoo; } set { this.tattoo = value; } }
        public string _TopClothing { get { return this.topClothing; } set { this.topClothing = value; } }
        public string _BotClothing { get { return this.botClothing; } set { this.botClothing = value; } }
        public string _Footwear { get { return this.footwear; } set { this.footwear = value; } }
        public string _Headgear { get { return this.headgear; } set { this.headgear = value; } }
        public string _Accessory { get { return this.accessory; } set { this.accessory = value; } }

        public bool _IsLoyal { get { return isLoyal; } set { isLoyal = value; } }

        // Complex objects
        public Flag flag = new Flag();
        public CharacterStats stats = new CharacterStats();
        public PirateInfo pirateInfo;
        public Character()
        {
            eyeColor = "Black";
            eyeShape = "Droopy";
            hairStyle = "Short Hair";
            hairColor = "Black";
            expression = "Neutral";
            height = "Tall";
            bodyType = "Average";
            skinTone = "Pale";
            physicalMark = "None";
            tattoo = "None";
            topClothing = "Topless";
            botClothing = "Pants";
            footwear = "Barefoot";
            headgear = "None";
            accessory = "None";
            pirateInfo = new PirateInfo(1, DateTime.Now);
            _IsLoyal = false;
        }

        // Virtual method - can be overridden by derived classes
        public virtual void displayCharacterSummary()
        {
            Console.Write("\x1b[3J");
            Console.Clear();
            Console.WriteLine("--- Character Summary ---");
            displayBackground();
            pirateInfo.Display();
            displayLoyaltyStatus(isLoyal);
            displayCharactereStats();
            displayPhysicalAttributes();
            displayGarments();
            flag.Display();
            Console.WriteLine("------");
            Console.WriteLine("Please press any key to Continue");
            Console.ReadKey();
        }

        // Virtual methods for different sections 
        public virtual void displayBackground()
        {
            Console.WriteLine($"\n---Background\n" +
                            $"Name: {name}\n" +
                            $"Gender: {gender}\n" +
                            $"Race: {race}\n" +
                            $"Class: {characterClass}");
        }

        public void displayCharactereStats()
        {
            Console.WriteLine($"\n---Character Stats\n" +
                            $"Strength: {stats.Strength}\n" +
                            $"Vitality: {stats.Vitality}\n" +
                            $"Intelligence: {stats.Intelligence}\n" +
                            $"Dexterity: {stats.Dexterity}\n" +
                            $"Luck: {stats.Luck}");
        }

        public virtual void displayPhysicalAttributes()
        {
            Console.WriteLine($"\n---Physical Attributes\n" +
                            $"Eye Color: {eyeColor}\n" +
                            $"Eye Shape: {eyeShape}\n" +
                            $"Hairstyle: {hairStyle}\n" +
                            $"Hair Color: {hairColor}\n" +
                            $"Expression: {expression}\n" +
                            $"Height: {height}\n" +
                            $"Body Type: {bodyType}\n" +
                            $"Skin Tone: {skinTone}\n" +
                            $"Physical Markings: {physicalMark}\n" +
                            $"Tattoo: {tattoo}");
        }

        public virtual void displayGarments()
        {
            Console.WriteLine($"\n---Garments & Accessories\n" +
                            $"Top Clothing: {topClothing}\n" +
                            $"Bottom Clothing: {botClothing}\n" +
                            $"Footwear: {footwear}\n" +
                            $"Headgear: {headgear}\n" +
                            $"Accessory: {accessory}");
        }
        public void displayLoyaltyStatus(bool isLoyal)
        {
            Console.WriteLine("\n---Allegience");
            if (isLoyal) {
                Console.WriteLine("Loyalty: Privateer\n" +
                    "Perk: Government-issued map");
            } else
            {
                Console.WriteLine("\nLoyalty: Outlaw Pirate\n" +
                    "Perk: High risk pirate contracts");
            }
        }
        
        // Abstract method for special abilities - to be overridden by race-specific classes
        public abstract string getSpecialAbility();

        // Abstract method for race description
        public abstract string getRaceDescription();

    }
}


namespace Morgan_Thieves
{
    // POLYMORPHISM - Derived class that overrides base class methods
    public class HumanCharacter : Character
    {
        public HumanCharacter() : base()
        {
            _Race = "Human";
        }

        // METHOD OVERRIDING - Overrides base class method
        public override string getSpecialAbility()
        {
            return "Adaptability: +20% learning speed for new skills";
        }

        public override string getRaceDescription()
        {
            return "Humans are resilient and ambitious, masters of innovation and technology.";
        }

        // Override to add race-specific information
        public override void displayBackground()
        {
            base.displayBackground();
            Console.WriteLine($"Special Ability: {getSpecialAbility()}");
            Console.WriteLine($"Description: {getRaceDescription()}");
        }

        public override void displayPhysicalAttributes()
        {
            Console.WriteLine($"\n---Physical Attributes (Human Traits)");
            base.displayPhysicalAttributes();
            Console.WriteLine($"Racial Bonus: Enhanced Tool Usage");
        }
    }
}

namespace Morgan_Thieves
{
    public class MerfolkCharacter : Character
    {
        public MerfolkCharacter() : base()
        {
            _Race = "Merfolk";
            // Merfolk-specific defaults
            _SkinTone = "Olive";
            _EyeColor = "Blue";
        }

        // METHOD OVERRIDING
        public override string getSpecialAbility()
        {
            return "Aquatic Mastery: Can breathe underwater and control water currents";
        }

        public override string getRaceDescription()
        {
            return "Merfolk are mysterious sea dwellers with telepathic abilities through water vibrations.";
        }

        public override void displayBackground()
        {
            base.displayBackground();
            Console.WriteLine($"Special Ability: {getSpecialAbility()}");
            Console.WriteLine($"Description: {getRaceDescription()}");
        }

        public override void displayPhysicalAttributes()
        {
            Console.WriteLine($"\n---Physical Attributes (Merfolk Traits)");
            base.displayPhysicalAttributes();
            Console.WriteLine($"Racial Bonus: Underwater Breathing & Water Manipulation");
        }
    }
}


namespace Morgan_Thieves
{
    public class BirdFolkCharacter : Character
    {
        public BirdFolkCharacter() : base()
        {
            _Race = "BirdFolk"; //defult values for BirdFolk
            _EyeColor = "Amber";
            _BodyType = "Slim";
        }

        // METHOD OVERRIDING
        public override string getSpecialAbility()
        {
            return "Aerial Superiority: Flight capability and enhanced vision";
        }

        public override string getRaceDescription()
        {
            return "BirdFolk are children of the wind, with wings, strength, and sight beyond the horizon.";
        }

        public override void displayBackground()
        {
            base.displayBackground();
            Console.WriteLine($"Special Ability: {getSpecialAbility()}");
            Console.WriteLine($"Description: {getRaceDescription()}");
        }

        public override void displayPhysicalAttributes()
        {
            Console.WriteLine($"\n---Physical Attributes (BirdFolk Traits)");
            base.displayPhysicalAttributes();
            Console.WriteLine($"Racial Bonus: Flight & Enhanced Vision");
        }
    }
}
