using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Practice.Models;

public partial class PracticeContext : DbContext
{
    public PracticeContext()
    {
    }

    public PracticeContext(DbContextOptions<PracticeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assortment> Assortments { get; set; }

    public virtual DbSet<Florist> Florists { get; set; }

    public virtual DbSet<Flower> Flowers { get; set; }

    public virtual DbSet<FlowerFlorist> FlowerFlorists { get; set; }

    public virtual DbSet<Plant> Plants { get; set; }

    public virtual DbSet<PlantsAssortmentFlorist> PlantsAssortmentFlorists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShopsForSale> ShopsForSales { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NATBOK\\MSSQLSERVER2;Initial Catalog=Practice;Integrated Security=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assortment>(entity =>
        {
            entity.HasKey(e => e.IdAssortment);

            entity.ToTable("Assortment");

            entity.Property(e => e.IdAssortment).HasColumnName("ID_Assortment");
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(12, 2)");
        });

        modelBuilder.Entity<Florist>(entity =>
        {
            entity.HasKey(e => e.IdFlorist);

            entity.ToTable("Florist");

            entity.Property(e => e.IdFlorist).HasColumnName("ID_Florist");
            entity.Property(e => e.FName)
                .HasMaxLength(50)
                .HasColumnName("F_Name");
            entity.Property(e => e.IdShop).HasColumnName("ID_Shop");
            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.LName)
                .HasMaxLength(50)
                .HasColumnName("L_Name");
            entity.Property(e => e.Patronymic).HasMaxLength(50);

            entity.HasOne(d => d.IdShopNavigation).WithMany(p => p.Florists)
                .HasForeignKey(d => d.IdShop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Florist_Shops_for_sale");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Florists)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Florist_Users");
        });

        modelBuilder.Entity<Flower>(entity =>
        {
            entity.HasKey(e => e.IdFlower);

            entity.ToTable(tb => tb.HasTrigger("Try_Delete"));

            entity.Property(e => e.IdFlower).HasColumnName("ID_Flower");
            entity.Property(e => e.BouquetDesign)
                .HasMaxLength(200)
                .HasColumnName("Bouquet_design");
            entity.Property(e => e.BouquetSize).HasColumnName("Bouquet_size");
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.FlowerSize)
                .HasMaxLength(100)
                .HasColumnName("Flower_size");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Packaging).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ShelfLife)
                .HasMaxLength(50)
                .HasColumnName("Shelf_life");
        });

        modelBuilder.Entity<FlowerFlorist>(entity =>
        {
            entity.HasKey(e => e.IdFlowerShops);

            entity.ToTable("Flower_Florist");

            entity.Property(e => e.IdFlowerShops).HasColumnName("ID_Flower_Shops");
            entity.Property(e => e.IdFlorist).HasColumnName("ID_Florist");
            entity.Property(e => e.IdFlower).HasColumnName("ID_Flower");

            entity.HasOne(d => d.IdFloristNavigation).WithMany(p => p.FlowerFlorists)
                .HasForeignKey(d => d.IdFlorist)
                .HasConstraintName("FK_Flower_Florist_Florist");

            entity.HasOne(d => d.IdFlowerNavigation).WithMany(p => p.FlowerFlorists)
                .HasForeignKey(d => d.IdFlower)
                .HasConstraintName("FK_Flower_Florist_Flowers");
        });


        modelBuilder.Entity<Plant>(entity =>
        {
            entity.HasKey(e => e.IdPlant);

            entity.Property(e => e.IdPlant).HasColumnName("ID_Plant");
            entity.Property(e => e.Color).HasMaxLength(100);
            entity.Property(e => e.FlowerShape)
                .HasMaxLength(200)
                .HasColumnName("Flower_shape");
            entity.Property(e => e.FlowerSize)
                .HasMaxLength(200)
                .HasColumnName("Flower_size");
            entity.Property(e => e.FloweringPeriod)
                .HasMaxLength(200)
                .HasColumnName("Flowering_period");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.RequieredSoil)
                .HasMaxLength(300)
                .HasColumnName("Requiered_soil");
            entity.Property(e => e.ShelfLife)
                .HasMaxLength(300)
                .HasColumnName("Shelf_life");
        });

        modelBuilder.Entity<PlantsAssortmentFlorist>(entity =>
        {
            entity.HasKey(e => e.IdPlantShopAssortment);

            entity.ToTable("Plants_Assortment_Florist");

            entity.Property(e => e.IdPlantShopAssortment).HasColumnName("ID_Plant_Shop_Assortment");
            entity.Property(e => e.IdAssortment).HasColumnName("ID_Assortment");
            entity.Property(e => e.IdFlorist).HasColumnName("ID_Florist");
            entity.Property(e => e.IdPlant).HasColumnName("ID_Plant");

            entity.HasOne(d => d.IdAssortmentNavigation).WithMany(p => p.PlantsAssortmentFlorists)
                .HasForeignKey(d => d.IdAssortment)
                .HasConstraintName("FK_Plants_Assortment_Florist_Assortment");

            entity.HasOne(d => d.IdFloristNavigation).WithMany(p => p.PlantsAssortmentFlorists)
                .HasForeignKey(d => d.IdFlorist)
                .HasConstraintName("FK_Plants_Assortment_Florist_Florist");

            entity.HasOne(d => d.IdPlantNavigation).WithMany(p => p.PlantsAssortmentFlorists)
                .HasForeignKey(d => d.IdPlant)
                .HasConstraintName("FK_Plants_Assortment_Florist_Plants");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole);

            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<ShopsForSale>(entity =>
        {
            entity.HasKey(e => e.IdShop);

            entity.ToTable("Shops_for_sale");

            entity.Property(e => e.IdShop).HasColumnName("ID_Shop");
            entity.Property(e => e.AreaOfTradingFloor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Area_of_trading_floor");
            entity.Property(e => e.AvailabilityOfOrders)
                .HasMaxLength(50)
                .HasColumnName("Availability_of_orders");
            entity.Property(e => e.Building).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Index).HasMaxLength(50);
            entity.Property(e => e.NameOfShop)
                .HasMaxLength(100)
                .HasColumnName("Name_of_shop");
            entity.Property(e => e.Street).HasMaxLength(70);
            entity.Property(e => e.TypeOfSale)
                .HasMaxLength(20)
                .HasColumnName("Type_of_sale");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(50)
                .HasColumnName("User_Login");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(50)
                .HasColumnName("User_Password");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
