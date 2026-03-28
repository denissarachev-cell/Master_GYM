using GymApp.Data.Context;
using GymApp.Data.Models;
using GymApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на тренировки.
    /// </summary>
    public class WorkoutService : IWorkoutService
    {
        private readonly GymDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="WorkoutService"/>.</summary>
        public WorkoutService(GymDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Workout>> GetAllAsync()
        {
            return await _context.Workouts.Include(w => w.Trainer).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Workout?> GetByIdAsync(int id)
        {
            return await _context.Workouts
                .Include(w => w.Trainer)
                .FirstOrDefaultAsync(w => w.WorkoutId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Workout workout)
        {
            if (workout == null) throw new ArgumentNullException(nameof(workout));
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Workout workout)
        {
            if (workout == null) throw new ArgumentNullException(nameof(workout));
            _context.Workouts.Update(workout);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null) throw new KeyNotFoundException($"Тренировка с ID {id} не е намерена.");
            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Workout>> GetByDifficultyAsync(string difficulty)
        {
            if (string.IsNullOrWhiteSpace(difficulty))
                throw new ArgumentException("Нивото на трудност не може да е празно.", nameof(difficulty));

            return await _context.Workouts
                .Include(w => w.Trainer)
                .Where(w => w.DifficultyLevel == difficulty)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Workout>> GetWorkoutsWithDetailsAsync()
        {
            return await _context.Workouts
                .Include(w => w.Trainer)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .ToListAsync();
        }
    }
}
