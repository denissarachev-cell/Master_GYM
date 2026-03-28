using GymApp.Data.Models;
using GymApp.Services.Implementations;
using NUnit.Framework;

namespace GymApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="MemberService"/>.
    /// </summary>
    [TestFixture]
    public class MemberServiceTests : TestBase
    {
        // ── GetAllAsync ────────────────────────────────────────────────────────

        [Test]
        public async Task GetAllAsync_ReturnsAllMembers()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].FirstName, Is.EqualTo("Pesho"));
        }

        [Test]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
        {
            using var context = CreateContext();
            var service = new MemberService(context);

            var result = await service.GetAllAsync();

            Assert.That(result, Is.Empty);
        }

        // ── GetByIdAsync ───────────────────────────────────────────────────────

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsMember()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            var member = await service.GetByIdAsync(1);

            Assert.That(member, Is.Not.Null);
            Assert.That(member!.LastName, Is.EqualTo("Petrov"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            var member = await service.GetByIdAsync(999);

            Assert.That(member, Is.Null);
        }

        // ── AddAsync ───────────────────────────────────────────────────────────

        [Test]
        public async Task AddAsync_ValidMember_IncreasesMemberCount()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);
            var newMember = new Member { FirstName = "Maria", LastName = "Ivanova", Gender = "Female" };

            await service.AddAsync(newMember);
            var all = (await service.GetAllAsync()).ToList();

            Assert.That(all, Has.Count.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullMember_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        // ── UpdateAsync ────────────────────────────────────────────────────────

        [Test]
        public async Task UpdateAsync_ValidMember_UpdatesData()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);
            var member = await service.GetByIdAsync(1);
            member!.FirstName = "Georgi";

            await service.UpdateAsync(member);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.FirstName, Is.EqualTo("Georgi"));
        }

        [Test]
        public void UpdateAsync_NullMember_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null!));
        }

        // ── DeleteAsync ────────────────────────────────────────────────────────

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesMember()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            await service.DeleteAsync(1);
            var all = await service.GetAllAsync();

            Assert.That(all, Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        // ── GetActiveMembersAsync ──────────────────────────────────────────────

        [Test]
        public async Task GetActiveMembersAsync_WithActiveSubscription_ReturnsMember()
        {
            using var context = CreateSeededContext();
            var service = new MemberService(context);

            var activeMembers = (await service.GetActiveMembersAsync()).ToList();

            Assert.That(activeMembers, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task GetActiveMembersAsync_ExpiredSubscription_ReturnsEmpty()
        {
            using var context = CreateContext();
            var member = new Member { MembersId = 1, FirstName = "Old", LastName = "Member", Gender = "Male" };
            context.Members.Add(member);
            context.Subscriptions.Add(new Subscription
            {
                MemberId  = 1,
                Type      = "Monthly",
                Price     = 50m,
                StartDate = new DateOnly(2020, 1, 1),
                EndDate   = new DateOnly(2020, 1, 31),
            });
            await context.SaveChangesAsync();

            var service = new MemberService(context);
            var result  = await service.GetActiveMembersAsync();

            Assert.That(result, Is.Empty);
        }
    }
}
