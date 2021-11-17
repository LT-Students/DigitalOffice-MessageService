using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(MessageServiceDbContext))]
  [Migration("20211117082700_RenameColumnOfChannelUser")]
  public class RenameColumnOfChannelUser : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
        name: "WorkspaceUserId",
        table: DbChannelUser.TableName,
        newName: "UserId");
    }
  }
}
