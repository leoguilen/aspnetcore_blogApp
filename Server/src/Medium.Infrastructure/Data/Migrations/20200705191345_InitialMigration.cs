using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Medium.Infrastructure.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ConfirmedEmail = table.Column<bool>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Deactivated = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Avatar", "Bio", "ConfirmedEmail", "CreatedAt", "Deactivated", "Deleted", "Email", "FirstName", "LastName", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("7bed486b-4db1-4359-bd1e-75228665cd52"), null, null, true, new DateTime(2020, 7, 5, 16, 13, 0, 0, DateTimeKind.Unspecified), false, false, "admin.master@email.com", "Administrador", "Master", "TsKJN+4eVQo0SMEe010vTTnMiU396k5CFKjxkmcR8g8=", "5PWj+aB2A44=", new DateTime(2020, 7, 5, 16, 13, 0, 0, DateTimeKind.Unspecified), "admin.master" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
