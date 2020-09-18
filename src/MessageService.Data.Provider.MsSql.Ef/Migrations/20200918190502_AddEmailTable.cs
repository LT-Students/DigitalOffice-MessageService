using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    public partial class AddEmailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: true),
                    Receiver = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");
        }
    }
}
