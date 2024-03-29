﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using identityservice.Infrastructure.Persistence;

#nullable disable

namespace identityservice.Migrations
{
    [DbContext(typeof(IdentityContext))]
    partial class IdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("identityservice.Domain.UserAggregate.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("IssuedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken", (string)null);
                });

            modelBuilder.Entity("identityservice.Domain.UserAggregate.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.ToTable("Role", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("e8e858f6-0df3-464a-ac2a-8d933ba669dd"),
                            CreatedBy = "Unknown",
                            CreatedDate = new DateTimeOffset(new DateTime(2022, 12, 23, 2, 21, 55, 343, DateTimeKind.Unspecified).AddTicks(7953), new TimeSpan(0, 0, 0, 0, 0)),
                            IsDefault = true,
                            Name = "Client",
                            Status = 0
                        },
                        new
                        {
                            Id = new Guid("b4547ab5-621e-42a2-b8ed-29b0f11c5891"),
                            CreatedBy = "Unknown",
                            CreatedDate = new DateTimeOffset(new DateTime(2022, 12, 23, 2, 21, 55, 343, DateTimeKind.Unspecified).AddTicks(7971), new TimeSpan(0, 0, 0, 0, 0)),
                            IsDefault = false,
                            Name = "Administrator",
                            Status = 0
                        });
                });

            modelBuilder.Entity("identityservice.Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("identityservice.Domain.UserAggregate.RefreshToken", b =>
                {
                    b.HasOne("identityservice.Domain.UserAggregate.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("identityservice.Domain.UserAggregate.User", b =>
                {
                    b.HasOne("identityservice.Domain.UserAggregate.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("identityservice.Domain.UserAggregate.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("EmailAddress")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("identityservice.Domain.UserAggregate.ValueObjects.FullName", "FullName", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("Surname")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("identityservice.Domain.UserAggregate.ValueObjects.Parole", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Password")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("identityservice.Domain.UserAggregate.ValueObjects.UserActivation", "Activation", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("ActivationCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTimeOffset?>("ActivationDate")
                                .HasColumnType("datetimeoffset");

                            b1.Property<byte>("IsActivated")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("tinyint")
                                .HasDefaultValue((byte)0);

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Activation")
                        .IsRequired();

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("FullName")
                        .IsRequired();

                    b.Navigation("Password")
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("identityservice.Domain.UserAggregate.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("identityservice.Domain.UserAggregate.User", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
