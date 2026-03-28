using GymApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Data.Context
{
    /// <summary>
    /// Entity Framework DbContext за фитнес залата.
    /// </summary>
    public class GymDbContext : DbContext
    {
        /// <summary>Инициализира нова инстанция на <see cref="GymDbContext"/>.</summary>
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

        /// <summary>Членове на фитнес залата.</summary>
        public DbSet<Member> Members { get; set; }

        /// <summary>Треньори.</summary>
        public DbSet<Trainer> Trainers { get; set; }

        /// <summary>Тренировъчни програми.</summary>
        public DbSet<Workout> Workouts { get; set; }

        /// <summary>Упражнения.</summary>
        public DbSet<Exercise> Exercises { get; set; }

        /// <summary>Упражнения в тренировките.</summary>
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }

        /// <summary>Записвания на членове за тренировки.</summary>
        public DbSet<MemberWorkout> MemberWorkouts { get; set; }

        /// <summary>Абонаменти.</summary>
        public DbSet<Subscription> Subscriptions { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite primary key за WorkoutExercise
            modelBuilder.Entity<WorkoutExercise>()
                .HasKey(we => new { we.WorkoutId, we.ExerciseId });

            // Composite primary key за MemberWorkout
            modelBuilder.Entity<MemberWorkout>()
                .HasKey(mw => new { mw.MemberId, mw.WorkoutId });

            // Конфигурация на връзки
            modelBuilder.Entity<Workout>()
                .HasOne(w => w.Trainer)
                .WithMany(t => t.Workouts)
                .HasForeignKey(w => w.TrainerId);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId);

            modelBuilder.Entity<MemberWorkout>()
                .HasOne(mw => mw.Member)
                .WithMany(m => m.MemberWorkouts)
                .HasForeignKey(mw => mw.MemberId);

            modelBuilder.Entity<MemberWorkout>()
                .HasOne(mw => mw.Workout)
                .WithMany(w => w.MemberWorkouts)
                .HasForeignKey(mw => mw.WorkoutId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Member)
                .WithMany(m => m.Subscriptions)
                .HasForeignKey(s => s.MemberId);
        }
    }
}
