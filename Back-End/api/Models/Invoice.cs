namespace api.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public double AmountDue { get; set; }
        public int EventID { get; set; }
        public string EventName {get; set;}
        public int NumberOfEvents {get; set;}

    }
}