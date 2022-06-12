using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace excel_to_database
{
    public partial class vdkContext : DbContext
    {
        public vdkContext()
        {
        }

        public vdkContext(DbContextOptions<vdkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Layout> Layouts { get; set; } = null!;
        public virtual DbSet<Record> Records { get; set; } = null!;
        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost;Port=55000;User Id=postgres;Password=714;Database=vdk;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("cities");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cityname)
                    .HasMaxLength(50)
                    .HasColumnName("cityname");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("companies");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Companyname)
                    .HasMaxLength(50)
                    .HasColumnName("companyname");
            });

            modelBuilder.Entity<Layout>(entity =>
            {
                entity.HasKey(e => e.Accountnumber)
                    .HasName("layouts_pkey");

                entity.ToTable("layouts");

                entity.Property(e => e.Accountnumber).HasColumnName("accountnumber");

                entity.Property(e => e.Companyid).HasColumnName("companyid");

                entity.Property(e => e.Layout1)
                    .HasMaxLength(50)
                    .HasColumnName("layout");

                entity.Property(e => e.Scheme)
                    .HasMaxLength(50)
                    .HasColumnName("scheme");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Layouts)
                    .HasForeignKey(d => d.Companyid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("layouts_companyid_fkey");
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.ToTable("records");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Accountnumber).HasColumnName("accountnumber");

                entity.Property(e => e.Apartmentnumber)
                    .HasMaxLength(50)
                    .HasColumnName("apartmentnumber");

                entity.Property(e => e.Cityid).HasColumnName("cityid");

                entity.Property(e => e.Dateend).HasColumnName("dateend");

                entity.Property(e => e.Datestart).HasColumnName("datestart");

                entity.Property(e => e.Fnp)
                    .HasMaxLength(50)
                    .HasColumnName("fnp");

                entity.Property(e => e.Meterreading).HasColumnName("meterreading");

                entity.Property(e => e.Metertype)
                    .HasMaxLength(50)
                    .HasColumnName("metertype");

                entity.Property(e => e.Regionid).HasColumnName("regionid");

                entity.Property(e => e.Streetadress)
                    .HasMaxLength(50)
                    .HasColumnName("streetadress");

                entity.Property(e => e.Streetnumber)
                    .HasMaxLength(50)
                    .HasColumnName("streetnumber");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("regions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Regionname)
                    .HasMaxLength(50)
                    .HasColumnName("regionname");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Accountnumber)
                    .HasName("users_pkey");

                entity.ToTable("users");

                entity.Property(e => e.Accountnumber)
                    .ValueGeneratedNever()
                    .HasColumnName("accountnumber");

                entity.Property(e => e.Apartmentnumber)
                    .HasMaxLength(50)
                    .HasColumnName("apartmentnumber");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Fnp)
                    .HasMaxLength(50)
                    .HasColumnName("fnp");

                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .HasColumnName("region");

                entity.Property(e => e.Streetadress)
                    .HasMaxLength(50)
                    .HasColumnName("streetadress");

                entity.Property(e => e.Streetnumber)
                    .HasMaxLength(50)
                    .HasColumnName("streetnumber");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
