using System;

namespace freelance.api.Dtos
{
    public class UserForReturnDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime LastActive { get; set; }
    }
}