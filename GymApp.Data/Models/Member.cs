using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data.Models
{
    /// <summary>
    /// Представлява член на фитнес залата.
    /// </summary>
    [Table("members")]
    public class Member
    {
        /// <summary>Уникален идентификатор на члена.</summary>
        [Key]
        [Column("members_id")]
        public int MembersId { get; set; }

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

        /// <summary>Дата на раждане.</summary>
        [Column("birth_date")]
        public DateOnly? BirthDate { get; set; }

        /// <summary>Пол.</summary>
        [Required]
        [MaxLength(10)]
        [Column("gender")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>Абонаменти на члена.</summary>
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        /// <summary>Тренировки на члена.</summary>
        public ICollection<MemberWorkout> MemberWorkouts { get; set; } = new List<MemberWorkout>();
    }
}
