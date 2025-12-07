namespace Morgan_Thieves
{
    public class Flag : INTERFACE
    {
        public string backgroundColor { get; set; } = "Black";
        public string flagSymbol { get; set; } = "Skulls";
        public string symbolColor { get; set; } = "White";

        public void Display()
        {
            Console.WriteLine("---Flag");
            Console.WriteLine($"Background Color: {backgroundColor}");
            Console.WriteLine($"Symbol: {flagSymbol}");
            Console.WriteLine($"Symbol Color: {symbolColor}");
        }
    }
}