namespace venue_service.Src.Dtos
{
    public class CreateVenueRequestDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public double Latitude { get; set; }
        public List<IFormFile> Images { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public bool AllowLocalPayment { get; set; }
        public int VenueTypeId { get; set; }
        public string Rules { get; set; }
        public int OwnerId { get; set; }
    }
}
