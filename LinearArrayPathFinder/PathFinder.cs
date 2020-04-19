using SavedResultManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace LinearArrayPathFinder
{
    public class PathFinder
    {
        private readonly IResultManager _resultManager;

        public PathFinder()
            : this(DependencyResolver.GetService<IResultManager>())
        { }

        public PathFinder(IResultManager resultManager)
        {
            _resultManager = resultManager;
        }

        /// <summary>
        /// Determines if it is possible to reach last element on a positive/negative numbers array, according to these rules:
        /// 1. You start at the first element.
        /// 2. Current element value indicates how many steps you can take at most.
        ///    a.Example: if the value is 3 then you can take 0, 1, 2 or 3 steps;
        ///    b.Example: if the value is 0 then you stuck – the end.
        /// </summary>
        /// <param name="arrayPath">Path to find </param>
        /// <returns>Result model</returns>
        private PathFinderResult Find(int[] arrayPath)
        {
            var path = new Path(arrayPath);
            var initialWay = new Way(path);
            var endWasReached = initialWay.EndWasReached;
            var ways = new List<Way> { initialWay };

            while (ways.Count > 0 && !endWasReached)
            {
                var newWays = new List<Way>();
                //For each way we look into the value of its last point and get new diverging ways which will be holding full paths.
                foreach (var possiblePaths in ways.Select(way => way.Advance())
                    .Where(possiblePaths => possiblePaths != null))
                {
                    newWays.AddRange(possiblePaths.Select(possiblePath => new Way(path, possiblePath)));
                }

                //As soon as we have reached the end it means most efficient path has been reached because all ways advance at the same time.
                if (newWays.Find(w => w.EndWasReached) is var bestWay && bestWay != null)
                {
                    endWasReached = true;
                    ways = new List<Way>{ bestWay };
                }
                //Each "move" we reinitialize ways to hold all possible ways diverging from the non dead-end point (when value is greater than zero).
                else
                {
                    ways = newWays;
                }
            }

            return new PathFinderResult
            {
                Input = string.Join(',', arrayPath),
                HasValidPath = endWasReached,
                MostEfficientPath = ways.FirstOrDefault()?.ToResult()
            };
        }

        /// <summary>
        /// Finds a single most efficient path and saves new result into the DB
        /// </summary>
        /// <param name="arrayPath">Array of paths to process</param>
        /// <returns>Most efficient path result model</returns>
        public PathFinderResult FindAndSaveSingle(int[] arrayPath)
        {
            if (arrayPath.Length == 0) return null;

            var result = _resultManager.GetResultByInput(string.Join(',', arrayPath));
            if (result == null)
            {
                result = Find(arrayPath);
                _resultManager.AddResult(result);
            }
            else
            {
                Console.WriteLine($"Skipping calculation - found {result.Input} in the database.");
            }

            result.ConsolePrint();
            return result;
        }

        /// <summary>
        /// Finds multiple most efficient paths at the same time and saves new results into the DB
        /// </summary>
        /// <param name="arrayPaths">Array of array paths to process</param>
        /// <returns>List of most efficient path result models</returns>
        public List<PathFinderResult> FindAndSaveMultiple(List<int[]> arrayPaths)
        {
            if (arrayPaths.Count == 0) return null;

            var results = new List<PathFinderResult>();
            var resultsToWrite = new List<PathFinderResult>();
            foreach (var arrayPath in arrayPaths)
            {
                var result = _resultManager.GetResultByInput(string.Join(',', arrayPath));
                if (result == null)
                {
                    result = Find(arrayPath);
                    resultsToWrite.Add(result);
                }
                else
                {
                    Console.WriteLine($"Skipping calculation - found {result.Input} in the database.");
                }
                results.Add(result);
                result.ConsolePrint();
            }
            _resultManager.BatchAddResult(resultsToWrite);
            return results;
        }
    }
}