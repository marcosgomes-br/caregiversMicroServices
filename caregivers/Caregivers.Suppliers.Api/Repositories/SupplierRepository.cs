using Caregivers.Suppliers.Api.Data;
using Caregivers.Suppliers.Api.Domain;
using Caregivers.Suppliers.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Caregivers.Suppliers.Api.Repositories
{
    public class SupplierRepository(IServiceProvider serviceProvider)
    {
        public List<Supplier> Find(double longitude, double latitude)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<SupplierContext>();
                return _context.Suppliers.Where(x => x.Ray >= Distance.CalculateDistance(latitude, longitude, x.Latitude, x.Longitude)).ToList();
            }
        }
    }
}
