using Microsoft.EntityFrameworkCore;

namespace TaskAssignmentAndNotificationService.Models
{
    public class AssignmentAndNotificationDbContext:DbContext
    {
        public AssignmentAndNotificationDbContext(DbContextOptions<AssignmentAndNotificationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
          .Entity<VTaskDetail>(eb =>
          {
              eb.HasNoKey();
              eb.ToView("VTaskDetails");
          });
        }
        public DbSet<VTaskDetail> TaskDetails { get; set; }

    }
}
