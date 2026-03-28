using GymApp.Data.Models;

namespace GymApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на упражнения.
    /// </summary>
    public interface IExerciseService
    {
        /// <summary>Връща всички упражнения.</summary>
        Task<IEnumerable<Exercise>> GetAllAsync();

        /// <summary>Връща упражнение по идентификатор.</summary>
        Task<Exercise?> GetByIdAsync(int id);

        /// <summary>Добавя ново упражнение.</summary>
        Task AddAsync(Exercise exercise);

        /// <summary>Актуализира съществуващо упражнение.</summary>
        Task UpdateAsync(Exercise exercise);

        /// <summary>Изтрива упражнение по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща упражнения по мускулна група.</summary>
        Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(string muscleGroup);
    }
}
