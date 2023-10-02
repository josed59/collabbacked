﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using collabDB;

#nullable disable

namespace collabDB.Migrations
{
    [DbContext(typeof(CollabContext))]
    partial class CollabContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("collabDB.Models.TaskChangeLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<bool>("IsCompleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isUatCompleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid>("userid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.HasIndex("userid");

                    b.ToTable("TaskChangeLogs", (string)null);
                });

            modelBuilder.Entity("collabDB.Models.TaskSize", b =>
                {
                    b.Property<int>("TaskSizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskSizeId"));

                    b.Property<string>("TaskDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Weigth")
                        .HasColumnType("int");

                    b.HasKey("TaskSizeId");

                    b.ToTable("TaskSize", (string)null);

                    b.HasData(
                        new
                        {
                            TaskSizeId = 1,
                            TaskDescription = "XS",
                            Weigth = 5
                        },
                        new
                        {
                            TaskSizeId = 2,
                            TaskDescription = "S",
                            Weigth = 12
                        },
                        new
                        {
                            TaskSizeId = 3,
                            TaskDescription = "M",
                            Weigth = 25
                        },
                        new
                        {
                            TaskSizeId = 4,
                            TaskDescription = "L",
                            Weigth = 50
                        },
                        new
                        {
                            TaskSizeId = 5,
                            TaskDescription = "XL",
                            Weigth = 100
                        });
                });

            modelBuilder.Entity("collabDB.Models.TaskState", b =>
                {
                    b.Property<int>("TaskStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskStateId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaskStateId");

                    b.ToTable("TaskState", (string)null);

                    b.HasData(
                        new
                        {
                            TaskStateId = 1,
                            Description = "Pending"
                        },
                        new
                        {
                            TaskStateId = 2,
                            Description = "InProgress"
                        },
                        new
                        {
                            TaskStateId = 3,
                            Description = "Completed"
                        },
                        new
                        {
                            TaskStateId = 4,
                            Description = "Delayed"
                        },
                        new
                        {
                            TaskStateId = 5,
                            Description = "StandBy"
                        },
                        new
                        {
                            TaskStateId = 6,
                            Description = "Deleted"
                        });
                });

            modelBuilder.Entity("collabDB.Models.Teams", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("Teams", (string)null);
                });

            modelBuilder.Entity("collabDB.Models.TeamsMembers", b =>
                {
                    b.Property<int>("TeamsMembersId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamsMembersId"));

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TeamsMembersId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("TeamsMembers", (string)null);
                });

            modelBuilder.Entity("collabDB.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("collabDB.Models.UserType", b =>
                {
                    b.Property<int>("UserTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserTypeId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserTypeId");

                    b.ToTable("Usertype", (string)null);
                });

            modelBuilder.Entity("collabDB.Models.UsersTask", b =>
                {
                    b.Property<Guid>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CloseDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("QaDateFinished")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskSizeId")
                        .HasColumnType("int");

                    b.Property<int>("TaskStateId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("UserTest")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.HasKey("TaskId");

                    b.HasIndex("TaskSizeId");

                    b.HasIndex("TaskStateId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersTask", (string)null);
                });

            modelBuilder.Entity("collabDB.Models.TaskChangeLogs", b =>
                {
                    b.HasOne("collabDB.Models.UsersTask", "UsersTask")
                        .WithMany("TaskChangeLogs")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("collabDB.Models.User", "user")
                        .WithMany("TaskChangeLogs")
                        .HasForeignKey("userid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UsersTask");

                    b.Navigation("user");
                });

            modelBuilder.Entity("collabDB.Models.Teams", b =>
                {
                    b.HasOne("collabDB.Models.User", "User")
                        .WithMany("Teams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("collabDB.Models.TeamsMembers", b =>
                {
                    b.HasOne("collabDB.Models.Teams", "Teams")
                        .WithMany("TeamsMembers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("collabDB.Models.User", "User")
                        .WithMany("TeamsMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teams");

                    b.Navigation("User");
                });

            modelBuilder.Entity("collabDB.Models.User", b =>
                {
                    b.HasOne("collabDB.Models.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("collabDB.Models.UsersTask", b =>
                {
                    b.HasOne("collabDB.Models.TaskSize", "TaskSize")
                        .WithMany("UsersTask")
                        .HasForeignKey("TaskSizeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("collabDB.Models.TaskState", "TaskState")
                        .WithMany("UsersTask")
                        .HasForeignKey("TaskStateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("collabDB.Models.User", "User")
                        .WithMany("UsersTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("TaskSize");

                    b.Navigation("TaskState");

                    b.Navigation("User");
                });

            modelBuilder.Entity("collabDB.Models.TaskSize", b =>
                {
                    b.Navigation("UsersTask");
                });

            modelBuilder.Entity("collabDB.Models.TaskState", b =>
                {
                    b.Navigation("UsersTask");
                });

            modelBuilder.Entity("collabDB.Models.Teams", b =>
                {
                    b.Navigation("TeamsMembers");
                });

            modelBuilder.Entity("collabDB.Models.User", b =>
                {
                    b.Navigation("TaskChangeLogs");

                    b.Navigation("Teams");

                    b.Navigation("TeamsMembers");

                    b.Navigation("UsersTasks");
                });

            modelBuilder.Entity("collabDB.Models.UserType", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("collabDB.Models.UsersTask", b =>
                {
                    b.Navigation("TaskChangeLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
