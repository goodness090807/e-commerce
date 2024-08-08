using e_commerce.Data.Models.User;

namespace e_commerce.Data.Models.RefreshToken
{
    public class RefreshTokenModel : Auditable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }

        public UserModel User { get; set; } = new();
    }
}
