using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Tests
{
    internal class Tests
    {
        private WebApplicationFactory<Program> _factory = null!;

        [OneTimeSetUp]
        public void OneTimeSetup() => _factory = new WebApplicationFactory<Program>();

        [Test]
        public async Task Test1()
        {
            var httpClient = _factory.CreateClient();
            var response = await httpClient.GetAsync("/users");
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<User>>();

            response.EnsureSuccessStatusCode();
            Assert.That(users?.Count(), Is.EqualTo(3));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => _factory.Dispose();
    }
}