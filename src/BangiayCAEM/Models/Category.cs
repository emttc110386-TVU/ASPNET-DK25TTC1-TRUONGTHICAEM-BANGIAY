using System.Collections.Generic;

namespace TrasuaMON.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } // 👈 Thêm dòng này
        public ICollection<Product>? Products { get; set; }
    }
}