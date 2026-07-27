namespace BangiayCAEM.Models
{
    public class CartItem
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }

        // Bổ sung đầy đủ các thuộc tính viết tắt
        public int ProductId => Product?.Id ?? 0;
        public string ImageUrl => Product?.ImageUrl ?? "";
        public string ProductName => Product?.Name ?? "";
        public decimal Price => Product?.Price ?? 0;
        public decimal Total => Price * Quantity;
    }
}