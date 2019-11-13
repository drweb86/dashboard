using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dashboard.Api;
using Dashboard.ViewModels.Auth;
using Xunit;
using Dashboard.Tests.Core;
using Dashboard.ViewModels.Stickers;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.Tests
{
    public class StickerTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public StickerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task CRUD()
        {
            var userName = Guid.NewGuid().ToString();
            var password = "The Top Secret";

            await _client.PostAsync<AuthRegisterInputModel, AuthResultModel>("/Auth/Register", new AuthRegisterInputModel() {
                Username = userName,
                Password = password
            });

            var loginResult = await _client.PostAsync<AuthLoginInputModel, AuthResultModel>("/Auth/Login", new AuthLoginInputModel()
            {
                Username = userName,
                Password = password
            });

            _client.UseTokenForNowOn(loginResult.Token);

            var addStickerRequest = new StickerAddInputModel()
            {
                HtmlColor = "red",
                Text = "Cap",
                X = 1,
                Y = 2
            };
            var addedStickerResult = await _client.PostAsync<StickerAddInputModel, StickerResultModel>("/Sticker", addStickerRequest);
            Assert.Equal(addedStickerResult.HtmlColor, addStickerRequest.HtmlColor);
            Assert.Equal(addedStickerResult.Text, addStickerRequest.Text);
            Assert.Equal(addedStickerResult.X, addStickerRequest.X);
            Assert.Equal(addedStickerResult.Y, addStickerRequest.Y);
            Assert.NotEqual(0, addedStickerResult.Id);

            var updateStickerRequest = new StickerUpdateInputModel()
            {
                HtmlColor = "red1",
                Text = "Cap1",
                X = 11,
                Y = 21,
                ItemId = addedStickerResult.Id,
            };
            var updatedStickerResult = await _client.PutAsync<StickerUpdateInputModel, StickerResultModel>("/Sticker", updateStickerRequest);
            Assert.Equal(updatedStickerResult.HtmlColor, updateStickerRequest.HtmlColor);
            Assert.Equal(updatedStickerResult.Text, updateStickerRequest.Text);
            Assert.Equal(updatedStickerResult.X, updateStickerRequest.X);
            Assert.Equal(updatedStickerResult.Y, updateStickerRequest.Y);

            var allEntities = await _client.GetAsync<IEnumerable<StickerResultModel>>("/Sticker");
            Assert.Single(allEntities);
            Assert.Equal(allEntities.First().Id, updatedStickerResult.Id);

            await _client.DeleteAsync($"/Sticker/{updatedStickerResult.Id}");

            var allEntities2 = await _client.GetAsync<IEnumerable<StickerResultModel>>("/Sticker");
            Assert.Empty(allEntities2);
        }
    }
}
