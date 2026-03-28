using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data.Models
{
    /// <summary>
    /// Представлява абонамент на член.
    /// </summary>
    [Table("subscriptions")]
    public class Subscription
    {
        /// <summary>Уникален идентификатор на абонамента.</summary>
        [Key]
        [Column("subscription_id")]
        public int SubscriptionId { get; set; }

        /// <summary>Идентификатор на члена.</summary>
        [Column("member_id")]
        public int MemberId { get; set; }

        /// <summary>Тип абонамент (Monthly / Annual).</summary>
        [Required]
        [MaxLength(100)]
        [Column("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>Цена на абонамента.</summary>
        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        /// <summary>Начална дата.</summary>
        [Required]
        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        /// <summary>Крайна дата.</summary>
        [Required]
        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        /// <summary>Навигационно свойство към член.</summary>
        [ForeignKey(nameof(MemberId))]
        public Member Member { get; set; } = null!;

        /// <summary>Проверява дали абонаментът е активен към днешна дата.</summary>
        public bool IsActive => DateOnly.FromDateTime(DateTime.Today) >= StartDate &&
                                DateOnly.FromDateTime(DateTime.Today) <= EndDate;
    }
}
