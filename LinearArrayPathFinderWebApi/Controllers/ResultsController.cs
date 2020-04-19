using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using SavedResultManager;

namespace LinearArrayPathFinderWebApi.Controllers
{
    [ApiController]
    public class ResultsController: ControllerBase
    {
        private readonly IResultManager _resultManager;

        public ResultsController(IResultManager resultManager)
        {
            _resultManager = resultManager;
        }

        [HttpGet]
        [Route("GetResults")]
        [Route("GetResults/All")]
        public List<PathFinderResult> GetAll()
        {
            var result = _resultManager.GetAllResults();
            return result.Count == 0 ? null : result;
        }

        [Route("GetResults/ById/{id}")]
        public PathFinderResult GetById(int id)
        {
            return _resultManager.GetResultById(id);
        }

        [Route("GetResults/ByInput/{input}")]
        public PathFinderResult GetByInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new PathFinderResult
                {
                    HasValidPath = false,
                    MostEfficientPath = $"Error. Input named \"input\" was not provided.",
                };
            }
            return _resultManager.GetResultByInput(input);
        }
    }
}