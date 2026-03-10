namespace MYCV.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for user's professional summary and career objective
    /// </summary>
    public class UserSummaryObjectiveDto
    {
        /// <summary>
        /// Unique identifier of the summary/objective record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Associated user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Professional summary of the user (short overview of experience, skills, and strengths)
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Career objective describing the user's professional goals
        /// </summary>
        public string Objective { get; set; } = string.Empty;

        /// <summary>
        /// Priority used for ordering/display purposes
        /// </summary>
        public int Priority { get; set; } = 1;
    }
}