﻿using Microsoft.AspNetCore.Mvc;
using SampleWeb.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace SampleWeb.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> loggerMock = new Mock<ILogger<HomeController>>();

        [Fact]
        public void Index_ReturnsAViewResult_WithNullModel()
        {
            //Arrange
            var controller = new HomeController(loggerMock.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public void Privacy_ReturnsAViewResult_WithNullModel()
        {
            //Arrange
            var controller = new HomeController(loggerMock.Object);

            //Act
            var result = controller.Privacy();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        //[Fact]
        //public void Error_ReturnsAViewResult_WithAnErrorViewModel()
        //{
        //    //Arrange
        //    var controller = new HomeController();

        //    //Act
        //    var result = controller.Error();

        //    //Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var modelData = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.Model);
        //}
    }
}