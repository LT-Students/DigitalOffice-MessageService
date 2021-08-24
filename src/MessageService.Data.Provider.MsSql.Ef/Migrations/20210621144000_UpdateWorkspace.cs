using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20210621144000_UpdateWorkspace")]
    public class UpdateWorkspace : Migration
    {
        private void CreateChannelsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                    name: DbChannel.TableName,
                    columns: table => new
                    {
                        Id = table.Column<Guid>(nullable: false),
                        WorkspaceId = table.Column<Guid>(nullable: false),
                        OwnerId = table.Column<Guid>(nullable: false),
                        ImageId = table.Column<Guid?>(nullable: true),
                        Name = table.Column<string>(nullable: false),
                        IsActive = table.Column<bool>(nullable: false),
                        IsPrivate = table.Column<bool>(nullable: false),
                        CreatedAt = table.Column<DateTime>(nullable: false),
                        DeactivatedAt = table.Column<DateTime?>(nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Channels", x => x.Id);
                    });
        }

        private void CreateChannelUsersTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                    name: DbChannelUser.TableName,
                    columns: table => new
                    {
                        Id = table.Column<Guid>(nullable: false),
                        WorkspaceUserId = table.Column<Guid>(nullable: false),
                        ChannelId = table.Column<Guid>(nullable: false),
                        IsAdmin = table.Column<bool>(nullable: false),
                        IsActive = table.Column<bool>(nullable: false),
                        CreatedAt = table.Column<DateTime>(nullable: false),
                        DeactivatedAt = table.Column<DateTime?>(nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ChannelUsers", x => x.Id);
                    });
        }

        private void CreateWorkspaceUsersTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                    name: DbWorkspaceUser.TableName,
                    columns: table => new
                    {
                        Id = table.Column<Guid>(nullable: false),
                        UserId = table.Column<Guid>(nullable: false),
                        WorkspaceId = table.Column<Guid>(nullable: false),
                        IsAdmin = table.Column<bool>(nullable: false),
                        IsActive = table.Column<bool>(nullable: false),
                        CreatedAt = table.Column<DateTime>(nullable: false),
                        DeactivatedAt = table.Column<DateTime?>(nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_WorkspaceUsers", x => x.Id);
                    });
        }

        private void EditWorkspacesTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: DbWorkspace.TableName,
                nullable: false);

            migrationBuilder.AddColumn<DateTime?>(
                name: "DeactivatedAt",
                table: DbWorkspace.TableName,
                nullable: true);
        }

        private void CreateWorkspaceAdminsTable(MigrationBuilder migrationBuilder)
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
            migrationBuilder.DropTable("WorkspaceAdmins");

            CreateChannelsTable(migrationBuilder);
            CreateChannelUsersTable(migrationBuilder);
            CreateWorkspaceUsersTable(migrationBuilder);
            EditWorkspacesTable(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(DbWorkspaceUser.TableName);
            migrationBuilder.DropTable(DbChannelUser.TableName);
            migrationBuilder.DropTable(DbChannel.TableName);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: DbWorkspace.TableName);

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: DbWorkspace.TableName);

            CreateWorkspaceAdminsTable(migrationBuilder);
        }
    }
}
