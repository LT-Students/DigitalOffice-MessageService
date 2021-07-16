using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20210104201900_ChangeWorkspaceImage")]
    class ChangeWorkspaceImage : Migration
    {
        private void UpdateWorkspaceTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: DbWorkspace.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ImageId",
                table: DbWorkspace.TableName,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: DbWorkspace.TableName,
                nullable: false);
        }

        private void CreateAdminsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkspaceAdmins",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    WorkspaceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceAdmins_Workspaces",
                        column: x => x.WorkspaceId,
                        principalTable: DbWorkspace.TableName,
                        principalColumn: nameof(DbWorkspace.Id),
                        onDelete: ReferentialAction.Cascade
                    );
                });
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateWorkspaceTable(migrationBuilder);
            CreateAdminsTable(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("WorkspaceAdmins");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: DbWorkspace.TableName,
                nullable: true);

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: DbWorkspace.TableName);

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: DbWorkspace.TableName);
        }
    }
}
