using System;
using System.Collections.Generic;
using System.Text;
using MarketPlaceBackend.Controllers;
using MarketPlaceBackend.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FluentAssertions;

namespace MarketPlaceBackend.Tests
{
    public class MarketPlaceIntegrationTest
    {
        private HttpClient _client;
        public MarketPlaceIntegrationTest()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());
            _client = host.CreateClient();
        }

        private async Task CreateTestDataAsync()
        {
            var app1 = new Application()
            {
                Id = "abcd-efgh",
                Name = "Github",
                Info = "Github Integration",
                AppUrl = "www.github.com",
                Developer = "Mr. XYZ",
                LogoUrl = "www.logo.com"
            };
            var app2 = new Application()
            {
                Id = "wxyz-abcd",
                Name = "Google Drive",
                Info = "Google Drive Integration",
                AppUrl = "www.googledrive.com",
                Developer = "Mr. ABC",
                LogoUrl = "www.glogo.com"
            };
            var json = JsonConvert.SerializeObject(app1);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("/api/applications", stringContent);

            json = JsonConvert.SerializeObject(app2);
            stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            Response = await _client.PostAsync("/api/applications", stringContent);
        }

        [Fact]
        public async Task TestGetRequestAsync()
        {
            var Response = await _client.GetAsync("/api/applications");
            var ResponseBody = await Response.Content.ReadAsStringAsync();
            Response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestGetById()
        {
            await CreateTestDataAsync();
            var Response = await _client.GetAsync("/api/applications/abcd-efgh");
            var ApplicationAsString = await Response.Content.ReadAsStringAsync();
            JObject applicationObject = JObject.Parse(ApplicationAsString);

            Assert.Equal("Github", applicationObject.GetValue("name"));
            Assert.Equal("Github Integration", applicationObject.GetValue("info"));
            Assert.Equal("Mr. XYZ", applicationObject.GetValue("developer"));
            Assert.Equal("www.github.com", applicationObject.GetValue("appUrl"));
            Assert.Equal("www.logo.com", applicationObject.GetValue("logoUrl"));

            Response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestGetByIdForNotNullCase()
        {
            var Response = await _client.GetAsync("/api/applications/abcd-efgh");
            Assert.Equal("NotFound", Response.StatusCode.ToString());
        }
    }
}
