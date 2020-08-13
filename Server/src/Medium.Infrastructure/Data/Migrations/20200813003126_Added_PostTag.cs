using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medium.Infrastructure.Data.Migrations
{
    public partial class Added_PostTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("ba9016a9-5a51-4847-ac01-1107dd087c82"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("1d2c7e1a-b024-495c-b128-0e658f1aa12c"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"),
                columns: new[] { "CreatedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified), "usBcD3CMDXbNK+TtsrQsOBMxUQP29R921X1KVJoFlbw=", "2CpxP1GbTYc=", new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Attachments", "AuthorId", "Content", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new Guid("040b026e-df88-4753-b96a-1fbc18498c9d"), "", new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"), "Post added in migration", new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified), "Post Example", new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("02ce73a0-768d-4ef3-8347-1fcf9522ee55"), new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified), "Tag 1", new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "PostTags",
                columns: new[] { "PostId", "TagId", "Id" },
                values: new object[] { new Guid("040b026e-df88-4753-b96a-1fbc18498c9d"), new Guid("02ce73a0-768d-4ef3-8347-1fcf9522ee55"), new Guid("496606b5-78b7-4a7a-9fa3-44458bbbd0f8") });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                table: "Tags");

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("040b026e-df88-4753-b96a-1fbc18498c9d"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("02ce73a0-768d-4ef3-8347-1fcf9522ee55"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"),
                columns: new[] { "CreatedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified), "Xfxy0vH0lWjwyDiiYyAvL5bh6eYAZ15fFEfODmV4TX8=", "3vVSUWzIYpk=", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Attachments", "AuthorId", "Content", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new Guid("ba9016a9-5a51-4847-ac01-1107dd087c82"), "", new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"), "Post added in migration", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified), "Post Example", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("1d2c7e1a-b024-495c-b128-0e658f1aa12c"), new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified), "Tag 1", new DateTime(2020, 8, 11, 21, 55, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
