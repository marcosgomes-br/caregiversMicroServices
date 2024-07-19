namespace Caregivers.Consumers.Api.Domain
{
    public class Consumer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
