namespace e_commerce.Data.Models.Store
{
    public class StoreModel : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// 經度
        /// </summary>
        public float Longitude { get; set; }
        /// <summary>
        /// 緯度
        /// </summary>
        public float Latitude { get; set; }
    }
}
