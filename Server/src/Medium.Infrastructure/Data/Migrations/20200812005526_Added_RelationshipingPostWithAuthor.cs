using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medium.Infrastructure.Data.Migrations
{
    public partial class Added_RelationshipingPostWithAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4c6380bf-e221-427d-9577-cb368a8aec09"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("1778f149-1b44-40f2-b01d-3bc1ff343da5"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("0dae5e32-10bb-4ecc-93e6-ff9e5eeb92f7"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Posts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Avatar", "Bio", "ConfirmedEmail", "CreatedAt", "Deactivated", "Deleted", "Email", "FirstName", "LastName", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"), null, null, true, new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified), false, false, "admin.master@email.com", "Administrador", "Master", "Xfxy0vH0lWjwyDiiYyAvL5bh6eYAZ15fFEfODmV4TX8=", "3vVSUWzIYpk=", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified), "admin.master" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("1d2c7e1a-b024-495c-b128-0e658f1aa12c"), new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified), "Tag 1", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Attachments", "AuthorId", "Content", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new Guid("ba9016a9-5a51-4847-ac01-1107dd087c82"), "", new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"), "Post added in migration", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified), "Post Example", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Authors_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Authors_AuthorId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts");

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("ba9016a9-5a51-4847-ac01-1107dd087c82"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("1d2c7e1a-b024-495c-b128-0e658f1aa12c"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"));

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Posts");

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
    }
}
