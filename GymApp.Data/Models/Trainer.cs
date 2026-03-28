using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data.Models
{
    /// <summary>
    /// Представлява треньор във фитнес залата.
    /// </summary>
    [Table("trainers")]
    public class Trainer
    {
        /// <summary>Уникален идентификатор на треньора.</summary>
        [Key]
        [Column("trainer_id")]
        public int TrainerId { get; set; }

        /// <summary>Собствено име.</summary>
        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Фамилно име.</summary>
        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>Телефонен номер (уникален).</summary>
        [Column("phone_number")]
        public long? PhoneNumber { get; set; }

        /// <summary>Тренировки, водени от треньора.</summary>
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
