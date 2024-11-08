using BusinessObject.Models;

namespace eStoreAPI.DTO
{
    public class FullOrderDTO
    {
        public byte[]? ImageData { get; set; }
        public string ProductName {  get; set; }
        public int Quantity { get; set; }
        public string? GetImageBase64()
        {
            if (ImageData != null)
            return Convert.ToBase64String(ImageData);
            return null;
        }

    }
}
