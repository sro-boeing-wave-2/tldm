using MarketPlace.Controllers;
using MarketPlace.Models;
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MarketPlace.Tests
{
    public class MarketPlaceUnitTest
    {
        public ApplicationsController GetController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MarketPlaceContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var appcontext = new MarketPlaceContext(optionsBuilder.Options);
            CreateTestData(optionsBuilder.Options);
            return new ApplicationsController(appcontext);
        }

        public void CreateTestData(DbContextOptions<MarketPlaceContext> options)
        {
            using (var appcontext = new MarketPlaceContext(options))
            {
                var ApplicationsToAdd = new List<Application>
                {
                    new Application()
                    {
                        Id=1,
                        Name="Github",
                        Info="Github Integration",
                        AppUrl="www.github.com",
                        Developer="Mr. XYZ",
                        LogoUrl="www.logo.com"
                    },
                    new Application()
                    {
                        Id=2,
                        Name="Google Drive",
                        Info="Google Drive Integration",
                        AppUrl="www.googledrive.com",
                        Developer="Mr. ABC",
                        LogoUrl="www.glogo.com"
                    }
                };
                appcontext.Application.AddRange(ApplicationsToAdd);
                appcontext.SaveChanges();
            }
        }

        [Fact]
        public async void TestForGetAll()
        {
            var _appcontroller = GetController();
            var Res = await _appcontroller.GetApplication();
            var ListOfApplications = Res as List<Application>;
            Assert.Equal(2, ListOfApplications.Count);
        }

        [Fact]
        public async void TestForGetById()
        {
            var _appcontroller = GetController();
            var Res = await _appcontroller.GetApplication(1);
            var AppOkObject = Res as OkObjectResult;
            var App = AppOkObject.Value as Application;
            App.Name.Should().Be("Github");
            App.Info.Should().Be("Github Integration");
            App.AppUrl.Should().Be("www.github.com");
            App.Developer.Should().Be("Mr. XYZ");
            App.LogoUrl.Should().Be("www.logo.com");
        }

        [Fact]
        public async void TestForPost()
        {
            var _appcontroller = GetController();
            Application app = new Application()
            {
                Id = 3,
                Name = "Google Photos",
                Info = "Google Photos Integration",
                AppUrl = "www.googlephotos.com",
                Developer = "Mr. Google",
                LogoUrl = "www.googlelogo.com"
            };
            var Result = await _appcontroller.PostApplication(app);
            var AppCreatedAtActionObject = Result as CreatedAtActionResult;
            var App = AppCreatedAtActionObject.Value as Application;
            App.Name.Should().Be("Google Photos");
            App.Info.Should().Be("Google Photos Integration");
            App.AppUrl.Should().Be("www.googlephotos.com");
            App.Developer.Should().Be("Mr. Google");
            App.LogoUrl.Should().Be("www.googlelogo.com");
        }

        [Fact]
        public async void DeleteById()
        {
            var _controller = GetController();
            var result = await _controller.DeleteApplication(1);
            var resultAsOkObjectResult = result as OkObjectResult;
            Assert.Equal(200, resultAsOkObjectResult.StatusCode);
        }

        [Fact]
        public async void TestingPut()
        {
            var _controller = GetController();
            var App = new Application()
            {
                Id = 2,
                Name = "Google Drive Name Updated"
            };
            var result = await _controller.PutApplication(2, App);
            var resultAsOkObjectResult = result as NoContentResult;
            Assert.Equal(204, resultAsOkObjectResult.StatusCode);
        }
    }
}
