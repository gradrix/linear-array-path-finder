using System.Collections.Generic;
using System.Linq;

namespace LinearArrayPathFinder
{
    public class Way
    {
        private readonly Path _availablePath;
        private readonly List<int> _currentPath;

        public Way(Path path, List<int> currentPath = null)
        {
            _availablePath = path;
            _currentPath = currentPath ?? new List<int> {0};
        }

        /// <summary>
        /// Advances current way by one step
        /// </summary>
        /// <returns>Returns the list of possible paths from the new diverging point</returns>
        public List<List<int>> Advance()
        {
            var nextPaths = new List<List<int>>();
            var nextPoints = _availablePath.GetNextPoints(_currentPath.Last());
            if (nextPoints.Count == 0) return null;

            foreach (var nextPoint in nextPoints)
            {
                var nextPath = new List<int>();
                nextPath.AddRange(_currentPath);
                nextPath.Add(nextPoint);
                nextPaths.Add(nextPath);
            }
            return nextPaths;
        }

        /// <summary>
        /// Checks if we finally reached the end of path
        /// </summary>
        public bool EndWasReached => _currentPath.Max() == _availablePath.Count - 1;

        public string ToResult()
        {
            return string.Join(", ", _currentPath);
        }
    }
}