using MYCV.Domain.Entities;

namespace MYCV.Application.Interfaces
{
    public interface IUserLanguageRepository
    {
        Task<UserLanguage?> GetByIdAsync(int id);
        Task<List<UserLanguage>> GetByUserIdAsync(int userId);
        Task AddAsync(UserLanguage language);
        Task UpdateAsync(UserLanguage language);
        Task DeleteAsync(UserLanguage language);
    }
}