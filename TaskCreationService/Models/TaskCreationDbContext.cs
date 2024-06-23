using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TaskCreationService.Models
{
    public class TaskCreationDbContext:DbContext
    {
        public TaskCreationDbContext(DbContextOptions<TaskCreationDbContext> options) : base(options)
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
        public DbSet<TaskDetail> TaskDetails { get; set; }
        public DbSet<VUserName> VUserName { get; set; }
        public DbSet<VTaskDetail> VTaskDetails { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<IssueType> IssueType { get; set; }
    }
}
