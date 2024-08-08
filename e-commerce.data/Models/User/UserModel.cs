using e_commerce.Data.Models.Product;
using e_commerce.Data.Models.RefreshToken;

namespace e_commerce.Data.Models.User
{
    public class UserModel : Auditable
    {
        public int Id { get; set; }
        /// <summary>
        /// Hash過的密碼
        /// </summary>
        public string HashedPassword { get; set; } = string.Empty;
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// 帳號名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 是否驗證
        /// </summary>
        public bool Valid { get; set; } = false;

        public RefreshTokenModel? RefreshToken { get; set; } = null;
        public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
