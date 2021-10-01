using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20210824105300_ReworkModels")]
    public class ReworkModels : Migration
    {
        private void UpdateChannelsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                newName: "CreatedBy",
                table: DbChannel.TableName);

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbChannel.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbChannel.TableName,
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "DeactivatedAt",
                newName: "ModifiedAtUtc",
                table: DbChannel.TableName);
        }

        private void UpdateChannelUsersTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: DbChannelUser.TableName,
                nullable: false);

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbChannelUser.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbChannelUser.TableName,
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "DeactivatedAt",
                newName: "ModifiedAtUtc",
                table: DbChannelUser.TableName);
        }

        private void UpdatEmailsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                newName: "CreatedAtUtc",
                table: DbEmail.TableName);
        }

        private void UpdatEmailTemplatesTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorId",
                newName: "CreatedBy",
                table: DbEmailTemplate.TableName);

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbEmailTemplate.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbEmailTemplate.TableName,
                nullable: true);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: DbEmailTemplate.TableName,
                nullable: true);
        }

        private void UpdatUnsentEmailsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSendAt",
                newName: "LastSendAtUtc",
                table: DbUnsentEmail.TableName);

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbUnsentEmail.TableName);
        }

        private void UpdateWorkspacesTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                newName: "CreatedBy",
                table: DbWorkspace.TableName);

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbWorkspace.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbWorkspace.TableName,
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "DeactivatedAt",
                newName: "ModifiedAtUtc",
                table: DbWorkspace.TableName);
        }

        private void UpdateWorkspaceUsersTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: DbWorkspaceUser.TableName,
                nullable: false);

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbWorkspaceUser.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbWorkspaceUser.TableName,
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "DeactivatedAt",
                newName: "ModifiedAtUtc",
                table: DbWorkspaceUser.TableName);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateChannelsTable(migrationBuilder);
            UpdateChannelUsersTable(migrationBuilder);
            UpdatEmailsTable(migrationBuilder);
            UpdatEmailTemplatesTable(migrationBuilder);
            UpdatUnsentEmailsTable(migrationBuilder);
            UpdateWorkspacesTable(migrationBuilder);
            UpdateWorkspaceUsersTable(migrationBuilder);
        }
    }
}
