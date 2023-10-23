﻿// <auto-generated />
using System;
using CyptoWallet.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CyptoWallet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231021190743_seeder")]
    partial class seeder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CyptoWallet.Entities.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"), 1L, 1);

                    b.Property<int>("AccountTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CBU")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CryptoAddress")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AccountId");

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("CyptoWallet.Entities.AccountType", b =>
                {
                    b.Property<int>("AccountTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountTypeId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountTypeId");

                    b.ToTable("AccountType");

                    b.HasData(
                        new
                        {
                            AccountTypeId = 1,
                            Name = "Pesos"
                        },
                        new
                        {
                            AccountTypeId = 2,
                            Name = "Dólares"
                        },
                        new
                        {
                            AccountTypeId = 3,
                            Name = "BTC"
                        });
                });

            modelBuilder.Entity("CyptoWallet.Entities.Operation", b =>
                {
                    b.Property<int>("OperationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OperationId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("DestinationAccountId")
                        .HasColumnType("int");

                    b.Property<int>("OperationTypeId")
                        .HasColumnType("int");

                    b.Property<int>("SourceAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OperationId");

                    b.HasIndex("DestinationAccountId");

                    b.HasIndex("OperationTypeId");

                    b.HasIndex("SourceAccountId");

                    b.HasIndex("UserId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("CyptoWallet.Entities.OperationType", b =>
                {
                    b.Property<int>("OperationTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OperationTypeId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("OperationTypeId");

                    b.ToTable("OperationType");

                    b.HasData(
                        new
                        {
                            OperationTypeId = 1,
                            Name = "Transferencia"
                        },
                        new
                        {
                            OperationTypeId = 2,
                            Name = "Compra"
                        },
                        new
                        {
                            OperationTypeId = 3,
                            Name = "Depósito"
                        },
                        new
                        {
                            OperationTypeId = 4,
                            Name = "Venta"
                        });
                });

            modelBuilder.Entity("CyptoWallet.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"), 1L, 1);

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            Activo = true,
                            Description = "Administrador",
                            Name = "Admin"
                        },
                        new
                        {
                            RoleId = 2,
                            Activo = true,
                            Description = "Consultor",
                            Name = "Consultor"
                        });
                });

            modelBuilder.Entity("CyptoWallet.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DNI")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaBaja")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Activo = true,
                            Apellido = "Marx",
                            DNI = 40123456,
                            Email = "tecno@gmail.com",
                            FechaBaja = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Carlos",
                            Password = "2073aeb3d12caffb41cfcee9a2e8bdb7c7ec1f9b67d57b06c554717526fa58d4",
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("CyptoWallet.Entities.Account", b =>
                {
                    b.HasOne("CyptoWallet.Entities.AccountType", "AccountType")
                        .WithMany("Accounts")
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CyptoWallet.Entities.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CyptoWallet.Entities.Operation", b =>
                {
                    b.HasOne("CyptoWallet.Entities.Account", "DestinationAccount")
                        .WithMany()
                        .HasForeignKey("DestinationAccountId");

                    b.HasOne("CyptoWallet.Entities.OperationType", "OperationType")
                        .WithMany("Operations")
                        .HasForeignKey("OperationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CyptoWallet.Entities.Account", "SourceAccount")
                        .WithMany()
                        .HasForeignKey("SourceAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CyptoWallet.Entities.User", "User")
                        .WithMany("Operations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinationAccount");

                    b.Navigation("OperationType");

                    b.Navigation("SourceAccount");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CyptoWallet.Entities.User", b =>
                {
                    b.HasOne("CyptoWallet.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CyptoWallet.Entities.AccountType", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("CyptoWallet.Entities.OperationType", b =>
                {
                    b.Navigation("Operations");
                });

            modelBuilder.Entity("CyptoWallet.Entities.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Operations");
                });
#pragma warning restore 612, 618
        }
    }
}
