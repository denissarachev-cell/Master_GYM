using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data.Models
{
    /// <summary>
    /// Представлява тренировъчна програма.
    /// </summary>
    [Table("workouts")]
    public class Workout
    {
        /// <summary>Уникален идентификатор на тренировката.</summary>
        [Key]
        [Column("workout_id")]
        public int WorkoutId { get; set; }

        /// <summary>Наименование на тренировката.</summary>
        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Ниво на трудност (Easy, Medium, Hard).</summary>
        [Required]
        [MaxLength(100)]
        [Column("difficulty_level")]
        public string DifficultyLevel { get; set; } = string.Empty;

        /// <summary>Идентификатор на треньора.</summary>
        [Column("trainer_id")]
        public int TrainerId { get; set; }

        /// <summary>Треньор, водещ тренировката.</summary>
        [ForeignKey(nameof(TrainerId))]
        public Trainer Trainer { get; set; } = null!;

        /// <summary>Упражнения в тренировката.</summary>
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

        /// <summary>Членове, записани за тренировката.</summary>
        public ICollection<MemberWorkout> MemberWorkouts { get; set; } = new List<MemberWorkout>();
    }
}
