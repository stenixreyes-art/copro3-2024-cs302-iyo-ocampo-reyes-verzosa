using System;

namespace Morgan_Thieves
{
    internal class Credits
    {
        public static void displayCredits()
        {
            Console.Clear();
            Console.WriteLine("--- Credits ---\n");
            Console.WriteLine("--- Angelos Delos Santos Iyo ---\n\n" +
                @"A member who masters the database. A flexible person that do any side tasks na ibigay sa kanya. 
                  The one willing to do any tasks regardless of difficulty at kalagayan." +
                "\n\n--- Paulo Verzosa ---\n\n" +
                @"A member na Malala magpuyat. Isang buong madaling araw lang ang flowchart sa kanya. 
                  Has a fatherly personality na nangangasiwa saming mga ginagawa sa bawat tasks namin. Has a wide insight about the process of our game." +
                "\n\n--- Steven Reyes ---\n\n" +
                @"A member na malalim ang kaalaman sa documentation, bagamat mas bago pa siya sa coding. 
                  The one compiles the reminders and important tasks that need tapusin. Has a wide knowledge when it comes to storytelling." +
                "\n\n--- Joshua Marco Ocampo ---\n\n" +
                @"A group leader who completes the needed code for the game. A silent task killer, di mo na mamalayan na tapos na gumawa ng agenda.  
                  He has an understanding personality; kahit wala kang gagawin, okay lang. Code Master ng group namin.");
            Console.WriteLine("\n---Press any key to go back to main menu");
            Console.ReadKey();
        }
    }
}