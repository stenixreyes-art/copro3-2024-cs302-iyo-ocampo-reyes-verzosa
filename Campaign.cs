using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Resources;
using System.Text;

namespace Morgan_Thieves
{
    internal class Campaign
    {
       public static void displayStory()
        {
            PixelBook.createBook();

            PixelBook.displayText("========== StoryLine ===========",
                @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Lore\Intro.txt");
            PixelArt.printArt(7, 3, @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Art\Sea.txt");
            userPrompt();

            PixelBook.displayText("=========== MerFolk ============",
                @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Lore\Merfolk.txt");
            PixelArt.printArt(16, 4, @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Art\Tail.txt");
            userPrompt();

            PixelArt.printArt(11, 5, @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Art\Wings.txt");
            PixelBook.displayText("=========== BirdFolk ============",
                @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Lore\Birdfolk.txt");
            userPrompt();

            PixelBook.displayText("=========== Human ============",
                @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Lore\Human.txt");
            PixelArt.printArt(11, 4, @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Art\Hand.txt");
            userPrompt();

            PixelBook.displayText("========== StoryLine ===========",
                @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Lore\Intro2.txt");
            PixelArt.printArt(7, 3, @"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Art\Sea.txt");
            userPrompt();

            return;
        }

        public static void userPrompt() 
        {
            string prompt = "--- Press any key to continue ---";
            byte leftPadding = (byte)((Console.WindowWidth - prompt.Length) / 2);

            Console.SetCursorPosition(leftPadding, 28);
            Console.Write(prompt);
            Console.ReadKey(true);
            PixelBook.createInnerPages(22, 34);
        }
    }

    public class PixelBook
    { 
        public static void createBook() // An orchestrator method that creates entire book
        {
            Console.Clear();
            createBackgroundBase(26, 92);
            createLayout();
            createInnerPages(22, 34);
            createMiddleColor(26);          
        }

        public static void createBackgroundBase(byte height, byte width)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            byte leftOffset = (byte)((Console.WindowWidth - width) / 2);
            byte topOffset = 1;

            for(byte i = 0; i < height; i++)
            {   
                Console.WriteLine("");
                Console.SetCursorPosition(leftOffset, Console.CursorTop);

                for (byte j = 0; j < width; j++)
                {
                    Console.Write(" ");
                }
            }
            Console.ResetColor();
        }

        public static void createLayout()
        {            

            string[] leftPage = File.ReadAllLines(@"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Art\leftPage.txt");
            string[] rightPage = File.ReadAllLines(@"C:\Users\Okuram\Downloads\Morgan Thievesaiyo\Morgan Thieves\Resources\Art\rightPage.txt");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;

            byte leftOffset = (byte)(((Console.WindowWidth - leftPage[0].Length) / 2) - 23);
            Console.CursorTop = 2;

            for (byte i = 0; i < leftPage.Length; i++)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.WriteLine(leftPage[i] + "     " + rightPage[i]);
            }            
            Console.ResetColor();
        }

        public static void createInnerPages(byte height, byte width)
        {
            byte leftOffset = (byte) (((Console.WindowWidth - width) / 2) - 23);
            Console.CursorTop = 2;

            Console.BackgroundColor = ConsoleColor.Yellow;

            for (byte i = 0; i < height; i++)
            {
                Console.WriteLine();
                Console.SetCursorPosition(leftOffset, Console.CursorTop);

                for (byte j = 0; j < width; j++)
                {
                    Console.Write(" ");
                }

                Console.SetCursorPosition(Console.CursorLeft + 13, Console.CursorTop);
                for (byte k = 0; k < width; k++)
                {
                    Console.Write(" ");
                }
            }
            Console.ResetColor();
        }

        public static void createMiddleColor(byte height)
        {
            byte leftOffset = (byte) ((Console.WindowWidth / 2) - 3);           

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.CursorTop = 0;

            for (byte i = 0; i < height; i++) {
                Console.WriteLine();
                Console.SetCursorPosition(leftOffset, Console.CursorTop);

                for  (byte j = 0; j < 7; j++)
                {
                    if ((i == 1 || i == 3 || i == 22 || i == 24) && (j == 0 || j == 6))
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        continue;
                    }
                    Console.Write("▒");
                }
            }
            Console.ResetColor();
        }

        public static void displayText(string header, string fileName)
        {
            var text = TextFormatter.processText(fileName);
 
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;

            byte leftOffset = (byte) (((Console.WindowWidth - 32) / 2) - 23);
            byte numOfLines = (byte) text.Count();

            for (byte i = 0; i < numOfLines; i++)
            {
                if (i == 0)
                {
                    Console.SetCursorPosition(leftOffset, 4);
                    Console.WriteLine(header);                    
                }
                Console.SetCursorPosition(leftOffset, 5 + i);
                Console.WriteLine(text.Dequeue());
            }
            Console.ResetColor();
        }
        public static void printGuide()
        {
            Console.SetCursorPosition(0, 0);

            int leftPadding = Console.WindowWidth / 2;
            for (int i = 1; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(leftPadding, Console.CursorTop);
                Console.WriteLine("W");
            }
        }
    }

    // formats the text
    public class TextFormatter
    {
        public static Queue<string> processText(string fileName) // Orchestor method for formatting texts
        {
            string text = fileReader(fileName); // gets the text from text files

            var wordsQueue = splitWords(text); // split the text into a queue of words

            var formattedLines = formatText(wordsQueue); // combine the words into a formatted line

            return formattedLines;
        }
        
        public static string fileReader(string fileName)
        {
            string text = File.ReadAllText(fileName);
            return text;
        }

        public static Queue<string> splitWords(string text)
        {
            Queue<string> wordsQueue = new Queue<string>();

            string[] arrayOfWords = text.Split(' ', '\n', '\r');

            foreach (string word in arrayOfWords)
            {
                wordsQueue.Enqueue(word + " ");
            }
            return wordsQueue;
        }

        public static Queue<string> formatText(Queue<string> qWords)
        {
            StringBuilder line = new StringBuilder();
            Queue<string> formattedLines = new Queue<string>();

            while (qWords.Count > 0)
            {
                string word = qWords.Dequeue();
                byte lineCharCount = (byte) (word.Length + line.Length);

                if (lineCharCount <= 33) { line.Append(word); }
                else
                {
                    byte padding = (byte) (33 - line.Length);
                    for (byte i = 0; i < padding; i++)
                    {
                        line.Append(" ");
                    }

                    formattedLines.Enqueue(line.ToString());

                    line.Clear();
                    line.Append(word);
                }
            }
            formattedLines.Enqueue(line.ToString()); // enters the last line into the queue.
            return formattedLines;
        }

    }

    public class PixelArt
    {
        public static string[] readArt(string fileName)
        {
            string[] pixelArt = File.ReadAllLines(fileName);
            return pixelArt;
        }

        public static void printArt(byte leftOffset, byte topOffset, string fileName)
        {
            string[] pixelArt = readArt(fileName);
            byte leftPadding = (byte)((Console.WindowWidth / 2) + leftOffset);

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;
            for (byte i = 0; i < pixelArt.Length; i++)
            {
                Console.SetCursorPosition(leftPadding, topOffset + i);
                Console.WriteLine(pixelArt[i]);
            }
            Console.ResetColor();
        }

    }

}