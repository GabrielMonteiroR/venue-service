namespace venue_service.Src.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Role role { get; set; }
        public bool isBanned { get; set; }
        public DateTime createdAt {  get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime deletedAt { get; set; }

    }
}
