namespace venue_service.Src.Dtos
{
    public class VenueResponseDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int capacity { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string Description { get; set; }
        public bool AllowLocalPayment { get; set; }
        public int VenueTypeId { get; set; }
        public string Rules { get; set; }
        public int ownerId { get; set; }
        public int VenueAvaliabilityId { get; set; }
    }
}
