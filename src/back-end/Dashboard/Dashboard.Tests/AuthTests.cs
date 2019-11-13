using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dashboard.Api;
using Dashboard.ViewModels.Auth;
using Xunit;
using Dashboard.Tests.Core;

namespace Dashboard.Tests
{
    public class AuthTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public AuthTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task CanRegisterAndLogin()
        {
            var userName = Guid.NewGuid().ToString();
            var password = "The Top Secret";

            var registerResult = await _client.PostAsync<AuthRegisterInputModel, AuthResultModel>("/Auth/Register", new AuthRegisterInputModel() {
                Username = userName,
                Password = password
            });

            Assert.Equal("Registered", registerResult.Message);

            var loginResult = await _client.PostAsync<AuthLoginInputModel, AuthResultModel>("/Auth/Login", new AuthLoginInputModel()
            {
                Username = userName,
                Password = password
            });
            Assert.NotEqual("", loginResult.Token);
            Assert.NotNull(loginResult.Token);

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var badloginResult = await _client.PostAsync<AuthLoginInputModel, AuthResultModel>("/Auth/Login", new AuthLoginInputModel()
                {
                    Username = userName,
                    Password = "1111111" + password
                });
            });
        }

        [Fact]
        public async Task UnauthorisedCallWillFail()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var testResult = await _client.PostAsync<AuthRegisterInputModel, AuthResultModel>("/Auth/Test", new AuthRegisterInputModel()
                {
                    Username = "Vasilina",
                    Password = "The Top Secret"
                });
            });
        }
    }
}
