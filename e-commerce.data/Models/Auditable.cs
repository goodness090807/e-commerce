namespace e_commerce.data.Models
{
    public class Auditable : IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
