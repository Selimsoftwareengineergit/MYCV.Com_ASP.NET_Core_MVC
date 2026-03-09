using MYCV.Domain.Enums;

namespace MYCV.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for user's language information
    /// </summary>
    public class UserLanguageDto
    {
        /// <summary>
        /// Unique identifier of the language record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Associated user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Name of the language (e.g., English, French)
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Language proficiency (Native, Fluent, Basic, Beginner)
        /// </summary>
        public LanguageProficiency? Proficiency { get; set; }

        /// <summary>
        /// Priority of the language (used for ordering/display)
        /// </summary>
        public int Priority { get; set; } = 1;
    }
}