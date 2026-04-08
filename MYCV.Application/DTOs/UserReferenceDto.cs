
namespace MYCV.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for user's reference details
    /// </summary>
    public class UserReferenceDto
    {
        /// <summary>
        /// Unique identifier of the reference record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Associated user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Name of the reference person
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Position of the reference person
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// Company of the reference person
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Contact information of the reference person (phone/email)
        /// </summary>
        public string Contact { get; set; } = string.Empty;

        /// <summary>
        /// Relation to the reference person (e.g., Manager, Colleague)
        /// </summary>
        public string? Relation { get; set; }

        /// <summary>
        /// Priority used for ordering/display purposes
        /// </summary>
        public int Priority { get; set; } = 1;
    }
}