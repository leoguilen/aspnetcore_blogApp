using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Medium.Infrastructure.Data.Migrations
{
    public partial class Added_Tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("66d0ed3b-4ed6-4367-90e7-8d375d35c110"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("5a45eb0e-365a-4058-b1dc-8385dc5e26ae"));

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Avatar", "Bio", "ConfirmedEmail", "CreatedAt", "Deactivated", "Deleted", "Email", "FirstName", "LastName", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("4c6380bf-e221-427d-9577-cb368a8aec09"), null, null, true, new DateTime(2020, 7, 25, 14, 47, 0, 0, DateTimeKind.Unspecified), false, false, "admin.master@email.com", "Administrador", "Master", "Jp+pLpdsmvhDHoOPcJ4uLq3trdWzkbubselxno+DlOI=", "TjB5Zbhe0p8=", new DateTime(2020, 7, 25, 14, 47, 0, 0, DateTimeKind.Unspecified), "admin.master" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Attachments", "Content", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new Guid("1778f149-1b44-40f2-b01d-3bc1ff343da5"), "", "Post added in migration", new DateTime(2020, 7, 25, 14, 47, 0, 0, DateTimeKind.Unspecified), "Post Example", new DateTime(2020, 7, 25, 14, 47, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("0dae5e32-10bb-4ecc-93e6-ff9e5eeb92f7"), new DateTime(2020, 7, 25, 14, 47, 0, 0, DateTimeKind.Unspecified), "Tag 1", new DateTime(2020, 7, 25, 14, 47, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4c6380bf-e221-427d-9577-cb368a8aec09"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("1778f149-1b44-40f2-b01d-3bc1ff343da5"));

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Avatar", "Bio", "ConfirmedEmail", "CreatedAt", "Deactivated", "Deleted", "Email", "FirstName", "LastName", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("66d0ed3b-4ed6-4367-90e7-8d375d35c110"), null, null, true, new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified), false, false, "admin.master@email.com", "Administrador", "Master", "tGmz3XcMajUcUJdYvx3QqhSTV8iEuJYzJ6BndoG5jM8=", "Gbi/UNO95Ps=", new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified), "admin.master" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Attachments", "Content", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new Guid("5a45eb0e-365a-4058-b1dc-8385dc5e26ae"), "", "Post added in migration", new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified), "Post Example", new DateTime(2020, 7, 12, 22, 11, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
