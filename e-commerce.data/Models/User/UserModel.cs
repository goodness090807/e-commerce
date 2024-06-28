namespace e_commerce.data.Models.User
{
    public class UserModel : Auditable
    {
        public int Id { get; set; }
        public string? Account { get; set; }
        public string? HashedPassword { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
