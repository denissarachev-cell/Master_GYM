using GymApp.Data.Models;
using GymApp.Services.Implementations;
using NUnit.Framework;

namespace GymApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="SubscriptionService"/>.
    /// </summary>
    [TestFixture]
    public class SubscriptionServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllSubscriptions()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsSubscription()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);

            var sub = await service.GetByIdAsync(1);

            Assert.That(sub, Is.Not.Null);
            Assert.That(sub!.Type, Is.EqualTo("Monthly"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);

            Assert.That(await service.GetByIdAsync(999), Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidSubscription_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);
            var today   = DateOnly.FromDateTime(DateTime.Today);
            var newSub  = new Subscription
            {
                MemberId  = 1,
                Type      = "Annual",
                Price     = 500m,
                StartDate = today,
                EndDate   = today.AddYears(1),
            };

            await service.AddAsync(newSub);

            Assert.That((await service.GetAllAsync()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullSubscription_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task GetActiveSubscriptionsAsync_ReturnsOnlyActive()
        {
            using var context = CreateSeededContext();
            // Добавяме изтекъл абонамент
            context.Subscriptions.Add(new Subscription
            {
                MemberId  = 1,
                Type      = "Monthly",
                Price     = 50m,
                StartDate = new DateOnly(2020, 1, 1),
                EndDate   = new DateOnly(2020, 1, 31),
            });
            await context.SaveChangesAsync();

            var service = new SubscriptionService(context);
            var active  = (await service.GetActiveSubscriptionsAsync()).ToList();

            Assert.That(active, Has.Count.EqualTo(1));
            Assert.That(active[0].IsActive, Is.True);
        }

        [Test]
        public async Task GetByMemberIdAsync_ReturnsCorrectSubscriptions()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);

            var subs = (await service.GetByMemberIdAsync(1)).ToList();

            Assert.That(subs, Has.Count.EqualTo(1));
            Assert.That(subs[0].MemberId, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_Removes()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);

            await service.DeleteAsync(1);

            Assert.That((await service.GetAllAsync()), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new SubscriptionService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public void IsActive_ActiveSubscription_ReturnsTrue()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var sub   = new Subscription
            {
                StartDate = today.AddDays(-5),
                EndDate   = today.AddDays(5),
            };

            Assert.That(sub.IsActive, Is.True);
        }

        [Test]
        public void IsActive_ExpiredSubscription_ReturnsFalse()
        {
            var sub = new Subscription
            {
                StartDate = new DateOnly(2020, 1, 1),
                EndDate   = new DateOnly(2020, 1, 31),
            };

            Assert.That(sub.IsActive, Is.False);
        }
    }
}
