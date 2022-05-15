using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ShuxiContext : DbContext
    {
        public DbSet<DicomInfoData> DicomInfoDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicomInfoData>().ToTable(nameof(DicomInfoData)).HasKey(x => x.ID);
            modelBuilder.Entity<DicomInfoData>().Property(e => e.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<DicomInfoData>().HasIndex(b => b.PerformedProcedureStepID);
            modelBuilder.Entity<DicomInfoData>().HasIndex(b => b.OperatorsName);
            modelBuilder.Entity<DicomInfoData>().HasIndex(b => b.PerformingPhysicansName);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shuxi.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
