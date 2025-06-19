using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using venue_service.Src.Models.Venue;

[Table("schedules")]
public class SchedulesEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("venue_id")]
    public int VenueId { get; set; }

    [ForeignKey("VenueId")]
    public VenueEntity Venue { get; set; }

    [Required]
    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Required]
    [Column("is_reserved")]
    public bool IsReserved { get; set; }

    public ReservationEntity? Reservation { get; set; }
}
