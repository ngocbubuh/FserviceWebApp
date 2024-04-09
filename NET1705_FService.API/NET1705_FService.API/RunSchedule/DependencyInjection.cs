using NET1705_FService.API.RunSchedule.Setup;
using Quartz;

namespace NET1705_FService.API.RunSchedule
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(option =>
            {
                option.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(option =>
            {
                option.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<ScanningApartmentPackageSetup>();
        }
    }
}
