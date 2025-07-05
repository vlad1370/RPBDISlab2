using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RPBDISlab2;

public partial class ToplivoContext : DbContext
{
    public ToplivoContext()
    {
    }

    public ToplivoContext(DbContextOptions<ToplivoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fuel> Fuels { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<Tank> Tanks { get; set; }

    public virtual DbSet<ViewAllOperation> ViewAllOperations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-2L1R9GG\\MSSQLSERVER01;Initial Catalog=toplivo;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fuel>(entity =>
        {
            entity.HasKey(e => e.FuelId).HasName("PK__Fuels__706CF3C7B3E140B1");

            entity.Property(e => e.FuelId).HasColumnName("FuelID");
            entity.Property(e => e.FuelType).HasMaxLength(50);
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__Operatio__A4F5FC6436C6745E");

            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.FuelId).HasColumnName("FuelID");
            entity.Property(e => e.IncExp).HasColumnName("Inc_Exp");
            entity.Property(e => e.TankId).HasColumnName("TankID");

            entity.HasOne(d => d.Fuel).WithMany(p => p.Operations)
                .HasForeignKey(d => d.FuelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Operations_Fuels");

            entity.HasOne(d => d.Tank).WithMany(p => p.Operations)
                .HasForeignKey(d => d.TankId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Operations_Tanks");
        });

        modelBuilder.Entity<Tank>(entity =>
        {
            entity.HasKey(e => e.TankId).HasName("PK__Tanks__97DE70059CD61367");

            entity.Property(e => e.TankId).HasColumnName("TankID");
            entity.Property(e => e.TankMaterial).HasMaxLength(20);
            entity.Property(e => e.TankPicture).HasMaxLength(50);
            entity.Property(e => e.TankType).HasMaxLength(20);
        });

        modelBuilder.Entity<ViewAllOperation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_AllOperations");

            entity.Property(e => e.FuelId).HasColumnName("FuelID");
            entity.Property(e => e.FuelType).HasMaxLength(50);
            entity.Property(e => e.IncExp).HasColumnName("Inc_Exp");
            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.TankId).HasColumnName("TankID");
            entity.Property(e => e.TankType).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
