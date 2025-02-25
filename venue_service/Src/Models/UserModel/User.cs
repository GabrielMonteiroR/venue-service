using System.Data;

namespace venue_service.Src.Models.UserModel
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Role Role { get; set; }
        public bool IsBanned { get; set; }
        public TimeSpan CreatedAt { get; set; }
        public TimeSpan UpdatedAt { get; set; }
        public TimeSpan DeletedAt { get; set; }

        public ICollection<User_Venue> UserVenues { get; set; }
    }
}
