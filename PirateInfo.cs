namespace Morgan_Thieves
{
    public struct PirateInfo
    {
        public byte Level;
        public DateTime DateCreated;

        public PirateInfo(byte level, DateTime dateCreated)
        {
            Level = level;
            DateCreated = dateCreated;
        }

        public void Display()
        {
            Console.WriteLine($"\nLevel: {Level}");
            Console.WriteLine($"Created: {DateCreated.ToString("MMMM dd, yyyy hh:mm tt")}");
        }
    }
}