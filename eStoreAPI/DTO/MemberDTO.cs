﻿
namespace eStoreAPI.DTO
{
    public class MemberDTO
    {
        public string Email { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Password { get; set; } = null!;

    }
}
