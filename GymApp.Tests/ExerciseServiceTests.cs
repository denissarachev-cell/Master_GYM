using GymApp.Data.Models;
using GymApp.Services.Implementations;
using NUnit.Framework;

namespace GymApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="ExerciseService"/>.
    /// </summary>
    [TestFixture]
    public class ExerciseServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllExercises()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Push Ups"));
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsExercise()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);

            var exercise = await service.GetByIdAsync(1);

            Assert.That(exercise, Is.Not.Null);
            Assert.That(exercise!.MuscleGroup, Is.EqualTo("Chest"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);

            Assert.That(await service.GetByIdAsync(999), Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidExercise_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);
            var newEx = new Exercise { Name = "Squats", MuscleGroup = "Legs", Equipment = "None" };

            await service.AddAsync(newEx);

            Assert.That((await service.GetAllAsync()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullExercise_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task UpdateAsync_ValidExercise_UpdatesName()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);
            var exercise = await service.GetByIdAsync(1);
            exercise!.Name = "Bench Press";

            await service.UpdateAsync(exercise);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.Name, Is.EqualTo("Bench Press"));
        }

        [Test]
        public async Task GetByMuscleGroupAsync_ReturnsOnlyChest()
        {
            using var context = CreateSeededContext();
            context.Exercises.Add(new Exercise { ExerciseId = 2, Name = "Squats", MuscleGroup = "Legs", Equipment = "None" });
            await context.SaveChangesAsync();

            var service = new ExerciseService(context);
            var result  = (await service.GetByMuscleGroupAsync("Chest")).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Push Ups"));
        }

        [Test]
        public void GetByMuscleGroupAsync_EmptyString_ThrowsArgumentException()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);

            Assert.ThrowsAsync<ArgumentException>(() => service.GetByMuscleGroupAsync(""));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesExercise()
        {
            using var context = CreateContext();
            context.Exercises.Add(new Exercise { ExerciseId = 1, Name = "Test", MuscleGroup = "Core", Equipment = "None" });
            await context.SaveChangesAsync();

            var service = new ExerciseService(context);
            await service.DeleteAsync(1);

            Assert.That((await service.GetAllAsync()), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new ExerciseService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }
    }
}
