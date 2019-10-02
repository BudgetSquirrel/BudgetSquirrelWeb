﻿// <auto-generated />
using System;
using BudgetTracker.BudgetSquirrel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetTracker.BudgetSquirrel.Web.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.BudgetDurationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DurationType");

                    b.Property<int>("EndDayOfMonth");

                    b.Property<int>("NumberDays");

                    b.Property<bool>("RolloverEndDateOnSmallMonths");

                    b.Property<bool>("RolloverStartDateOnSmallMonths");

                    b.Property<int>("StartDayOfMonth");

                    b.HasKey("Id");

                    b.ToTable("BudgetDurations");
                });

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.BudgetModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BudgetStart");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid>("DurationId");

                    b.Property<decimal>("FundBalance");

                    b.Property<string>("Name");

                    b.Property<Guid>("OwnerId");

                    b.Property<Guid?>("ParentBudgetId");

                    b.Property<double?>("PercentAmount");

                    b.Property<decimal?>("SetAmount");

                    b.HasKey("Id");

                    b.HasIndex("DurationId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ParentBudgetId");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.BudgetPeriodModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<Guid>("RootBudgetId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("RootBudgetId");

                    b.ToTable("BudgetPeriods");
                });

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.TransactionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<Guid>("BudgetId");

                    b.Property<string>("CheckNumber");

                    b.Property<DateTime>("DateOfTransaction");

                    b.Property<string>("Description");

                    b.Property<string>("Notes")
                        .HasMaxLength(500);

                    b.Property<string>("VendorName");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PassWord");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.BudgetModel", b =>
                {
                    b.HasOne("BudgetTracker.Data.EntityFramework.Models.BudgetDurationModel", "Duration")
                        .WithMany("Budgets")
                        .HasForeignKey("DurationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetTracker.Data.EntityFramework.Models.UserModel", "Owner")
                        .WithMany("Budgets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetTracker.Data.EntityFramework.Models.BudgetModel", "ParentBudget")
                        .WithMany()
                        .HasForeignKey("ParentBudgetId");
                });

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.BudgetPeriodModel", b =>
                {
                    b.HasOne("BudgetTracker.Data.EntityFramework.Models.BudgetModel", "RootBudget")
                        .WithMany()
                        .HasForeignKey("RootBudgetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BudgetTracker.Data.EntityFramework.Models.TransactionModel", b =>
                {
                    b.HasOne("BudgetTracker.Data.EntityFramework.Models.BudgetModel", "Budget")
                        .WithMany()
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
