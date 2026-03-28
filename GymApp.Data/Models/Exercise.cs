using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data.Models
{
    /// <summary>
    /// Представлява упражнение.
    /// </summary>
    [Table("exercises")]
    public class Exercise
    {
        /// <summary>Уникален идентификатор на упражнението.</summary>
        [Key]
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        /// <summary>Наименование на упражнението.</summary>
        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Мускулна група, която упражнява.</summary>
        [Required]
        [MaxLength(100)]
        [Column("muscle_group")]
        public string MuscleGroup { get; set; } = string.Empty;

        /// <summary>Необходимо оборудване.</summary>
        [Required]
        [MaxLength(100)]
        [Column("equipment")]
        public string Equipment { get; set; } = string.Empty;

        /// <summary>Тренировки, съдържащи това упражнение.</summary>
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
