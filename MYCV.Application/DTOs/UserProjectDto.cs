using MYCV.Domain.Enums;

namespace MYCV.Application.DTOs
{
    public class UserProjectDto
    {
        /// <summary>
        /// Unique identifier of the project record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Associated user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Title of the project
        /// </summary>
        public string ProjectTitle { get; set; } = string.Empty;

        /// <summary>
        /// Role of the user in the project
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Type of the project (Web, Mobile, Desktop, etc.)
        /// </summary>
        public ProjectType ProjectType { get; set; } = ProjectType.Other;

        /// <summary>
        /// Detailed description of the project
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Technologies used in the project
        /// </summary>
        public string Technologies { get; set; } = string.Empty;

        /// <summary>
        /// Project start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Project end date (null if ongoing)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Indicates whether the project is ongoing
        /// </summary>
        public bool IsOngoing => EndDate == null;

        /// <summary>
        /// Optional link to the project
        /// </summary>
        public string? ProjectLink { get; set; }

        /// <summary>
        /// Optional remarks about the project
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Priority of the project (used for ordering/display)
        /// </summary>
        public int Priority { get; set; } = 1;
    }
}