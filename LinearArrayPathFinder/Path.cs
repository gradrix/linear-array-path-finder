using System.Collections.Generic;

namespace LinearArrayPathFinder
{
    public class Path
    {
        private readonly Dictionary<int, int> _pathDict;

        public Path(int[] path)
        {
            _pathDict = new Dictionary<int, int>();
            for (var i = 0; i < path.Length; i++)
            {
                _pathDict.Add(i, path[i]);
            }
        }

        public int Count => _pathDict.Count;

        /// <summary>
        /// Uses the point of the path and determines new diverging paths from this point by its value.
        /// </summary>
        /// <param name="index">Index of the point to in the path begin from</param>
        /// <returns>If the point still can be used -> gets its value and finds next point indexes of the path. If not -> returns empty int list</returns>
        public List<int> GetNextPoints(int index)
        {
            var nextIndexes = new List<int>();
            if (!_pathDict.ContainsKey(index)) return nextIndexes;

            var value = _pathDict[index];
            if (value <= 0) return nextIndexes; //Do not return any paths if value is less than zero
            for (var i = index + 1; i <= index + value; i++)
            {
                if (i < _pathDict.Count)
                {
                    nextIndexes.Add(i);
                }
            }
            return nextIndexes;
        }
    }
}