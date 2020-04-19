using System;

namespace Models
{
    public class PathFinderResult
    {
        public bool HasValidPath { get; set; }
        public string Input { get; set; }
        public string MostEfficientPath { get; set; }

        public void ConsolePrint()
        {
            Console.WriteLine($"Input path: {Input}");
            Console.WriteLine("End is " + (HasValidPath ? $"reachable!\nPath taken: {MostEfficientPath}\n" : "not reachable..\n"));
        }
    }
}