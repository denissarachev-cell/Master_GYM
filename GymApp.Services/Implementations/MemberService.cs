using GymApp.Data.Context;
using GymApp.Data.Models;
using GymApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на членове.
    /// </summary>
    public class MemberService : IMemberService
    {
        private readonly GymDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="MemberService"/>.</summary>
        public MemberService(GymDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) throw new KeyNotFoundException($"Член с ID {id} не е намерен.");
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.Members
                .Include(m => m.Subscriptions)
                .Where(m => m.Subscriptions.Any(s => s.StartDate <= today && s.EndDate >= today))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Member>> GetMembersWithWorkoutsAsync()
        {
            return await _context.Members
                .Include(m => m.MemberWorkouts)
                    .ThenInclude(mw => mw.Workout)
                .ToListAsync();
        }
    }
}
