using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class prevdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "GroupID",
                table: "Users",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "BaseMessageID",
                table: "Users",
                newName: "BaseMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GroupID",
                table: "Users",
                newName: "IX_Users_GroupId");

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "group1" },
                    { 2L, "group2" },
                    { 3L, "group3" },
                    { 4L, "group4" },
                    { 5L, "group5" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BaseMessageId", "GroupId", "Name", "Pass" },
                values: new object[,]
                {
                    { 1L, null, 1L, "User1", "pass" },
                    { 2L, null, 2L, "User2", "pass" },
                    { 3L, null, 3L, "User3", "pass" },
                    { 4L, null, 4L, "User4", "pass" },
                    { 5L, null, 5L, "User5", "pass" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Users",
                newName: "GroupID");

            migrationBuilder.RenameColumn(
                name: "BaseMessageId",
                table: "Users",
                newName: "BaseMessageID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                newName: "IX_Users_GroupID");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupID",
                table: "Users",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
