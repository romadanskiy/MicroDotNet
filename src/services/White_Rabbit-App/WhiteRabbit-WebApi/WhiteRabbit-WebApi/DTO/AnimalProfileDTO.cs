namespace WhiteRabbit_WebApi.DTO
{
    public class AnimalProfileDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? About { get; set; }
        public bool Is_booked { get; set; }
        public bool Is_liked { get; set; }
        public string? Address { get; set; }
        //public IFormFile? Photo { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
