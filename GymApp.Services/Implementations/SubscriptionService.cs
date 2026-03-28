using GymApp.Data.Context;
using GymApp.Data.Models;
using GymApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на абонаменти.
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly GymDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="SubscriptionService"/>.</summary>
        public SubscriptionService(GymDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            return await _context.Subscriptions
                .Include(s => s.Member)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _context.Subscriptions
                .Include(s => s.Member)
                .FirstOrDefaultAsync(s => s.SubscriptionId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null) throw new KeyNotFoundException($"Абонамент с ID {id} не е намерен.");
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Subscription>> GetActiveSubscriptionsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.Subscriptions
                .Include(s => s.Member)
                .Where(s => s.StartDate <= today && s.EndDate >= today)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Subscription>> GetByMemberIdAsync(int memberId)
        {
            return await _context.Subscriptions
                .Include(s => s.Member)
                .Where(s => s.MemberId == memberId)
                .ToListAsync();
        }
    }
}
