﻿using e_commerce.Data.Models.RefreshToken;

namespace e_commerce.Data.Models.User
{
    public class UserModel : Auditable
    {
        public int Id { get; set; }
        public string HashedPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public ICollection<RefreshTokenModel>? RefreshTokens { get; set; }
    }
}
