﻿// <auto-generated />
using System;
using MelonPay.Persisted.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MelonPay.PersistentDb.Migrations.Initial
{
    [DbContext(typeof(MelonPayDbContext))]
    partial class MelonPayDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MelonPay.Common.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CardHolderId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CardHolderId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.CardHolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<int?>("Gender");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("MiddleName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CardHolders");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.Invoice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("FromWalletId");

                    b.Property<string>("Reason");

                    b.Property<int>("StatusId");

                    b.Property<int?>("ToWalletId");

                    b.HasKey("Id");

                    b.HasIndex("FromWalletId");

                    b.HasIndex("StatusId");

                    b.HasIndex("ToWalletId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.InvoiceStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("InvoiceStatuses");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CardHolderId");

                    b.Property<int>("CurrencyId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CardHolderId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Wallet");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Wallet");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.WalletPrivate", b =>
                {
                    b.HasBaseType("MelonPay.Common.Entities.Wallet");

                    b.Property<decimal>("Balance");

                    b.Property<bool>("IsDeleted");

                    b.HasDiscriminator().HasValue("WalletPrivate");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.Account", b =>
                {
                    b.HasOne("MelonPay.Common.Entities.CardHolder", "CardHolder")
                        .WithMany()
                        .HasForeignKey("CardHolderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MelonPay.Common.Entities.Invoice", b =>
                {
                    b.HasOne("MelonPay.Common.Entities.Wallet", "FromWallet")
                        .WithMany()
                        .HasForeignKey("FromWalletId");

                    b.HasOne("MelonPay.Common.Entities.InvoiceStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MelonPay.Common.Entities.Wallet", "ToWallet")
                        .WithMany()
                        .HasForeignKey("ToWalletId");
                });

            modelBuilder.Entity("MelonPay.Common.Entities.Wallet", b =>
                {
                    b.HasOne("MelonPay.Common.Entities.CardHolder", "CardHolder")
                        .WithMany("Wallets")
                        .HasForeignKey("CardHolderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MelonPay.Common.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}