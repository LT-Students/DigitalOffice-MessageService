using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20210614151600_AddUnsentEmailTable")]
    public class AddUnsentEmailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .CreateTable(
                    name: DbUnsentEmail.TableName,
                    columns: table => new
                    {
                        Id = table.Column<Guid>(nullable: false),
                        EmailId = table.Column<Guid>(nullable: false),
                        CreatedAt = table.Column<DateTime>(nullable: false),
                        LastSendAt = table.Column<DateTime>(nullable: false),
                        TotalSendingCount = table.Column<uint>(nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UnsentEmails", x => x.Id);
                    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .DropTable(name: DbUnsentEmail.TableName);
        }
    }
}
