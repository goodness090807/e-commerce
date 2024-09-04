using e_commerce.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Data.Models.SerialNumber
{
    public class SerialNumberModel
    {
        public int Id { get; set; }
        public SerialNumberType Type { get; set; }
        public string Prefix { get; set; } = string.Empty;
        public int Length { get; set; }
        public int CurrentNumber { get; set; }
        public DateTime LastGeneratedDate { get; set; }

        /// <summary>
        /// 製作樂觀鎖所需的欄位，為了讓多資料庫支援，所以使用Guid，並自己更新
        /// </summary>
        public Guid RowVersion { get; set; }
    }
}
