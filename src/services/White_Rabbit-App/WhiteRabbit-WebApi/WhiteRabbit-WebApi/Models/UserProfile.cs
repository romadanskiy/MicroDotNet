namespace WhiteRabbit_WebApi.Models
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
        public string? Date_of_birth { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
