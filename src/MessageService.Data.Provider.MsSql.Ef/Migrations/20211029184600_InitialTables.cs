using System;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(MessageServiceDbContext))]
  [Migration("20211029184600_InitialTables")]
  public class InitialTables : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbWorkspace.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          Description = table.Column<string>(nullable: true),
          IsActive = table.Column<bool>(nullable: false),
          ImageContent = table.Column<string>(nullable: true),
          ImageExtension = table.Column<string>(nullable: true),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbWorkspace.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbWorkspaceUser.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          UserId = table.Column<Guid>(nullable: false),
          WorkspaceId = table.Column<Guid>(nullable: false),
          IsAdmin = table.Column<bool>(nullable: false),
          IsActive = table.Column<bool>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbWorkspaceUser.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbChannel.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          WorkspaceId = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          IsPrivate = table.Column<bool>(nullable: false),
          IsActive = table.Column<bool>(nullable: false),
          ImageContent = table.Column<string>(nullable: true),
          ImageExtension = table.Column<string>(nullable: true),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbChannel.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbChannelUser.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          WorkspaceUserId = table.Column<Guid>(nullable: false),
          ChannelId = table.Column<Guid>(nullable: false),
          IsAdmin = table.Column<bool>(nullable: false),
          IsActive = table.Column<bool>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbChannelUser.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbMessage.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          ChannelId = table.Column<Guid>(nullable: false),
          Content = table.Column<string>(nullable: true),
          Status = table.Column<int>(nullable: false),
          ThreadMessagesCount = table.Column<int>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbMessage.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbThreadMessage.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          MessageId = table.Column<Guid>(nullable: false),
          Content = table.Column<string>(nullable: true),
          Status = table.Column<int>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbThreadMessage.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbMessageFile.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          MessageId = table.Column<Guid>(nullable: false),
          FileId = table.Column<Guid>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbMessageFile.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbMessageImage.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          MessageId = table.Column<Guid>(nullable: false),
          ImageId = table.Column<Guid>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbMessageImage.TableName}", x => x.Id);
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        name: nameof(DbWorkspace));

      migrationBuilder.DropTable(
        name: nameof(DbWorkspaceUser));

      migrationBuilder.DropTable(
        name: nameof(DbChannel));

      migrationBuilder.DropTable(
        name: nameof(DbChannelUser));

      migrationBuilder.DropTable(
        name: nameof(DbMessage));

      migrationBuilder.DropTable(
        name: nameof(DbThreadMessage));

      migrationBuilder.DropTable(
        name: nameof(DbMessageFile));

      migrationBuilder.DropTable(
        name: nameof(DbMessageImage));
    }
  }
}
