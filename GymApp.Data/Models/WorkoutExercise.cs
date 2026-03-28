using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data.Models
{
    /// <summary>
    /// Свързваща таблица между тренировки и упражнения.
    /// Съдържа брой серии и повторения.
    /// </summary>
    [Table("workout_exercises")]
    public class WorkoutExercise
    {
        /// <summary>Идентификатор на тренировката.</summary>
        [Column("workout_id")]
        public int WorkoutId { get; set; }

        /// <summary>Идентификатор на упражнението.</summary>
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        /// <summary>Брой серии.</summary>
        [Required]
        [Column("sets")]
        public int Sets { get; set; }

        /// <summary>Брой повторения.</summary>
        [Required]
        [Column("reps")]
        public int Reps { get; set; }

        /// <summary>Навигационно свойство към тренировка.</summary>
        [ForeignKey(nameof(WorkoutId))]
        public Workout Workout { get; set; } = null!;

        /// <summary>Навигационно свойство към упражнение.</summary>
        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; } = null!;
    }
}
