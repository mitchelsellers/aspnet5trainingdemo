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