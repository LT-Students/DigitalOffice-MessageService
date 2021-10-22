using System;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
  [Migration("20211021204800_AddCreateAtUtcColumnToMessage")]
  [DbContext(typeof(MessageServiceDbContext))]
  public class AddCreateAtUtcColumnToMessage : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      builder.AddColumn<DateTime>(
        name: nameof(DbMessage.CreatedAtUtc),
        table: DbMessage.TableName,
        nullable: true);

      builder.DropColumn(
        name: "Title",
        table: DbMessage.TableName);
    }

    protected override void Down(MigrationBuilder builder)
    {
      builder.DropColumn(
        name: nameof(DbMessage.CreatedAtUtc),
        table: DbMessage.TableName);

      builder.AddColumn<string>(
        name: "Title",
        table: DbMessage.TableName,
        nullable: true);
    }
  }
}
