using Auth.Api.DAL;
using Microsoft.EntityFrameworkCore;

namespace Auth.Migrator
{
    public class MigrationService(
        //ILogger<MigrationService> logger,
        IDbContextFactory<AuthDbContext> dbContextFactory)
    {
        //private readonly ILogger<MigrationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        private readonly IDbContextFactory<AuthDbContext> _dbContextFactory =
            dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
                if (pendingMigrations.Any())
                {
                    //_logger.LogInformation("Pending migrations found: {PendingMigrationsCount}.", pendingMigrations.Count);

                    foreach (var migration in pendingMigrations)
                    {
                        //_logger.LogInformation("Applying migration: {MigrationName}", migration);
                    }

                    await dbContext.Database.MigrateAsync(cancellationToken);
                    //_logger.LogInformation("Migrations have been applied successfully.");
                }
                else
                {
                    //_logger.LogInformation("No pending migrations found. Database is up to date.");
                }
            }
            catch (Exception e)
            {
                //_logger.LogCritical(e, "Error occured while migrating database.");
            }
        }
    }
}
