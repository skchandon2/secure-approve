using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace secure_approve.Models.localdb
{
    public partial class secureApproveDBContext : DbContext
    {
        public secureApproveDBContext()
        {
        }

        public secureApproveDBContext(DbContextOptions<secureApproveDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<ApproveRejectAction> ApproveRejectActions { get; set; }
        public virtual DbSet<RequestForm> RequestForms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-V1T05S7;Database=secureapproval;Trusted_Connection=True;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.HasKey(e => e.Roleid)
                    .HasName("PK__appRoles__CD994BF275A79109");

                entity.ToTable("appRoles", "userInfo");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Rolename)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("rolename");
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PK__appUsers__CBA1B2575EDC953C");

                entity.ToTable("appUsers", "userInfo");

                entity.HasIndex(e => e.UserRoleId, "NonClustered_userInfoAppUsersRoles");

                entity.HasIndex(e => e.Username, "NonClustered_userInfoAppUsersUsername");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.UserRoleId).HasColumnName("userRoleId");

                entity.Property(e => e.Useremail)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("useremail");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("username");

                entity.Property(e => e.Userpass)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("userpass");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_userroleid");
            });

            modelBuilder.Entity<ApproveRejectAction>(entity =>
            {
                entity.HasKey(e => e.ActionId)
                    .HasName("PK__ApproveR__004C68E36791B135");

                entity.ToTable("ApproveRejectActions", "workflow");

                entity.HasIndex(e => e.ApprovalDate, "NonClustered_approvalrequestByApprovalDate");

                entity.HasIndex(e => e.ApproverEmail, "NonClustered_approvalrequestByApproverEmail");

                entity.HasIndex(e => e.RequestedAmount, "NonClustered_approvalrequestByRequestAmount");

                entity.HasIndex(e => e.RequestFormId, "NonClustered_approvalrequestByRequestFormId");

                entity.HasIndex(e => e.RequestorEmail, "NonClustered_approvalrequestByRequestorEmail");

                entity.HasIndex(e => e.IsApproved, "NonClustered_approvalrequestByisApproved");

                entity.HasIndex(e => e.IsRejected, "NonClustered_approvalrequestByisRejected");

                entity.Property(e => e.ActionId).HasColumnName("actionId");

                entity.Property(e => e.ApprovalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approvalDate");

                entity.Property(e => e.ApprovalRejectWithUserDataHash)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("approvalRejectWithUserDataHash");

                entity.Property(e => e.ApproverEmail)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("approverEmail");

                entity.Property(e => e.ApproverUsername)
                    .HasMaxLength(300)
                    .HasColumnName("approverUsername");

                entity.Property(e => e.IsApproved).HasColumnName("isApproved");

                entity.Property(e => e.IsRejected).HasColumnName("isRejected");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasColumnName("requestDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RequestFormId).HasColumnName("requestFormId");

                entity.Property(e => e.RequestReason)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("requestReason");

                entity.Property(e => e.RequestedAmount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("requestedAmount");

                entity.Property(e => e.RequestorEmail)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("requestorEmail");

                entity.Property(e => e.UserDataHash)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("userDataHash");

                entity.HasOne(d => d.RequestForm)
                    .WithMany(p => p.ApproveRejectActions)
                    .HasForeignKey(d => d.RequestFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_requestId");
            });

            modelBuilder.Entity<RequestForm>(entity =>
            {
                entity.ToTable("RequestForm", "workflow");

                entity.HasIndex(e => e.RequestedAmount, "NonClustered_RequestFormAmount");

                entity.HasIndex(e => e.RequestorEmail, "NonClustered_RequestFormEmail");

                entity.Property(e => e.RequestFormId).HasColumnName("requestFormId");

                entity.Property(e => e.IsSubmitted).HasColumnName("isSubmitted");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasColumnName("requestDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RequestReason)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("requestReason");

                entity.Property(e => e.RequestedAmount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("requestedAmount");

                entity.Property(e => e.RequestorEmail)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("requestorEmail");

                entity.Property(e => e.RequestorUsername)
                    .HasMaxLength(300)
                    .HasColumnName("requestorUsername");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
