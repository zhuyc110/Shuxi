using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ShuxiContext : DbContext
    {
        public DbSet<DicomInfoData> DicomInfoDatas { get; set; }

        public DbSet<DicomGeneralInfo> DicomGeneralInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicomInfoData>().ToTable(nameof(DicomInfoData)).HasKey(x => x.ID);
            modelBuilder.Entity<DicomInfoData>().Property(e => e.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<DicomInfoData>().HasIndex(b => b.PerformedProcedureStepID);

            modelBuilder.Entity<DicomGeneralInfo>().ToTable(nameof(DicomGeneralInfo)).HasKey(x => x.Key);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shuxi.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
