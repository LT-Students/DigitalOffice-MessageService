using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [Migration("20210705113900_DeleteSmtpCredentialsTable")]
    public class DeleteSmtpCredentialsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SMTPCredentials");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
