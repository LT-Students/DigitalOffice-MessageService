﻿// <auto-generated />
using System;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20200916113747_AddEmailTable")]
    partial class AddEmailTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbEmailReciever", b =>
                {
                    b.Property<Guid>("EmailId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RecieverEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EmailId", "RecieverEmail");

                    b.ToTable("DbEmailReciever");
                });

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SenderUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbMessageFile", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MessageId", "FileId");

                    b.ToTable("DbMessageFile");
                });

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbMessageRecipientUser", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RecipientUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MessageId", "RecipientUserId");

                    b.ToTable("DbMessageRecipientUser");
                });

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbEmailReciever", b =>
                {
                    b.HasOne("LT.DigitalOffice.MessageService.Models.Db.DbEmail", "Email")
                        .WithMany("Receivers")
                        .HasForeignKey("EmailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbMessageFile", b =>
                {
                    b.HasOne("LT.DigitalOffice.MessageService.Models.Db.DbMessage", "Message")
                        .WithMany("FilesIds")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LT.DigitalOffice.MessageService.Models.Db.DbMessageRecipientUser", b =>
                {
                    b.HasOne("LT.DigitalOffice.MessageService.Models.Db.DbMessage", "Message")
                        .WithMany("RecipientUsersIds")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
