namespace Caregivers.Suppliers.Api.Domain
{
    public class Supplier
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long Ray { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
