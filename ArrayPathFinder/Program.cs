using LinearArrayPathFinder;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ArrayPathFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathFinder = new PathFinder();

            while (!Console.KeyAvailable)
            {
                Console.WriteLine("Enter array of int separated by spaces, e.g.: 1 2 0 3 0 2 0\nEnter q to quit");
                Console.Write(">");
                var line = Console.ReadLine();
                if (line == "q") break;

                var ints = line.Split(" ");
                var input = new List<int>();
                foreach (var number in ints)
                {
                    var isInt = int.TryParse(number.Trim(), out var intNumber);
                    if (isInt)
                    {
                        input.Add(intNumber);
                    }
                    else
                    {
                        Console.WriteLine($"Skipped array item {number} because it is a not valid int.");
                    }
                }
                var result = pathFinder.Find(input.ToArray());
            }
        }
    }
}
