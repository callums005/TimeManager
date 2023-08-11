/*
 *  -- Project --
 *  Project: TimeManager  
 *  File: Output.cs
 *  Date: 11-08-2023 (DD-MM-YYYY)
 *  
 *  -- Credit --
 *  Name: Callum D Stevenson
 *  Position: Creator
 */

namespace TimeManager
{
    internal class Output
    {
        public static void CreateTitle(string title, bool doNotCentre = false, ConsoleColor background = ConsoleColor.Blue, ConsoleColor foreground = ConsoleColor.White)
        {
            // Sets the console background
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;

            // If not to centre text then print half of the bufferwidth minus the text length
            if (!doNotCentre)
                for (int i = 0; i < ((Console.BufferWidth - title.Length) / 2); i++)
                    Console.Write(" ");

            // Print the text
            Console.Write(title);

            // Print half of the bufferwidth minus the text length
            for (int i = 0; i < (!doNotCentre ? ((Console.BufferWidth - title.Length) / 2) : (Console.BufferWidth - title.Length)); i++)
                Console.Write(" ");

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
