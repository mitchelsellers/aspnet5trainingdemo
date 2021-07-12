# EF Core

## Add Training Class Model (Models/TrainingClass.cs)

```
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleWeb.Data.Models
{
    public class TrainingClass
    {
        public int TrainingClassId { get; set; }

        [Required]
        public int TrainingEventId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomName { get; set; }

        public int? MaxAttendees { get; set; }


        public TrainingEvent TrainingEvent { get; set; }
    }
}
```

## Add Training Event 

```
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleWeb.Data.Models
{
    public class TrainingEvent
    {
        public int TrainingEventId { get; set; }

        [Required]
        [StringLength(300)]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(500)]
        public string EventLocation { get; set; }

        public ICollection<TrainingClass> Classes { get; set; }
    }
}
```

## Add DbContext

```
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SampleWeb.Data.Models;

namespace SampleWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Helpful snippets
            //For Migrations
            //dotnet ef --startup-project ../SampleWeb migrations add Initial

            //Apply changes
            //dotnet ef --startup-project ../SampleWeb database update
        }

        public DbSet<TrainingEvent> TrainingEvents { get; set; }
        public DbSet<TrainingClass> TrainingClasses { get; set; }
    }
}
```

## Add ViewModel for Listing (SampleWeb.Services.Samples.Models.TrainingEventListModel)

```
using System;

namespace SampleWeb.Services.Samples.Models
{
    public class TrainingEventListModel
    {
        public int TrainingEventId { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public string EventLocation { get; set; }

        public int ClassCount { get; set; }
    }
}
```

## Update SampleService

```
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SampleWeb.Data;
using SampleWeb.Services.Samples.Models;

namespace SampleWeb.Services.Samples
{
    public interface ISampleDataService
    {
        FormWithFileViewModel GetFormWithFileViewModel();
        bool ProcessFormWithFileViewModel(FormWithFileViewModel model);
        IQueryable<TrainingEventListModel> ListTrainingEvents();
    }

    public class SampleDataService : ISampleDataService
    {
        private readonly ApplicationDbContext _context;

        public SampleDataService(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public IQueryable<TrainingEventListModel> ListTrainingEvents()
        {
            return _context.TrainingEvents
                .AsNoTracking()
                .OrderBy(m => m.EventDate)
                .Select(e => new TrainingEventListModel
                {
                    EventDate = e.EventDate,
                    ClassCount = e.Classes.Count,
                    EventLocation = e.EventLocation,
                    EventName = e.EventName,
                    TrainingEventId = e.TrainingEventId
                });
        }
    }
}
```

## Addd Action to SampleController.cs (SampleWeb Project)

```
[HttpGet]
public IActionResult ListClasses()
{
    var data = _sampleDataService.ListTrainingEvents().Take(20);
    return View(data);
}
```

## Scaffold a list view

## Add & Apply Migration

```
dotnet ef --startup-project ../SampleWeb migrations add TrainingEvents
dotnet ef --startup-project ../SampleWeb database update
```
