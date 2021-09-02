using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
  [Migration("AddImagePropertiesToWorkspace")]
  [DbContext(typeof(MessageServiceDbContext))]
  class AddImagePropertiesToWorkspace : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      builder.AddColumn<string>(
        name: nameof(DbWorkspace.AvatarContent),
        table: DbWorkspace.TableName,
        nullable: true);

      builder.AddColumn<string>(
        name: nameof(DbWorkspace.AvatarExtension),
        table: DbWorkspace.TableName,
        nullable: true);

      builder.DropColumn(
        name: "ImageId",
        table: DbWorkspace.TableName);

      builder.AddColumn<string>(
        name: nameof(DbWorkspace.AvatarContent),
        table: DbChannel.TableName,
        nullable: true);

      builder.AddColumn<string>(
        name: nameof(DbWorkspace.AvatarExtension),
        table: DbChannel.TableName,
        nullable: true);

      builder.DropColumn(
        name: "ImageId",
        table: DbChannel.TableName);
    }
  }
}
