using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
  [Migration("20210905143900_AddImagePropertiesToWorkspaceAndChannel")]
  [DbContext(typeof(MessageServiceDbContext))]
  class AddImagePropertiesToWorkspaceAndChannel : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      builder.AddColumn<string>(
        name: nameof(DbWorkspace.ImageContent),
        table: DbWorkspace.TableName,
        nullable: true);

      builder.AddColumn<string>(
        name: nameof(DbWorkspace.ImageExtension),
        table: DbWorkspace.TableName,
        nullable: true);

      builder.DropColumn(
        name: "ImageId",
        table: DbWorkspace.TableName);

      builder.AddColumn<string>(
        name: nameof(DbChannel.ImageContent),
        table: DbChannel.TableName,
        nullable: true);

      builder.AddColumn<string>(
        name: nameof(DbChannel.ImageExtension),
        table: DbChannel.TableName,
        nullable: true);

      builder.DropColumn(
        name: "ImageId",
        table: DbChannel.TableName);
    }
  }
}
