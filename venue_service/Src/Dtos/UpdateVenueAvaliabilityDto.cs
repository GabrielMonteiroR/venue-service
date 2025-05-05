namespace venue_service.Src.Dtos
{
    public class UpdateVenueAvaliabilityDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
    }
}
