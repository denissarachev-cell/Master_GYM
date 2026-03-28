using GymApp.Data.Context;
using GymApp.Data.Models;
using GymApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на упражнения.
    /// </summary>
    public class ExerciseService : IExerciseService
    {
        private readonly GymDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="ExerciseService"/>.</summary>
        public ExerciseService(GymDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Exercises.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Exercise exercise)
        {
            if (exercise == null) throw new ArgumentNullException(nameof(exercise));
            await _context.Exercises.AddAsync(exercise);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Exercise exercise)
        {
            if (exercise == null) throw new ArgumentNullException(nameof(exercise));
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) throw new KeyNotFoundException($"Упражнение с ID {id} не е намерено.");
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(string muscleGroup)
        {
            if (string.IsNullOrWhiteSpace(muscleGroup))
                throw new ArgumentException("Мускулната група не може да е празна.", nameof(muscleGroup));

            return await _context.Exercises
                .Where(e => e.MuscleGroup == muscleGroup)
                .ToListAsync();
        }
    }
}
