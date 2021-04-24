﻿// <auto-generated />
using System;
using Garden.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Garden.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Garden.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileExt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("FileSize")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("Garden.Models.BaseSubType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BaseTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BaseTypeId");

                    b.ToTable("BaseSubType");
                });

            modelBuilder.Entity("Garden.Models.BaseType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSubTypeEditable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BaseType");
                });

            modelBuilder.Entity("Garden.Models.GardenAttachMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("GardenId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("GardenId");

                    b.ToTable("GardenAttachMap");
                });

            modelBuilder.Entity("Garden.Models.GardenRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GardenId")
                        .HasColumnType("int");

                    b.Property<string>("SubTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("GardenId");

                    b.HasIndex("SubTypeId");

                    b.ToTable("GardenRole");
                });

            modelBuilder.Entity("Garden.Models.GardenSpace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActivate")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SubTypeId");

                    b.ToTable("GardenSpace");
                });

            modelBuilder.Entity("Garden.Models.GardenTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GardenSpaceId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActivate")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SubTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("GardenSpaceId");

                    b.HasIndex("RegUserId");

                    b.HasIndex("SubTypeId");

                    b.ToTable("GardenTask");
                });

            modelBuilder.Entity("Garden.Models.GardenTaskAttachMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("GardenId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("GardenId");

                    b.ToTable("GardenTaskAttachMap");
                });

            modelBuilder.Entity("Garden.Models.GardenUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GardenRoleId")
                        .HasColumnType("int");

                    b.Property<int?>("GardenSpaceId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActivate")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("GardenRoleId");

                    b.HasIndex("GardenSpaceId");

                    b.HasIndex("UserId");

                    b.ToTable("GardenUser");
                });

            modelBuilder.Entity("Garden.Models.GardenUserTaskMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GardenManagerId")
                        .HasColumnType("int");

                    b.Property<int?>("GardenTaskId")
                        .HasColumnType("int");

                    b.Property<int?>("GardenUserId")
                        .HasColumnType("int");

                    b.Property<int?>("GardenWorkTimeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TaskDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskWeek")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GardenManagerId");

                    b.HasIndex("GardenTaskId");

                    b.HasIndex("GardenUserId");

                    b.HasIndex("GardenWorkTimeId");

                    b.ToTable("GardenUserTaskMap");
                });

            modelBuilder.Entity("Garden.Models.GardenWorkTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GardenSpaceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GardenSpaceId");

                    b.ToTable("GardenWorkTime");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Garden.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Garden.Models.BaseSubType", b =>
                {
                    b.HasOne("Garden.Models.BaseType", "BaseType")
                        .WithMany("baseSubTypes")
                        .HasForeignKey("BaseTypeId");

                    b.Navigation("BaseType");
                });

            modelBuilder.Entity("Garden.Models.GardenAttachMap", b =>
                {
                    b.HasOne("Garden.Models.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garden.Models.GardenSpace", "Garden")
                        .WithMany()
                        .HasForeignKey("GardenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("Garden");
                });

            modelBuilder.Entity("Garden.Models.GardenRole", b =>
                {
                    b.HasOne("Garden.Models.GardenSpace", "Garden")
                        .WithMany()
                        .HasForeignKey("GardenId");

                    b.HasOne("Garden.Models.BaseSubType", "BaseSubType")
                        .WithMany()
                        .HasForeignKey("SubTypeId");

                    b.Navigation("BaseSubType");

                    b.Navigation("Garden");
                });

            modelBuilder.Entity("Garden.Models.GardenSpace", b =>
                {
                    b.HasOne("Garden.Models.BaseSubType", "BaseSubType")
                        .WithMany()
                        .HasForeignKey("SubTypeId");

                    b.Navigation("BaseSubType");
                });

            modelBuilder.Entity("Garden.Models.GardenTask", b =>
                {
                    b.HasOne("Garden.Models.GardenSpace", "GardenSpace")
                        .WithMany("GardenTasks")
                        .HasForeignKey("GardenSpaceId");

                    b.HasOne("Garden.Models.ApplicationUser", "RegUser")
                        .WithMany()
                        .HasForeignKey("RegUserId");

                    b.HasOne("Garden.Models.BaseSubType", "BaseSubType")
                        .WithMany()
                        .HasForeignKey("SubTypeId");

                    b.Navigation("BaseSubType");

                    b.Navigation("GardenSpace");

                    b.Navigation("RegUser");
                });

            modelBuilder.Entity("Garden.Models.GardenTaskAttachMap", b =>
                {
                    b.HasOne("Garden.Models.Attachment", "Attachment")
                        .WithMany("GardenTaskAttachMaps")
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garden.Models.GardenSpace", "GardenSpace")
                        .WithMany("GardenTaskAttachMaps")
                        .HasForeignKey("GardenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("GardenSpace");
                });

            modelBuilder.Entity("Garden.Models.GardenUser", b =>
                {
                    b.HasOne("Garden.Models.GardenRole", "GardenRole")
                        .WithMany()
                        .HasForeignKey("GardenRoleId");

                    b.HasOne("Garden.Models.GardenSpace", "GardenSpace")
                        .WithMany("GardenUsers")
                        .HasForeignKey("GardenSpaceId");

                    b.HasOne("Garden.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("GardenRole");

                    b.Navigation("GardenSpace");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Garden.Models.GardenUserTaskMap", b =>
                {
                    b.HasOne("Garden.Models.GardenUser", "GardenManagerTask")
                        .WithMany("GardenManagerTasks")
                        .HasForeignKey("GardenManagerId");

                    b.HasOne("Garden.Models.GardenTask", "GardenTask")
                        .WithMany("GardenUserTaskMaps")
                        .HasForeignKey("GardenTaskId");

                    b.HasOne("Garden.Models.GardenUser", "GardenUserTask")
                        .WithMany("GardenUserTasks")
                        .HasForeignKey("GardenUserId");

                    b.HasOne("Garden.Models.GardenWorkTime", "GardenWorkTime")
                        .WithMany("GardenUserTaskMaps")
                        .HasForeignKey("GardenWorkTimeId");

                    b.Navigation("GardenManagerTask");

                    b.Navigation("GardenTask");

                    b.Navigation("GardenUserTask");

                    b.Navigation("GardenWorkTime");
                });

            modelBuilder.Entity("Garden.Models.GardenWorkTime", b =>
                {
                    b.HasOne("Garden.Models.GardenSpace", "GardenSpace")
                        .WithMany("GardenWorkTimes")
                        .HasForeignKey("GardenSpaceId");

                    b.Navigation("GardenSpace");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Garden.Models.Attachment", b =>
                {
                    b.Navigation("GardenTaskAttachMaps");
                });

            modelBuilder.Entity("Garden.Models.BaseType", b =>
                {
                    b.Navigation("baseSubTypes");
                });

            modelBuilder.Entity("Garden.Models.GardenSpace", b =>
                {
                    b.Navigation("GardenTaskAttachMaps");

                    b.Navigation("GardenTasks");

                    b.Navigation("GardenUsers");

                    b.Navigation("GardenWorkTimes");
                });

            modelBuilder.Entity("Garden.Models.GardenTask", b =>
                {
                    b.Navigation("GardenUserTaskMaps");
                });

            modelBuilder.Entity("Garden.Models.GardenUser", b =>
                {
                    b.Navigation("GardenManagerTasks");

                    b.Navigation("GardenUserTasks");
                });

            modelBuilder.Entity("Garden.Models.GardenWorkTime", b =>
                {
                    b.Navigation("GardenUserTaskMaps");
                });
#pragma warning restore 612, 618
        }
    }
}
