using GymApp.Data.Context;
using GymApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Tests
{
    /// <summary>
    /// Базов клас за тестове — създава изолирана in-memory база данни с тестови данни.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>Създава нова in-memory GymDbContext с уникално име за изолация между тестове.</summary>
        protected static GymDbContext CreateContext(string dbName = "")
        {
            var options = new DbContextOptionsBuilder<GymDbContext>()
                .UseInMemoryDatabase(string.IsNullOrEmpty(dbName) ? Guid.NewGuid().ToString() : dbName)
                .Options;
            return new GymDbContext(options);
        }

        /// <summary>Сийдва базата с тестови данни и връща контекст.</summary>
        protected static GymDbContext CreateSeededContext()
        {
            var context = CreateContext();

            var trainer = new Trainer { TrainerId = 1, FirstName = "Ivan", LastName = "Stanchev", PhoneNumber = 0887111111 };
            context.Trainers.Add(trainer);

            var workout = new Workout { WorkoutId = 1, Name = "Full Body", DifficultyLevel = "Easy", TrainerId = 1 };
            context.Workouts.Add(workout);

            var exercise = new Exercise { ExerciseId = 1, Name = "Push Ups", MuscleGroup = "Chest", Equipment = "None" };
            context.Exercises.Add(exercise);

            var member = new Member
            {
                MembersId   = 1,
                FirstName   = "Pesho",
                LastName    = "Petrov",
                Gender      = "Male",
                BirthDate   = new DateOnly(1995, 1, 1),
                PhoneNumber = 0888111222,
            };
            context.Members.Add(member);

            var today = DateOnly.FromDateTime(DateTime.Today);
            var subscription = new Subscription
            {
                SubscriptionId = 1,
                MemberId       = 1,
                Type           = "Monthly",
                Price          = 50m,
                StartDate      = today.AddDays(-10),
                EndDate        = today.AddDays(20),
            };
            context.Subscriptions.Add(subscription);

            context.SaveChanges();
            return context;
        }
    }
}
