using NET1705_FService.Repositories.Interface;
using Quartz;

namespace NET1705_FService.API.RunSchedule.Job
{
    public class ScanningApartmentPackage : IJob
    {
        private readonly ILogger<ScanningApartmentPackage> _logger;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;

        public ScanningApartmentPackage(ILogger<ScanningApartmentPackage> logger,
            IApartmentPackageRepository apartmentPackageRepo)
        {
            _logger = logger;
            _apartmentPackageRepo = apartmentPackageRepo;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("{UtcNow} - Start - Scanning apartment package", DateTime.UtcNow);
                await _apartmentPackageRepo.CheckExpiredAllApartmentpackage();
                _logger.LogInformation("{UtcNow} - Done - Scanning apartment package", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the scanning of apartment packages.");
            }
            await Task.CompletedTask;
        }
    }
}
