using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20210701095500_AddSMTP")]
    public class AddSMTP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .CreateTable(
                    name: "SMTPCredentials",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(nullable: false),
                        Host = table.Column<string>(nullable: false),
                        Port = table.Column<int>(nullable: false),
                        EnableSsl = table.Column<bool>(nullable: false),
                        Email = table.Column<string>(nullable: false),
                        Password = table.Column<string>(nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_SMTPCredentials", x => x.Id);
                    }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SMTPCredentials");
        }
    }
}
