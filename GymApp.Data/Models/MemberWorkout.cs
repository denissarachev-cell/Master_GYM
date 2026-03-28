using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data.Models
{
    /// <summary>
    /// Свързваща таблица между членове и тренировки.
    /// Съдържа час на начало и край.
    /// </summary>
    [Table("members_workouts")]
    public class MemberWorkout
    {
        /// <summary>Идентификатор на члена.</summary>
        [Column("member_id")]
        public int MemberId { get; set; }

        /// <summary>Идентификатор на тренировката.</summary>
        [Column("workout_id")]
        public int WorkoutId { get; set; }

        /// <summary>Час на начало на тренировката.</summary>
        [Column("start_hour")]
        public TimeOnly? StartHour { get; set; }

        /// <summary>Час на край на тренировката.</summary>
        [Column("end_hour")]
        public TimeOnly? EndHour { get; set; }

        /// <summary>Навигационно свойство към член.</summary>
        [ForeignKey(nameof(MemberId))]
        public Member Member { get; set; } = null!;

        /// <summary>Навигационно свойство към тренировка.</summary>
        [ForeignKey(nameof(WorkoutId))]
        public Workout Workout { get; set; } = null!;
    }
}
