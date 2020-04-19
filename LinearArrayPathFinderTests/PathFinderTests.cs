using LinearArrayPathFinder;
using NSubstitute;
using NUnit.Framework;
using SavedResultManager;

namespace LinearArrayPathFinderTests
{
    public class PathFinderTests
    {
        private PathFinder _pathFinder;

        [SetUp]
        public void Setup()
        {
            var resultManagerMock = Substitute.For<IResultManager>();
            _pathFinder = new PathFinder(resultManagerMock);
        }

        //Valid Routes
        [TestCase(new[] { 0 }, true)]
        [TestCase(new[] { 1, 0 }, true)]
        [TestCase(new[] { 1, 2, 0, 3, 0, 2, 0 }, true)]
        [TestCase(new[] { 1, 2, -1, 3, 0, 2, 1, 2 }, true)]
        [TestCase(new[] { 1, 6, -1, 0, 2, 0, 0, 0 }, true)]
        //Invalid Routes
        [TestCase(new[] { 1, 0, 0 }, false)]
        [TestCase(new[] { 1, 2, 0, 1, 0, 2, 0 }, false)]
        [TestCase(new[] { 1, 2, 0, -1, 0, 2, 0 }, false)]
        [TestCase(new[] { 1, 2, 1, -1, 0, 2, 0 }, false)]
        [TestCase(new[] { 1, 6, -1, 0, 2, 0, 0, 0, 2 }, false)]
        public void PathTests(int[] input, bool expectedResult)
        {
            var result = _pathFinder.FindAndSaveSingle(input);
            Assert.AreEqual(expectedResult, result.HasValidPath);
        }
    }
}