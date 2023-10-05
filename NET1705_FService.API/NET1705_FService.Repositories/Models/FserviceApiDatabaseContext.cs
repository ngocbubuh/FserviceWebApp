using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NET1705_FService.Repositories.Models;

public partial class FserviceApiDatabaseContext : IdentityDbContext<Accounts>
{
    public FserviceApiDatabaseContext()
    {
    }

    public FserviceApiDatabaseContext(DbContextOptions<FserviceApiDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<ApartmentPackage> ApartmentPackages { get; set; }

    public virtual DbSet<ApartmentPackageService> ApartmentPackageServices { get; set; }

    public virtual DbSet<ApartmentType> ApartmentTypes { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Accounts> Accounts { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Floor> Floors { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageDetail> PackageDetails { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    //public virtual DbSet<ServiceJoinStaff> ServiceJoinStaffs { get; set; }

    //public virtual DbSet<Staff> Staff { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=tcp:fserviceapi-databasedbserver.database.windows.net,1433;uid=fservices;pwd=abc@12345;database=FServiceAPI_Database;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.ToTable("Apartment");

            entity.Property(e => e.RoomNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Apartment__Custo__5165187F").IsRequired(false);

            entity.HasOne(d => d.Floor).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.FloorId)
                .HasConstraintName("FK_Apartment_Apartment");

            entity.HasOne(d => d.Type).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Apartment__TypeI__6383C8BA");
        });

        modelBuilder.Entity<ApartmentPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07B63E6DDD");

            entity.ToTable("ApartmentPackage");

            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.Apartment).WithMany(p => p.ApartmentPackages)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK_ApartmentPackage_Apartment");

            entity.HasOne(d => d.Order).WithMany(p => p.ApartmentPackages)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ApartmentPackage_Order");

            entity.HasOne(d => d.Package).WithMany(p => p.ApartmentPackages)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_ApartmentPackage_Package");
        });

        modelBuilder.Entity<ApartmentPackageService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0780F2D6BA");

            entity.ToTable("ApartmentPackageService");

            entity.HasOne(d => d.ApartmentPackage).WithMany(p => p.ApartmentPackageServices)
                .HasForeignKey(d => d.ApartmentPackageId)
                .HasConstraintName("FK__Apartment__Apart__2CF2ADDF");

            entity.HasOne(d => d.Service).WithMany(p => p.ApartmentPackageServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Apartment__Servi__2DE6D218");
        });

        modelBuilder.Entity<ApartmentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Apartmen__3214EC07E07E6E5C");

            entity.ToTable("ApartmentType");

            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.Building).WithMany(p => p.ApartmentTypes)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Apartment__Build__4BAC3F29");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Building__3214EC07AC719328");

            entity.ToTable("Building");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Accounts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07B63E5396");

            entity.ToTable("Account");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Banner_123456ABC");
            entity.ToTable("Banners");

            entity.Property(e => e.Page).HasMaxLength(50);

            entity.Property(e => e.Title).HasMaxLength(500);
        });

        modelBuilder.Entity<Floor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Floors");

            entity.HasOne(d => d.Building).WithMany(p => p.Floors)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Floors_Building");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC0740C46C94");

            entity.ToTable("Order");

            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            //entity.HasOne(d => d.ApartmentPackage).WithMany(p => p.Orders)
            //    .HasForeignKey(d => d.ApartmentPackageId)
            //    .HasConstraintName("FK__Order__Apartment__60A75C0F");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC0741A2FEA3");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.CompleteDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Feedback).HasMaxLength(500);

            entity.HasOne(d => d.ApartmentPackage).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ApartmentPackageId)
                .HasConstraintName("FK_OrderDetail_ApartmentPackage");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__693CA210");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__OrderDeta__Servi__6A30C649");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0754413A46");

            entity.ToTable("Package");

            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Type).WithMany(p => p.Packages)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Package_ApartmentType").IsRequired(false);
        });

        modelBuilder.Entity<PackageDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PackageD__3214EC075597077E");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageDetails)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK__PackageDe__Packa__3587F3E0");

            entity.HasOne(d => d.Service).WithMany(p => p.PackageDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__PackageDe__Servi__05D8E0BE");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC0743C0E74C");

            entity.ToTable("Service");

            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        //modelBuilder.Entity<ServiceJoinStaff>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__ServiceJ__3214EC071F71799A");

        //    entity.ToTable("ServiceJoinStaff");

        //    entity.Property(e => e.DateComplete).HasColumnType("date");
        //    entity.Property(e => e.WorkDate).HasColumnType("date");

        //    entity.HasOne(d => d.Service).WithMany(p => p.ServiceJoinStaffs)
        //        .HasForeignKey(d => d.ServiceId)
        //        .HasConstraintName("FK__ServiceJo__Servi__06CD04F7");

        //    entity.HasOne(d => d.Staff).WithMany(p => p.ServiceJoinStaffs)
        //        .HasForeignKey(d => d.StaffId)
        //        .HasConstraintName("FK__ServiceJo__Staff__07C12930");
        //});

        //modelBuilder.Entity<Staff>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Staff__3214EC0798429772");

        //    entity.Property(e => e.Email).HasMaxLength(50);
        //    entity.Property(e => e.Name).HasMaxLength(50);
        //    entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        //});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
