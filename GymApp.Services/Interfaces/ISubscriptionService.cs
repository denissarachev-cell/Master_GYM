using GymApp.Data.Models;

namespace GymApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на абонаменти.
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>Връща всички абонаменти.</summary>
        Task<IEnumerable<Subscription>> GetAllAsync();

        /// <summary>Връща абонамент по идентификатор.</summary>
        Task<Subscription?> GetByIdAsync(int id);

        /// <summary>Добавя нов абонамент.</summary>
        Task AddAsync(Subscription subscription);

        /// <summary>Актуализира съществуващ абонамент.</summary>
        Task UpdateAsync(Subscription subscription);

        /// <summary>Изтрива абонамент по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща активните абонаменти към днешна дата.</summary>
        Task<IEnumerable<Subscription>> GetActiveSubscriptionsAsync();

        /// <summary>Връща абонаментите на определен член.</summary>
        Task<IEnumerable<Subscription>> GetByMemberIdAsync(int memberId);
    }
}
