namespace TourBookingManagment.Model
{
    public class CurrencyConversionModel
    {
        public decimal Rate { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}