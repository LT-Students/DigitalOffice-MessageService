using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(MessageServiceDbContext))]
    [Migration("20210729102000_AddParsePropertiesaTable")]
    public class AddParsePropertiesaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbKeyword.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Keyword = table.Column<string>(nullable: false, maxLength: 50),
                    ServiceName = table.Column<int>(nullable: false),
                    EntityName = table.Column<string>(nullable: false),
                    PropertyName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", p => p.Id);
                    table.UniqueConstraint("UC_Keyword", p => p.Keyword);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(DbKeyword.TableName);
        }
    }
}
