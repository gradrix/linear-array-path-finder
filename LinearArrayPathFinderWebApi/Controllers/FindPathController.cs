using System.Collections.Generic;
using LinearArrayPathFinder;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace LinearArrayPathFinderWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FindPathController : ControllerBase
    {
        private readonly PathFinder _pathFinder;

        public FindPathController(PathFinder pathFinder)
        {
            _pathFinder = pathFinder;
        }

        [HttpPost]
        public PathFinderResult Find(List<int> input)
        {
            if (input == null || input.Count == 0)
            {
                return new PathFinderResult
                {
                    Input = string.Join(", ", input),
                    HasValidPath = false,
                    MostEfficientPath = "Error. Input value was not provided.",
                };
            }
            return _pathFinder.FindAndSaveSingle(input.ToArray());
        }
    }
}
