namespace eStoreAPI.DTO
{
    public class OrderDTO
    {
        public int MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequireDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public double Total { get; set; }
    }
}
