namespace OrderManagement.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerSegment Segment { get; set; }
    }

    public enum CustomerSegment
    {
        Regular,
        VIP,
        Wholesale
    }
}
