using Caregivers.Suppliers.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Caregivers.Suppliers.Api.Data
{
    public class SupplierContext : DbContext
    {
        public SupplierContext(DbContextOptions<SupplierContext> options) : base(options) { ; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
