using GymApp.Data.Models;
using GymApp.Services.Implementations;
using NUnit.Framework;

namespace GymApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="TrainerService"/>.
    /// </summary>
    [TestFixture]
    public class TrainerServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllTrainers()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].LastName, Is.EqualTo("Stanchev"));
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsTrainer()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);

            var trainer = await service.GetByIdAsync(1);

            Assert.That(trainer, Is.Not.Null);
            Assert.That(trainer!.FirstName, Is.EqualTo("Ivan"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);

            var trainer = await service.GetByIdAsync(999);

            Assert.That(trainer, Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidTrainer_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);
            var newTrainer = new Trainer { FirstName = "Petya", LastName = "Kirova" };

            await service.AddAsync(newTrainer);
            var all = (await service.GetAllAsync()).ToList();

            Assert.That(all, Has.Count.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullTrainer_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task UpdateAsync_ValidTrainer_ChangesName()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);
            var trainer = await service.GetByIdAsync(1);
            trainer!.FirstName = "Hristo";

            await service.UpdateAsync(trainer);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.FirstName, Is.EqualTo("Hristo"));
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesTrainer()
        {
            // Нужен е отделен контекст без FK зависимости
            using var context = CreateContext();
            context.Trainers.Add(new Trainer { TrainerId = 5, FirstName = "Test", LastName = "Trainer" });
            await context.SaveChangesAsync();

            var service = new TrainerService(context);
            await service.DeleteAsync(5);

            var all = await service.GetAllAsync();
            Assert.That(all, Is.Empty);
        }

        [Test]
        public async Task GetTrainersWithWorkoutsAsync_ReturnsTrainersAndWorkouts()
        {
            using var context = CreateSeededContext();
            var service = new TrainerService(context);

            var trainers = (await service.GetTrainersWithWorkoutsAsync()).ToList();

            Assert.That(trainers, Is.Not.Empty);
            Assert.That(trainers[0].Workouts, Is.Not.Null);
        }
    }
}
