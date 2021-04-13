using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20210904201912_ChangeEmailTemplate")]
    class ChangeEmailTemplate : Migration
    {
        private void UpdateEmailTemplate(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: DbEmailTemplate.TableName);

            migrationBuilder.DropColumn(
                name: nameof(DbEmailTemplateText.Subject),
                table: DbEmailTemplate.TableName);

            migrationBuilder.AddColumn<string>(
               name: nameof(DbEmailTemplate.Name),
               table: DbEmailTemplate.TableName,
               nullable: false);

            migrationBuilder.AddColumn<int>(
               name: nameof(DbEmailTemplate.Type),
               table: DbEmailTemplate.TableName,
               nullable: false);
        }

        private void CreateEmailTemplateText(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbEmailTemplateText.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmailTemplateId = table.Column<Guid>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false, maxLength: 2)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{DbEmailTemplateText.TableName}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{DbEmailTemplateText.TableName}_{DbEmailTemplate.TableName}",
                        column: x => x.EmailTemplateId,
                        principalTable: DbEmailTemplate.TableName,
                        principalColumn: nameof(DbEmailTemplate.Id),
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateEmailTemplate(migrationBuilder);
            CreateEmailTemplateText(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(DbEmailTemplateText.TableName);

            migrationBuilder.DropColumn(
                name: nameof(DbEmailTemplate.Name),
                table: DbEmailTemplate.TableName);

            migrationBuilder.DropColumn(
                name: nameof(DbEmailTemplate.Type),
                table: DbEmailTemplate.TableName);

            migrationBuilder.AddColumn<string>(
               name: nameof(DbEmailTemplateText.Subject),
               table: DbEmailTemplate.TableName,
               nullable: false);

            migrationBuilder.AddColumn<string>(
               name: "Body",
               table: DbEmailTemplate.TableName,
               nullable: false);
        }
    }
}
