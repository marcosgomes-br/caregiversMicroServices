namespace Caregivers.Suppliers.Api
{
    public static class Distance
    {
        public static double CalculateDistance(double latitudeConsumer, double longitudeConsumer,
                                        double latitudeSupplier, double longitudeSupplier)
        {
            var d1 = latitudeConsumer * (Math.PI / 180.0);
            var num1 = longitudeConsumer * (Math.PI / 180.0);
            var d2 = latitudeSupplier * (Math.PI / 180.0);
            var num2 = longitudeSupplier * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            var distanceMetros = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            return distanceMetros / 1000;
        }
    }
}
