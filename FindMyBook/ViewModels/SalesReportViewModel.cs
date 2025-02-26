using System;

namespace FindMyBook.Models
{
    public class SalesReportViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string BookName { get; set; }
        public double BookPrice { get; set; }
    }
}
