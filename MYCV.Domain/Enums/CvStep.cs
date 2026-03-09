
namespace MYCV.Domain.Enums
{
    public enum CvStep
    {
        PersonalDetail = 1,      
        Education = 2,             
        WorkExperience = 3,       
        Skills = 4,               
        Projects = 5,              
        Languages = 6,             // Languages with proficiency
        SummaryObjective = 7,      // Career summary and objective
        References = 8,            // References (optional)
        Subscription = 9,          // Subscription/Payment check before download
        PreviewDownload = 10       // Final CV preview and download
    }
}