using Microsoft.EntityFrameworkCore;

#nullable disable

namespace spartan_claim_service.Models
{
    public partial class Claims_dbContext : DbContext
    {
        public Claims_dbContext()
        {
        }

        public Claims_dbContext(DbContextOptions<Claims_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClaimsTest> ClaimsTests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:caliber-spartan-2022.database.windows.net,1433;Initial Catalog=Claims_db;Persist Security Info=False;User ID=SQLAdmin;Password=uwcMf3SR;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ClaimsTest>(entity =>
            {
                entity.ToTable("Claims_test");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.BeneId)
                    .HasMaxLength(50)
                    .HasColumnName("BeneID");

                entity.Property(e => e.ChronicCondAlzheimer).HasColumnName("ChronicCond_Alzheimer");

                entity.Property(e => e.ChronicCondCancer).HasColumnName("ChronicCond_Cancer");

                entity.Property(e => e.ChronicCondDepression).HasColumnName("ChronicCond_Depression");

                entity.Property(e => e.ChronicCondDiabetes).HasColumnName("ChronicCond_Diabetes");

                entity.Property(e => e.ChronicCondHeartfailure).HasColumnName("ChronicCond_Heartfailure");

                entity.Property(e => e.ChronicCondIschemicHeart).HasColumnName("ChronicCond_IschemicHeart");

                entity.Property(e => e.ChronicCondKidneyDisease).HasColumnName("ChronicCond_KidneyDisease");

                entity.Property(e => e.ChronicCondObstrPulmonary).HasColumnName("ChronicCond_ObstrPulmonary");

                entity.Property(e => e.ChronicCondOsteoporasis).HasColumnName("ChronicCond_Osteoporasis");

                entity.Property(e => e.ChronicCondRheumatoidarthritis).HasColumnName("ChronicCond_rheumatoidarthritis");

                entity.Property(e => e.ChronicCondStroke).HasColumnName("ChronicCond_stroke");

                entity.Property(e => e.IpannualDeductibleAmt).HasColumnName("IPAnnualDeductibleAmt");

                entity.Property(e => e.IpannualReimbursementAmt).HasColumnName("IPAnnualReimbursementAmt");

                entity.Property(e => e.IsInpatient).HasColumnName("is_inpatient");

                entity.Property(e => e.OpannualDeductibleAmt).HasColumnName("OPAnnualDeductibleAmt");

                entity.Property(e => e.OpannualReimbursementAmt).HasColumnName("OPAnnualReimbursementAmt");

                entity.Property(e => e.Provider).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
