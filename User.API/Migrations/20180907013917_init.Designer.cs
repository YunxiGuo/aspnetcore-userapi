﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using System;
using User.API.Data;

namespace User.API.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20180907013917_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("User.API.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Avater");

                    b.Property<string>("City");

                    b.Property<int>("CityId");

                    b.Property<string>("Company");

                    b.Property<string>("Email");

                    b.Property<byte>("Gender");

                    b.Property<string>("Name");

                    b.Property<string>("NameCard");

                    b.Property<string>("Phone");

                    b.Property<string>("Province");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("User.API.Models.BPFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("FileName");

                    b.Property<string>("FormatFilePath");

                    b.Property<string>("OriginFilePath");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("BPFiles");
                });

            modelBuilder.Entity("User.API.Models.UserProperty", b =>
                {
                    b.Property<int>("AppUserId");

                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<string>("Value")
                        .HasMaxLength(100);

                    b.Property<string>("Text");

                    b.HasKey("AppUserId", "Key", "Value");

                    b.ToTable("UserPropertities");
                });

            modelBuilder.Entity("User.API.Models.UserTag", b =>
                {
                    b.Property<int>("AppUserId");

                    b.Property<string>("Tag")
                        .HasMaxLength(100);

                    b.HasKey("AppUserId", "Tag");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("User.API.Models.UserProperty", b =>
                {
                    b.HasOne("User.API.Models.AppUser")
                        .WithMany("UserProperties")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
