using Microsoft.EntityFrameworkCore;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;

namespace PMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── DbSets ────────────────────────────────────────────────
        public DbSet<User> Users => Set<User>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<Taluk> Taluks => Set<Taluk>();
        public DbSet<Hobli> Hoblis => Set<Hobli>();
        public DbSet<Village> Villages => Set<Village>();
        public DbSet<Town> Towns => Set<Town>();
        public DbSet<ComplaintCategory> ComplaintCategories => Set<ComplaintCategory>();
        public DbSet<Petition> Petitions => Set<Petition>();
        public DbSet<PetitionAttachment> PetitionAttachments => Set<PetitionAttachment>();
        public DbSet<PetitionWorkflowLog> PetitionWorkflowLogs => Set<PetitionWorkflowLog>();
        public DbSet<ActionReport> ActionReports => Set<ActionReport>();
        public DbSet<ActionReportAttachment> ActionReportAttachments => Set<ActionReportAttachment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── User ─────────────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.UserId);
                e.HasIndex(u => u.MobileNumber).IsUnique();
                e.Property(u => u.MobileNumber).HasMaxLength(15).IsRequired();
                e.Property(u => u.Name).HasMaxLength(200);
                e.Property(u => u.Email).HasMaxLength(200);
                e.Property(u => u.Role).HasConversion<string>().HasMaxLength(50);
                e.Property(u => u.OfficeType).HasConversion<string>().HasMaxLength(50);
            });

            // ── District ─────────────────────────────────────────
            modelBuilder.Entity<District>(e =>
            {
                e.HasKey(d => d.DistrictId);
                e.Property(d => d.Name).HasMaxLength(100).IsRequired();
                e.Property(d => d.NameKN).HasMaxLength(100);
                e.Property(d => d.KGISCode).HasMaxLength(5);
            });

            // ── Taluk ─────────────────────────────────────────────
            modelBuilder.Entity<Taluk>(e =>
            {
                e.HasKey(t => t.TalukId);
                e.Property(t => t.Name).HasMaxLength(100).IsRequired();
                e.Property(t => t.NameKN).HasMaxLength(100);
                e.Property(t => t.MasterCode).HasMaxLength(10);
                e.HasOne(t => t.District)
                 .WithMany(d => d.Taluks)
                 .HasForeignKey(t => t.DistrictId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Hobli ─────────────────────────────────────────────
            modelBuilder.Entity<Hobli>(e =>
            {
                e.HasKey(h => h.HobliId);
                e.Property(h => h.MasterCode).HasMaxLength(10).IsRequired();
                e.Property(h => h.NameEN).HasMaxLength(100).IsRequired();
                e.Property(h => h.NameKN).HasMaxLength(100);
                e.HasOne(h => h.Taluk)
                 .WithMany(t => t.Hoblis)
                 .HasForeignKey(h => h.TalukId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(h => h.District)
                 .WithMany()
                 .HasForeignKey(h => h.DistrictId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            // ── Village ───────────────────────────────────────────
            modelBuilder.Entity<Village>(e =>
            {
                e.HasKey(v => v.VillageId);
                e.Property(v => v.MasterCode).HasMaxLength(15).IsRequired();
                e.Property(v => v.NameEN).HasMaxLength(100).IsRequired();
                e.Property(v => v.NameKN).HasMaxLength(100);
                e.HasOne(v => v.Hobli)
                 .WithMany(h => h.Villages)
                 .HasForeignKey(v => v.HobliId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(v => v.Taluk)
                 .WithMany()
                 .HasForeignKey(v => v.TalukId)
                 .OnDelete(DeleteBehavior.NoAction);
                e.HasOne(v => v.District)
                 .WithMany()
                 .HasForeignKey(v => v.DistrictId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            // ── Town ──────────────────────────────────────────────
            modelBuilder.Entity<Town>(e =>
            {
                e.HasKey(t => t.TownId);
                e.Property(t => t.KGISCode).HasMaxLength(10).IsRequired();
                e.Property(t => t.NameEN).HasMaxLength(100).IsRequired();
                e.Property(t => t.TownType).HasMaxLength(10);
                e.HasOne(t => t.District)
                 .WithMany(d => d.Towns)
                 .HasForeignKey(t => t.DistrictId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── ComplaintCategory ─────────────────────────────────
            modelBuilder.Entity<ComplaintCategory>(e =>
            {
                e.HasKey(c => c.CategoryId);
                e.Property(c => c.Name).HasMaxLength(200).IsRequired();
            });

            // ── Petition ──────────────────────────────────────────
            modelBuilder.Entity<Petition>(e =>
            {
                e.HasKey(p => p.PetitionId);
                e.HasIndex(p => p.PetitionApplicationId).IsUnique();
                e.Property(p => p.PetitionApplicationId).HasMaxLength(30).IsRequired();
                e.Property(p => p.MobileNumber).HasMaxLength(15).IsRequired();
                e.Property(p => p.Status).HasConversion<string>().HasMaxLength(50);
                e.Property(p => p.OARecommendation).HasConversion<string>().HasMaxLength(50);
                e.Property(p => p.RecommendedOfficeType).HasConversion<string>().HasMaxLength(50);
                e.Property(p => p.AssignedOfficeType).HasConversion<string>().HasMaxLength(50);
                e.Property(p => p.Description).HasColumnType("nvarchar(max)");
                e.Property(p => p.OARemarks).HasColumnType("nvarchar(max)");
                e.Property(p => p.APCCFRemarks).HasColumnType("nvarchar(max)");

                // Complainant address
                e.HasOne(p => p.ComplainantDistrict).WithMany().HasForeignKey(p => p.ComplainantDistrictId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.ComplainantTaluk).WithMany().HasForeignKey(p => p.ComplainantTalukId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.ComplainantHobli).WithMany().HasForeignKey(p => p.ComplainantHobliId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.ComplainantVillageNav).WithMany().HasForeignKey(p => p.ComplainantVillageId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.ComplainantTown).WithMany().HasForeignKey(p => p.ComplainantTownId).OnDelete(DeleteBehavior.NoAction);

                // Incident location
                e.HasOne(p => p.LocationDistrict).WithMany().HasForeignKey(p => p.LocationDistrictId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(p => p.LocationTaluk).WithMany().HasForeignKey(p => p.LocationTalukId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.LocationHobli).WithMany().HasForeignKey(p => p.LocationHobliId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.LocationVillageNav).WithMany().HasForeignKey(p => p.LocationVillageId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.LocationTown).WithMany().HasForeignKey(p => p.LocationTownId).OnDelete(DeleteBehavior.NoAction);

                // Category
                e.HasOne(p => p.ComplaintCategory).WithMany().HasForeignKey(p => p.ComplaintCategoryId).OnDelete(DeleteBehavior.Restrict);

                // Officers
                e.HasOne(p => p.AssignedOfficer).WithMany().HasForeignKey(p => p.AssignedOfficerId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.ScrutinizedBy).WithMany().HasForeignKey(p => p.ScrutinizedByUserId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.RecommendedOfficer).WithMany().HasForeignKey(p => p.RecommendedOfficerId).OnDelete(DeleteBehavior.NoAction);

                // Manager & OA assignment
                e.HasOne(p => p.AssignedManager).WithMany().HasForeignKey(p => p.AssignedManagerId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.AssignedOA).WithMany().HasForeignKey(p => p.AssignedOAId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(p => p.SummaryByOA).WithMany().HasForeignKey(p => p.SummaryByOAId).OnDelete(DeleteBehavior.NoAction);

                // New text fields
                e.Property(p => p.ManagerRemarks).HasColumnType("nvarchar(max)");
                e.Property(p => p.APCCFDocumentPath).HasMaxLength(500);
                e.Property(p => p.SummaryRemarks).HasColumnType("nvarchar(max)");
            });

            // ── PetitionAttachment ────────────────────────────────
            modelBuilder.Entity<PetitionAttachment>(e =>
            {
                e.HasKey(a => a.AttachmentId);
                e.Property(a => a.FileType).HasConversion<string>().HasMaxLength(20);
                e.Property(a => a.StoredFileName).HasMaxLength(500).IsRequired();
                e.Property(a => a.OriginalFileName).HasMaxLength(500).IsRequired();
                e.HasOne(a => a.Petition).WithMany(p => p.Attachments).HasForeignKey(a => a.PetitionId).OnDelete(DeleteBehavior.Cascade);
            });

            // ── PetitionWorkflowLog ───────────────────────────────
            modelBuilder.Entity<PetitionWorkflowLog>(e =>
            {
                e.HasKey(l => l.LogId);
                e.Property(l => l.Action).HasMaxLength(100).IsRequired();
                e.Property(l => l.PublicLabel).HasMaxLength(200);
                e.Property(l => l.InternalRemarks).HasColumnType("nvarchar(max)");
                e.Property(l => l.FromStatus).HasConversion<string>().HasMaxLength(50);
                e.Property(l => l.ToStatus).HasConversion<string>().HasMaxLength(50);
                e.HasOne(l => l.Petition).WithMany(p => p.WorkflowLogs).HasForeignKey(l => l.PetitionId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(l => l.ActorUser).WithMany(u => u.WorkflowLogs).HasForeignKey(l => l.ActorUserId).OnDelete(DeleteBehavior.NoAction);
            });

            // ── ActionReport ──────────────────────────────────────
            modelBuilder.Entity<ActionReport>(e =>
            {
                e.HasKey(r => r.ReportId);
                e.Property(r => r.ReportType).HasConversion<string>().HasMaxLength(20);
                e.Property(r => r.ReportText).HasColumnType("nvarchar(max)");
                e.Property(r => r.APCCFDecisionRemarks).HasColumnType("nvarchar(max)");
                e.HasOne(r => r.Petition).WithMany(p => p.ActionReports).HasForeignKey(r => r.PetitionId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(r => r.Officer).WithMany(u => u.ActionReports).HasForeignKey(r => r.OfficerId).OnDelete(DeleteBehavior.Restrict);
            });

            // ── ActionReportAttachment ────────────────────────────
            modelBuilder.Entity<ActionReportAttachment>(e =>
            {
                e.HasKey(a => a.AttachmentId);
                e.Property(a => a.FileType).HasConversion<string>().HasMaxLength(20);
                e.Property(a => a.StoredFileName).HasMaxLength(500).IsRequired();
                e.Property(a => a.OriginalFileName).HasMaxLength(500).IsRequired();
                e.HasOne(a => a.Report).WithMany(r => r.Attachments).HasForeignKey(a => a.ReportId).OnDelete(DeleteBehavior.Cascade);
            });

            // ── Seed only ComplaintCategories (masters seeded via SQL) ──
            modelBuilder.Entity<ComplaintCategory>().HasData(
                new ComplaintCategory { CategoryId = 1, Name = "Illegal Felling of Trees", IsActive = true },
                new ComplaintCategory { CategoryId = 2, Name = "Encroachment of Forest Land", IsActive = true },
                new ComplaintCategory { CategoryId = 3, Name = "Poaching / Wildlife Crime", IsActive = true },
                new ComplaintCategory { CategoryId = 4, Name = "Corruption / Bribery", IsActive = true },
                new ComplaintCategory { CategoryId = 5, Name = "Misappropriation of Funds", IsActive = true },
                new ComplaintCategory { CategoryId = 6, Name = "Illegal Mining / Quarrying", IsActive = true },
                new ComplaintCategory { CategoryId = 7, Name = "Forest Fire (Negligence)", IsActive = true },
                new ComplaintCategory { CategoryId = 8, Name = "Other", IsActive = true }
            );
        }
    }
}