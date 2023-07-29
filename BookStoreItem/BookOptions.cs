using System.Globalization;

[assembly: CLSCompliant(true)]

namespace BookStoreItem
{
    public class BookOptions
    {
        public DateTime? Published { get; set; }
        public string BookBinding { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
    }
}
