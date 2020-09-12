using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medium.Infrastructure.Data.Migrations
{
    public partial class Added_RelationshipingTagWithAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Tags",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"),
                columns: new[] { "CreatedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 8, 12, 22, 33, 0, 0, DateTimeKind.Unspecified), "D6PCwii9KSmoyUnUUPBtQsgpla70RLprkdwiqIYwFug=", "L+FUqju1urg=", new DateTime(2020, 8, 12, 22, 33, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "PostTags",
                keyColumns: new[] { "PostId", "TagId" },
                keyValues: new object[] { new Guid("040b026e-df88-4753-b96a-1fbc18498c9d"), new Guid("02ce73a0-768d-4ef3-8347-1fcf9522ee55") },
                column: "Id",
                value: new Guid("fb3c0380-0634-4140-a961-ce5f79c47f81"));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("040b026e-df88-4753-b96a-1fbc18498c9d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 8, 12, 22, 33, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 8, 12, 22, 33, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("02ce73a0-768d-4ef3-8347-1fcf9522ee55"),
                columns: new[] { "AuthorId", "CreatedAt", "UpdatedAt" },
                values: new object[] { new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"), new DateTime(2020, 8, 12, 22, 33, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 8, 12, 22, 33, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_AuthorId",
                table: "Tags",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Authors_AuthorId",
                table: "Tags",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Authors_AuthorId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_AuthorId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Tags");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4f29ee19-ea4b-421d-a796-c2ee446becd2"),
                columns: new[] { "CreatedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified), "usBcD3CMDXbNK+TtsrQsOBMxUQP29R921X1KVJoFlbw=", "2CpxP1GbTYc=", new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "PostTags",
                keyColumns: new[] { "PostId", "TagId" },
                keyValues: new object[] { new Guid("040b026e-df88-4753-b96a-1fbc18498c9d"), new Guid("02ce73a0-768d-4ef3-8347-1fcf9522ee55") },
                column: "Id",
                value: new Guid("496606b5-78b7-4a7a-9fa3-44458bbbd0f8"));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("040b026e-df88-4753-b96a-1fbc18498c9d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("02ce73a0-768d-4ef3-8347-1fcf9522ee55"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 8, 12, 21, 31, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
