using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medium.Infrastructure.Data.Migrations
{
    public partial class AddedPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("7bed486b-4db1-4359-bd1e-75228665cd52"));

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Attachments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Avatar", "Bio", "ConfirmedEmail", "CreatedAt", "Deactivated", "Deleted", "Email", "FirstName", "LastName", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("66d0ed3b-4ed6-4367-90e7-8d375d35c110"), null, null, true, new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified), false, false, "admin.master@email.com", "Administrador", "Master", "tGmz3XcMajUcUJdYvx3QqhSTV8iEuJYzJ6BndoG5jM8=", "Gbi/UNO95Ps=", new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified), "admin.master" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Attachments", "Content", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new Guid("5a45eb0e-365a-4058-b1dc-8385dc5e26ae"), "", "Post added in migration", new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified), "Post Example", new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("66d0ed3b-4ed6-4367-90e7-8d375d35c110"));

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Avatar", "Bio", "ConfirmedEmail", "CreatedAt", "Deactivated", "Deleted", "Email", "FirstName", "LastName", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("7bed486b-4db1-4359-bd1e-75228665cd52"), null, null, true, new DateTime(2020, 7, 5, 16, 13, 0, 0, DateTimeKind.Unspecified), false, false, "admin.master@email.com", "Administrador", "Master", "TsKJN+4eVQo0SMEe010vTTnMiU396k5CFKjxkmcR8g8=", "5PWj+aB2A44=", new DateTime(2020, 7, 5, 16, 13, 0, 0, DateTimeKind.Unspecified), "admin.master" });
        }
    }
}
