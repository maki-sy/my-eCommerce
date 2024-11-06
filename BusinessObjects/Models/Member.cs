﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string? MemberName { get; set; }
        public string? MemberImage {  get; set; }
        public string Email { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Password { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
