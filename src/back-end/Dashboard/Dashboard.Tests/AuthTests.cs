using System;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dashboard.Api;
using Dashboard.ViewModels.Auth;
using Newtonsoft.Json;
using Xunit;

namespace Dashboard.Tests
{
    // In memory db ?

    static class HttpClientExtensions
    {
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(this HttpClient client, string url, TRequest data)
        {
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            var response = await client.PostAsync(url, byteContent);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Api '{url}' call failed with error code {response.StatusCode} and message {content}");

            return JsonConvert.DeserializeObject<TResponse>(content);
        }
    }


    public class AuthTests : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;

        public AuthTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task CanRegisterAndLogin()
        {
            var registerResult = await Client.PostAsync<AuthRegisterInputModel, AuthResultModel>("/Auth/Register", new AuthRegisterInputModel() {
                FirstName = "Vasya",
                LastName = "Kurochkin",
                Username = "Vasilina",
                Password = "The Top Secret"
            });

            Assert.Equal("Registered", registerResult.Message);

            var loginResult = await Client.PostAsync<AuthLoginInputModel, AuthResultModel>("/Auth/Login", new AuthLoginInputModel()
            {
                Username = "Vasilina",
                Password = "The Top Secret"
            });
            Assert.NotEqual("", loginResult.Token);
            Assert.NotNull(loginResult.Token);

            try
            {
                var badloginResult = await Client.PostAsync<AuthLoginInputModel, AuthResultModel>("/Auth/Login", new AuthLoginInputModel()
                {
                    Username = "Vasilina",
                    Password = "1111111The Top Secret"
                });
                throw new Exception("Can login with invalid password!");
            }
            catch
            {

            }

        }

        [Fact]
        public async Task UnauthorisedCallWillFail()
        {
            try
            {
                var testResult = await Client.PostAsync<AuthRegisterInputModel, AuthResultModel>("/Auth/Test", new AuthRegisterInputModel()
                {
                    FirstName = "Vasya",
                    LastName = "Kurochkin",
                    Username = "Vasilina",
                    Password = "The Top Secret"
                });
                throw new Exception("Can do unauthorized staff!");
            }
            catch { }

        }

        //[Fact]
        //public async Task TestGetStockItemAsync()
        //{
        //    // Arrange
        //    var request = "/api/v1/Warehouse/StockItem/1";

        //    // Act
        //    var response = await Client.GetAsync(request);

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        //[Fact]
        //public async Task TestPostStockItemAsync()
        //{
        //    // Arrange
        //    var request = new
        //    {
        //        Url = "/api/v1/Warehouse/StockItem",
        //        Body = new
        //        {
        //            StockItemName = string.Format("USB anime flash drive - Vegeta {0}", Guid.NewGuid()),
        //            SupplierID = 12,
        //            UnitPackageID = 7,
        //            OuterPackageID = 7,
        //            LeadTimeDays = 14,
        //            QuantityPerOuter = 1,
        //            IsChillerStock = false,
        //            TaxRate = 15.000m,
        //            UnitPrice = 32.00m,
        //            RecommendedRetailPrice = 47.84m,
        //            TypicalWeightPerUnit = 0.050m,
        //            CustomFields = "{ \"CountryOfManufacture\": \"Japan\", \"Tags\": [\"32GB\",\"USB Powered\"] }",
        //            Tags = "[\"32GB\",\"USB Powered\"]",
        //            SearchDetails = "USB anime flash drive - Vegeta",
        //            LastEditedBy = 1,
        //            ValidFrom = DateTime.Now,
        //            ValidTo = DateTime.Now.AddYears(5)
        //        }
        //    };

        //    // Act
        //    var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
        //    var value = await response.Content.ReadAsStringAsync();

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        //[Fact]
        //public async Task TestPutStockItemAsync()
        //{
        //    // Arrange
        //    var request = new
        //    {
        //        Url = "/api/v1/Warehouse/StockItem/1",
        //        Body = new
        //        {
        //            StockItemName = string.Format("USB anime flash drive - Vegeta {0}", Guid.NewGuid()),
        //            SupplierID = 12,
        //            Color = 3,
        //            UnitPrice = 39.00m
        //        }
        //    };

        //    // Act
        //    var response = await Client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        //[Fact]
        //public async Task TestDeleteStockItemAsync()
        //{
        //    // Arrange

        //    var postRequest = new
        //    {
        //        Url = "/api/v1/Warehouse/StockItem",
        //        Body = new
        //        {
        //            StockItemName = string.Format("Product to delete {0}", Guid.NewGuid()),
        //            SupplierID = 12,
        //            UnitPackageID = 7,
        //            OuterPackageID = 7,
        //            LeadTimeDays = 14,
        //            QuantityPerOuter = 1,
        //            IsChillerStock = false,
        //            TaxRate = 10.000m,
        //            UnitPrice = 10.00m,
        //            RecommendedRetailPrice = 47.84m,
        //            TypicalWeightPerUnit = 0.050m,
        //            CustomFields = "{ \"CountryOfManufacture\": \"USA\", \"Tags\": [\"Sample\"] }",
        //            Tags = "[\"Sample\"]",
        //            SearchDetails = "Product to delete",
        //            LastEditedBy = 1,
        //            ValidFrom = DateTime.Now,
        //            ValidTo = DateTime.Now.AddYears(5)
        //        }
        //    };

        //    // Act
        //    var postResponse = await Client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
        //    var jsonFromPostResponse = await postResponse.Content.ReadAsStringAsync();

        //    //var singleResponse = JsonConvert.DeserializeObject<SingleResponse<StockItem>>(jsonFromPostResponse);

        //    //var deleteResponse = await Client.DeleteAsync(string.Format("/api/v1/Warehouse/StockItem/{0}", singleResponse.Model.StockItemID));

        //    //// Assert
        //    //postResponse.EnsureSuccessStatusCode();

        //    //Assert.False(singleResponse.DidError);

        //    //deleteResponse.EnsureSuccessStatusCode();
        //}
    }
}
