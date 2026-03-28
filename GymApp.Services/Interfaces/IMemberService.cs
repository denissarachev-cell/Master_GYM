using GymApp.Data.Models;

namespace GymApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на членове.
    /// </summary>
    public interface IMemberService
    {
        /// <summary>Връща всички членове.</summary>
        Task<IEnumerable<Member>> GetAllAsync();

        /// <summary>Връща член по идентификатор.</summary>
        Task<Member?> GetByIdAsync(int id);

        /// <summary>Добавя нов член.</summary>
        Task AddAsync(Member member);

        /// <summary>Актуализира съществуващ член.</summary>
        Task UpdateAsync(Member member);

        /// <summary>Изтрива член по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща членове с активни абонаменти към днешна дата.</summary>
        Task<IEnumerable<Member>> GetActiveMembersAsync();

        /// <summary>Връща членове с техните тренировки.</summary>
        Task<IEnumerable<Member>> GetMembersWithWorkoutsAsync();
    }
}
