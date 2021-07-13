# Addition of Services Layer

Trying to get a more enterprise scenario.

## Create New Project (SampleWeb.Services)

* Project will be a .NET 5 Class Library
* Add project reference to the `data` project
* Update `SampleWeb` to reference this project

## Install needed NuGet

```
Install-Package Microsoft.AspNetCore.Http.Features
```

## Setup Dependency Injection in our Services Layer

Add a new file /DependencyResolution/StartupExtensions.cs with this content

```
using SampleWeb.Services.Samples;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static void RegisterDataServices(this IServiceCollection services)
        {
            //Add your stuff here!

        }
    }
}
```

## Setup Services Code

### Add a new file /Samples/Models/FormWithFileViewModel.cs

```
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SampleWeb.Services.Samples.Models
{
    public class FormWithFileViewModel
    {
        [Required]
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name="Profile Photo")]
        public IFormFile ProfilePhoto { get; set; }
    }
}
```


### Add a new file /Samples/SampleDataService.cs with the following

```
using SampleWeb.Services.Samples.Models;

namespace SampleWeb.Services.Samples
{
    public interface ISampleDataService
    {
        FormWithFileViewModel GetFormWithFileViewModel();
        bool ProcessFormWithFileViewModel(FormWithFileViewModel model);
    }

    public class SampleDataService : ISampleDataService
    {
        public FormWithFileViewModel GetFormWithFileViewModel()
        {
            //Do special initialization here
            return new FormWithFileViewModel();
        }

        public bool ProcessFormWithFileViewModel(FormWithFileViewModel model)
        {
            //Do actual stuff here
            return true;
        }
    }
}
```

Register this as a dependency

```
services.AddTransient<ISampleDataService, SampleDataService>();
```

## Update Startup.cs within 

Add to startup.cs inside of the "ConfigureServices" section, just after AddControllersWithViews()

```
services.RegisterDataServices();
```

## Add a SampleController with this content to the Web Project

```
using Microsoft.AspNetCore.Mvc;
using SampleWeb.Services.Samples;
using SampleWeb.Services.Samples.Models;

namespace SampleWeb.Controllers
{
    public class SampleController : Controller
    {
        private readonly ISampleDataService _sampleDataService;

        public SampleController(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        [HttpGet]
        public IActionResult TestForm()
        {
            return View(_sampleDataService.GetFormWithFileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TestForm(FormWithFileViewModel data)
        {
            if (!ModelState.IsValid)
                return View(data);

            var success = _sampleDataService.ProcessFormWithFileViewModel(data);
            if (success)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Unknown error");
            return View(data);
        }
    }
}
```

## Add SampleControllerTests.cs to the Unit Test Project

```
using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleWeb.Controllers;
using SampleWeb.Services.Samples;
using SampleWeb.Services.Samples.Models;
using Xunit;

namespace SampleWeb.Tests.Controllers
{
    public class SampleControllerTests
    {
        private readonly Mock<ISampleDataService> _sampleMockDataService;

        public SampleControllerTests()
        {
            _sampleMockDataService = new Mock<ISampleDataService>();
        }

        [Fact]
        public void TestFormGet_ShouldCallService_ReturnViewResultWithModel()
        {
            //Arrange
            var expectedModel = new FormWithFileViewModel();
            _sampleMockDataService.Setup(s => s.GetFormWithFileViewModel()).Returns(expectedModel).Verifiable();
            var controller = new SampleController(_sampleMockDataService.Object);
            
            //Act
            var actualResult = controller.TestForm();

            //Assert
            _sampleMockDataService.Verify();
            var actualViewResult = Assert.IsType<ViewResult>(actualResult);
            Assert.Equal(expectedModel, actualViewResult.Model);
        }

        [Fact]
        public void TestFormPost_ShouldNotCallService_ReturningViewResultWithPostedModel_WhenModelErrorsExist()
        {
            //Arrange
            var controller = new SampleController(_sampleMockDataService.Object);
            controller.ModelState.AddModelError("FirstName", "Message");
            var input = new FormWithFileViewModel {FirstName = "Test", LastName = "Test"};

            //Act
            var actualResult = controller.TestForm(input);

            //Assert
            var actualViewResult = Assert.IsType<ViewResult>(actualResult);
            Assert.Equal(input, actualViewResult.Model);
        }
    }
}
```

## Add the View (Right Click "Add View" -> "Model")

Add this to the `<form>` tag  `method="post" enctype="multipart/form-data"`

### Add File Input Field

```
<div class="form-group">
    <label asp-for="ProfilePhoto" class="control-label"></label>
    <input type="file" asp-for="ProfilePhoto" class="form-control" />
    <span asp-validation-for="ProfilePhoto" class="text-danger"></span>
    <span class="help-block">Should Be 945 X 490 px & Compressed using <a href="https://compresspng.com" target="_blank">Compress Png</a> or <a href="https://compressjpg.com" target="_blank;">Compress Jpg</a> before upload</span>
</div>
```

## Update Menu (/Views/Shared/_Layout.cshtml)

Add menu item for the sample page 

```
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-controller="Sample" asp-action="TestForm">Sample Test</a>
</li>
```
