using GymApp.Data.Context;
using GymApp.Data.Models;
using GymApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на треньори.
    /// </summary>
    public class TrainerService : ITrainerService
    {
        private readonly GymDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="TrainerService"/>.</summary>
        public TrainerService(GymDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Trainer>> GetAllAsync()
        {
            return await _context.Trainers.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Trainer?> GetByIdAsync(int id)
        {
            return await _context.Trainers.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Trainer trainer)
        {
            if (trainer == null) throw new ArgumentNullException(nameof(trainer));
            await _context.Trainers.AddAsync(trainer);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Trainer trainer)
        {
            if (trainer == null) throw new ArgumentNullException(nameof(trainer));
            _context.Trainers.Update(trainer);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) throw new KeyNotFoundException($"Треньор с ID {id} не е намерен.");
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Trainer>> GetTrainersWithWorkoutsAsync()
        {
            return await _context.Trainers
                .Include(t => t.Workouts)
                .ToListAsync();
        }
    }
}
