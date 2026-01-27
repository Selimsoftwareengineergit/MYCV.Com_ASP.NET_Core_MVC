using Microsoft.AspNetCore.Mvc;
using MYCV.Domain.Enums;

namespace MYCV.Web.ViewComponents
{
    public class CvStepNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int currentStep = 1, List<int>? completedSteps = null)
        {
            completedSteps ??= new List<int>();
            var steps = Enum.GetValues(typeof(CvStep)).Cast<CvStep>().ToList();

            var model = new CvStepNavigationViewModel
            {
                Steps = steps,
                CurrentStep = currentStep,
                CompletedSteps = completedSteps
            };

            return View(model); 
        }
    }

    public class CvStepNavigationViewModel
    {
        public List<CvStep> Steps { get; set; } = new List<CvStep>();
        public int CurrentStep { get; set; }
        public List<int> CompletedSteps { get; set; } = new List<int>();
    }
}