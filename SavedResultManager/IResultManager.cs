using System.Collections.Generic;
using Models;

namespace SavedResultManager
{
    public interface IResultManager
    {
        void AddResult(PathFinderResult model);
        void BatchAddResult(List<PathFinderResult> modelList);
        List<PathFinderResult> GetAllResults();
        PathFinderResult GetResultById(int id);
        PathFinderResult GetResultByInput(string input);
    }
}