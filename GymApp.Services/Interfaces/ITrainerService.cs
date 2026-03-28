using GymApp.Data.Models;

namespace GymApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на треньори.
    /// </summary>
    public interface ITrainerService
    {
        /// <summary>Връща всички треньори.</summary>
        Task<IEnumerable<Trainer>> GetAllAsync();

        /// <summary>Връща треньор по идентификатор.</summary>
        Task<Trainer?> GetByIdAsync(int id);

        /// <summary>Добавя нов треньор.</summary>
        Task AddAsync(Trainer trainer);

        /// <summary>Актуализира съществуващ треньор.</summary>
        Task UpdateAsync(Trainer trainer);

        /// <summary>Изтрива треньор по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща треньори с техните тренировки.</summary>
        Task<IEnumerable<Trainer>> GetTrainersWithWorkoutsAsync();
    }
}
