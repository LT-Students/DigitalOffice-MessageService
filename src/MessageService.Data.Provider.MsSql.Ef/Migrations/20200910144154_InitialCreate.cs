using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SenderUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbMessageFile",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbMessageFile", x => new { x.MessageId, x.FileId });
                    table.ForeignKey(
                        name: "FK_DbMessageFile_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbMessageRecipientUser",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    RecipientUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbMessageRecipientUser", x => new { x.MessageId, x.RecipientUserId });
                    table.ForeignKey(
                        name: "FK_DbMessageRecipientUser_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbMessageFile");

            migrationBuilder.DropTable(
                name: "DbMessageRecipientUser");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
