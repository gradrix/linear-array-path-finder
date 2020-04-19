using System.Collections.Generic;
using System.Linq;
using LinearArrayPathFinder;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace LinearArrayPathFinderWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BulkFindPathController : ControllerBase
    {
        private readonly PathFinder _pathFinder;

        public BulkFindPathController(PathFinder pathFinder)
        {
            _pathFinder = pathFinder;
        }

        [HttpPost]
        public List<PathFinderResult> Find(List<List<int>> inputs)
        {
            var result = new List<PathFinderResult>();
            if (inputs == null || inputs.Count == 0)
            {
                result.Add(new PathFinderResult
                {
                    HasValidPath = false,
                    MostEfficientPath = "Error. Input value was not provided."
                });
            }
            else
            {
                return _pathFinder.FindAndSaveMultiple(inputs
                    .Select(i => i.ToArray())
                    .ToList());
            }
            return result;
        }
    }
}