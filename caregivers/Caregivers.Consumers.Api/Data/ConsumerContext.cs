using Caregivers.Consumers.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Caregivers.Consumers.Api.Data
{
    public class ConsumerContext : DbContext
    {
        public ConsumerContext(DbContextOptions<ConsumerContext> options) : base(options) { ; }

        public DbSet<Consumer> Consumers { get; set; }
    }
}
