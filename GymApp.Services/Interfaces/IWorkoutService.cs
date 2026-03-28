using GymApp.Data.Models;

namespace GymApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на тренировки.
    /// </summary>
    public interface IWorkoutService
    {
        /// <summary>Връща всички тренировки.</summary>
        Task<IEnumerable<Workout>> GetAllAsync();

        /// <summary>Връща тренировка по идентификатор.</summary>
        Task<Workout?> GetByIdAsync(int id);

        /// <summary>Добавя нова тренировка.</summary>
        Task AddAsync(Workout workout);

        /// <summary>Актуализира съществуваща тренировка.</summary>
        Task UpdateAsync(Workout workout);

        /// <summary>Изтрива тренировка по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща тренировки по ниво на трудност.</summary>
        Task<IEnumerable<Workout>> GetByDifficultyAsync(string difficulty);

        /// <summary>Връща тренировки с упражнения и треньор.</summary>
        Task<IEnumerable<Workout>> GetWorkoutsWithDetailsAsync();
    }
}
