using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearArrayPathFinder
{
    public class PathFinder
    {
        /// <summary>
        /// Determines if it is possible to reach last element on a positive/negative numbers array, according to these rules:
        /// 1. You start at the first element.
        /// 2. Current element value indicates how many steps you can take at most.
        ///    a.Example: if the value is 3 then you can take 0, 1, 2 or 3 steps;
        ///    b.Example: if the value is 0 then you stuck – the end.
        /// </summary>
        /// <param name="arrayPath"></param>
        /// <returns></returns>
        public bool Find(int[] arrayPath)
        {
            var path = new Path(arrayPath);
            var initialWay = new Way(path);
            var endWasReached = initialWay.EndWasReached;
            var ways = new List<Way> { initialWay };

            while (ways.Count > 0 && !endWasReached)
            {
                var newWays = new List<Way>();
                foreach (var possiblePaths in ways.Select(way => way.Advance())
                    .Where(possiblePaths => possiblePaths != null))
                {
                    newWays.AddRange(possiblePaths.Select(possiblePath => new Way(path, possiblePath)));
                }

                if (newWays.Find(w => w.EndWasReached) is var bestWay && bestWay != null)
                {
                    endWasReached = true;
                    ways = new List<Way>{ bestWay };
                }
                else
                {
                    ways = newWays;
                }
            }

            Console.WriteLine(path);
            Console.WriteLine("End is " + (endWasReached ? $"reachable! {ways.FirstOrDefault()}" : "not reachable.."));
            return endWasReached;
        }
    }
}