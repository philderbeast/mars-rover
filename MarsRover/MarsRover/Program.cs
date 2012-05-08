using System;
using System.IO;
using System.Text;

namespace MarsRover
{
    public class Program
    {
        static void Main(string[] args)
        {
            var fileName = args[0];
            if(!File.Exists(fileName))
            {
                Console.WriteLine("Run this program by passing a file name. This file should contain the inputs");
                Console.WriteLine("Search the web if you are unsure of the input format for the well known mars rover programming challenge.");
                return;
            }

            var input = File.ReadAllText(fileName);
            Console.Write(Run(input));
        }

        public static string Run(string input)
        {
            var sr = new StringReader(input);
            var gridMaxString = sr.ReadLine();

            Parser parser = new Parser();
            var gridMax = parser.MaxPosition(gridMaxString);

            var output = new StringBuilder();

            for (; ; )
            {
                var startAsString = sr.ReadLine();
                var movementsAsString = sr.ReadLine();
                if (string.IsNullOrEmpty(startAsString) || string.IsNullOrEmpty(movementsAsString))
                {
                    break;
                }

                var start = parser.Start(startAsString);
                var movements = parser.Movements(movementsAsString);
                var rover = new Rover(start.Position, start.Direction);
                rover = movements(rover);
                output.AppendFormat("{0}{1}", rover, Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
