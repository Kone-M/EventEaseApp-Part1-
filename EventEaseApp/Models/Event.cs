using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseApp.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Foreign key - nullable for events not yet assigned
        [Display(Name = "Venue")]
        public int? VenueId { get; set; }

        // Navigation properties
        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }

        // An Event can have many Bookings
        public ICollection<Booking>? Bookings { get; set; }
    }
}