using ExampleApi.Models;

namespace Tests
{
    internal class NUnitDoingFluentyThings
    {
        [Test]
        public void CollectionCompareTest()
        {
            var collectionOne = new List<int>[1, 2, 3, 5];
            var collectionTwo = new List<int>[1, 2, 3, 5];
            CollectionAssert.AreEquivalent(collectionOne, collectionTwo);
        }

        [Test]
        public void ItemCompareTest()
        {
            Joke joke1 = new("aaa", "bbbb");
            Joke joke2 = new("aaa", "bbbb");
            Assert.That(joke1, Is.EqualTo(joke2));
        }

        [Test]
        public void TimeTest()
        {
            var timeOne = DateTime.UtcNow;
            var timeTwo = DateTime.UtcNow.AddSeconds(2);
            Assert.Multiple(() =>
            {
                Assert.That(timeOne, Is.EqualTo(timeTwo).Within(3).Seconds);
                Assert.That(timeOne, Is.Not.EqualTo(timeTwo).Within(1).Seconds);
            });
        }
    }
}
