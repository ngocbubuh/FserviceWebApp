using NET1705_FService.Repositories.Interface;
using Quartz;

namespace NET1705_FService.API.RunSchedule.Job
{
    public class ScanningApartmentPackage : IJob
    {
        private readonly ILogger<ScanningApartmentPackage> _logger;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;
        private readonly ISystemLogRepository _systemLogRepo;

        public ScanningApartmentPackage(ILogger<ScanningApartmentPackage> logger,
            IApartmentPackageRepository apartmentPackageRepo, ISystemLogRepository systemLogRepo)
        {
            _logger = logger;
            _apartmentPackageRepo = apartmentPackageRepo;
            _systemLogRepo = systemLogRepo;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("{Now} - Start - Scanning apartment package", DateTime.Now);
                await _apartmentPackageRepo.CheckExpiredAllApartmentpackage();
                _logger.LogInformation("{Now} - Done - Scanning apartment package", DateTime.Now);
                await _systemLogRepo.WriteLog("Scan expired time package done");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the scanning of apartment packages.");
            }
            await Task.CompletedTask;
        }
    }
}
