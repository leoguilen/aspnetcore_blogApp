using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medium.Infrastructure.Data.Migrations
{
    public partial class InitialStructure_AddedAuthorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ConfirmedEmail = table.Column<bool>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Deactivated = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
