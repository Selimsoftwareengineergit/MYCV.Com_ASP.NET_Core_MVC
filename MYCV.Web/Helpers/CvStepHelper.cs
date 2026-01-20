using MYCV.Domain.Enums;

namespace MYCV.Web.Helpers
{
    public static class CvStepHelper
    {
        public static bool IsValidStep(int stepNumber)
        {
            var minStep = (int)Enum.GetValues(typeof(CvStep)).Cast<CvStep>().Min();
            var maxStep = (int)Enum.GetValues(typeof(CvStep)).Cast<CvStep>().Max();
            return stepNumber >= minStep && stepNumber <= maxStep;
        }

        public static int GetTotalSteps()
        {
            return Enum.GetValues(typeof(CvStep)).Length;
        }
    }
}