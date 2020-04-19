using System;
using System.Collections.Generic;
using LinearArrayPathFinder;
using Microsoft.Extensions.DependencyInjection;
using Models;
using SavedResultManager;

namespace ArrayPathFinderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dependency injection
            var services = new ServiceCollection();
            services.AddSingleton<IResultManager, ResultManager>();
            DependencyResolver.GetInstance = () => services.BuildServiceProvider();

            var resultManager = DependencyResolver.GetService<IResultManager>();
            var pathFinder = new PathFinder();

            while (!Console.KeyAvailable)
            {
                Console.WriteLine("Enter array of int separated by spaces, e.g.: 1 2 0 3 0 2 0");
                Console.WriteLine("Enter \"h\" to host WebApi server.");
                Console.WriteLine("Enter \"q\" to quit.");
                Console.WriteLine("Enter \"l\" to print all saved results.");
                Console.WriteLine("Enter \"l -id [id]\" to print saved result by id, e.g.: \"l -id 1\"");
                Console.WriteLine("Enter \"l -input [input]\" to print saved result by input, e.g.: \"l -input 1,2,0,3,0,2,0\"");
                Console.Write(">");
                
                var line = Console.ReadLine();
                Console.Clear();

                if (string.IsNullOrEmpty(line)) continue;

                if (line == "q") break;

                if (line == "h")
                {
                    LinearArrayPathFinderWebApi.Program.Main(null);
                    continue;
                }

                if (line == "l")
                {
                    var results = resultManager.GetAllResults();
                    Console.WriteLine(results.Count == 0
                        ? "Not a single result exists in table yet..\n"
                        : "Saved results:");
                    foreach (var result in results)
                    {
                        result.ConsolePrint();
                    }

                    continue;
                }

                if (line.Split(' ') is var listArgs && listArgs != null
                    && listArgs.Length == 3 && listArgs[0].Trim() == "l")
                {
                    switch (listArgs[1].Trim())
                    {
                        case "-id":
                        {
                            int.TryParse(listArgs[2].Trim(), out var id);
                            var resultById = resultManager.GetResultById(id);
                            if (resultById == null)
                            {
                                Console.WriteLine($"Record \"{id}\" was not found!\n");
                            }
                            else
                            {
                                resultById.ConsolePrint();
                            }

                            break;
                        }
                        case "-input":
                        {
                            var resultByInput = resultManager.GetResultByInput(listArgs[2].Trim());
                            if (resultByInput == null)
                            {
                                Console.WriteLine($"Record \"{listArgs[2].Trim()}\" was not found!\n");
                            }
                            else
                            {
                                resultByInput.ConsolePrint();
                            }
                            break;
                        }
                    }
                    continue;
                }

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
                        Console.WriteLine($"Skipped array item \"{number}\" because it is not a valid int.");
                    }
                }
                Console.WriteLine($"Input: {line}\n");
                pathFinder.FindAndSaveSingle(input.ToArray());
            }
        }
    }
}