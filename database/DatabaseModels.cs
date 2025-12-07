using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Morgan_Thieves.database
{
    // Database entity for saving characters
    [Table("Characters")]
    public class CharacterEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Background
        [Required]
        [MaxLength(14)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string Race { get; set; }

        [Required]
        [MaxLength(50)]
        public string CharacterClass { get; set; }

        // Head Features
        [MaxLength(20)]
        public string EyeColor { get; set; }

        [MaxLength(20)]
        public string EyeShape { get; set; }

        [MaxLength(20)]
        public string HairStyle { get; set; }

        [MaxLength(20)]
        public string HairColor { get; set; }

        [MaxLength(20)]
        public string Expression { get; set; }

        // Body Features
        [MaxLength(20)]
        public string Height { get; set; }

        [MaxLength(20)]
        public string BodyType { get; set; }

        [MaxLength(20)]
        public string SkinTone { get; set; }

        [MaxLength(30)]
        public string PhysicalMark { get; set; }

        [MaxLength(30)]
        public string Tattoo { get; set; }

        // Clothing
        [MaxLength(30)]
        public string TopClothing { get; set; }

        [MaxLength(30)]
        public string BotClothing { get; set; }

        [MaxLength(30)]
        public string Footwear { get; set; }

        [MaxLength(30)]
        public string Headgear { get; set; }

        [MaxLength(30)]
        public string Accessory { get; set; }

        // Flag
        [MaxLength(20)]
        public string FlagBackgroundColor { get; set; }

        [MaxLength(30)]
        public string FlagSymbol { get; set; }

        [MaxLength(20)]
        public string FlagSymbolColor { get; set; }

        // Stats
        public byte Strength { get; set; }
        public byte Vitality { get; set; }
        public byte Intelligence { get; set; }
        public byte Dexterity { get; set; }
        public byte Luck { get; set; }

        // Pirate Info
        public byte Level { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        // Loyaly
        [Required]
        public bool isLoyal { get; set; }
    }
}