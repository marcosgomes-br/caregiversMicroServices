using Hangfire;

namespace Caregivers.Suppliers.Api.Services
{
    public class WorkerService(IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<SchedulerWorker>().Start();
            }
            await Task.CompletedTask;
        }
    }

    public class SchedulerWorker(SupplierService service)
    {
        public void Start()
        {
            Console.WriteLine("---> Serviço Iniciado");
            RecurringJob.AddOrUpdate("listening", () => TakeFreshDataAsync(), Cron.Minutely);
        }

        public void TakeFreshDataAsync() => service.ReceiveMessage();
    }
}
