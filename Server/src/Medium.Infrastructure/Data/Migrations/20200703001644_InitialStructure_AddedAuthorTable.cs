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
                columns: new[] { "Id", "Avatar", "Bio", "ConfirmedEmail", "CreatedAt", "Deactivated", "Deleted", "Email", "FirstName", "Hash", "LastName", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("b3752e54-0207-4cf9-ab39-2b33df08c5c9"), null, null, true, new DateTime(2020, 7, 2, 21, 16, 0, 0, DateTimeKind.Unspecified), false, false, "admin.master@email.com", "Administrador", "af808e", "Master", "Admin123!", new DateTime(2020, 7, 2, 21, 16, 0, 0, DateTimeKind.Unspecified), "admin.master" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
