using GymApp.Data.Models;
using GymApp.Services.Implementations;
using NUnit.Framework;

namespace GymApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="WorkoutService"/>.
    /// </summary>
    [TestFixture]
    public class WorkoutServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllWorkouts()
        {
            using var context = CreateSeededContext();
            var service = new WorkoutService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Full Body"));
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsWorkout()
        {
            using var context = CreateSeededContext();
            var service = new WorkoutService(context);

            var workout = await service.GetByIdAsync(1);

            Assert.That(workout, Is.Not.Null);
            Assert.That(workout!.DifficultyLevel, Is.EqualTo("Easy"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new WorkoutService(context);

            Assert.That(await service.GetByIdAsync(999), Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidWorkout_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new WorkoutService(context);
            var newWorkout = new Workout { Name = "Leg Day", DifficultyLevel = "Hard", TrainerId = 1 };

            await service.AddAsync(newWorkout);

            Assert.That((await service.GetAllAsync()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullWorkout_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new WorkoutService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task GetByDifficultyAsync_Easy_ReturnsOnlyEasy()
        {
            using var context = CreateSeededContext();
            context.Workouts.Add(new Workout { WorkoutId = 2, Name = "HIIT", DifficultyLevel = "Hard", TrainerId = 1 });
            await context.SaveChangesAsync();

            var service = new WorkoutService(context);
            var result  = (await service.GetByDifficultyAsync("Easy")).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Full Body"));
        }

        [Test]
        public void GetByDifficultyAsync_EmptyString_ThrowsArgumentException()
        {
            using var context = CreateSeededContext();
            var service = new WorkoutService(context);

            Assert.ThrowsAsync<ArgumentException>(() => service.GetByDifficultyAsync(""));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesWorkout()
        {
            using var context = CreateContext();
            context.Trainers.Add(new Trainer { TrainerId = 1, FirstName = "T", LastName = "T" });
            context.Workouts.Add(new Workout { WorkoutId = 1, Name = "Test", DifficultyLevel = "Easy", TrainerId = 1 });
            await context.SaveChangesAsync();

            var service = new WorkoutService(context);
            await service.DeleteAsync(1);

            Assert.That((await service.GetAllAsync()), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new WorkoutService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public async Task GetWorkoutsWithDetailsAsync_IncludesExercises()
        {
            using var context = CreateSeededContext();
            context.WorkoutExercises.Add(new WorkoutExercise { WorkoutId = 1, ExerciseId = 1, Sets = 3, Reps = 10 });
            await context.SaveChangesAsync();

            var service  = new WorkoutService(context);
            var workouts = (await service.GetWorkoutsWithDetailsAsync()).ToList();

            Assert.That(workouts[0].WorkoutExercises, Is.Not.Empty);
        }
    }
}
