using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ChatServer.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Pass = table.Column<string>(type: "text", nullable: true),
                    BaseMessageId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseMessages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseMessages_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "Id", "BaseMessageId", "Name", "Pass" },
                values: new object[,]
                {
                    { 1L, null, "User1", "pass" },
                    { 2L, null, "User2", "pass" },
                    { 3L, null, "User3", "pass" },
                    { 4L, null, "User4", "pass" },
                    { 5L, null, "User5", "pass" }
                });

            migrationBuilder.InsertData(
                table: "GroupUser",
                columns: new[] { "GroupsId", "UsersId" },
                values: new object[,]
                {
                    { 1L, 1L },
                    { 2L, 2L },
                    { 3L, 3L },
                    { 4L, 4L },
                    { 5L, 5L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_GroupId",
                table: "BaseMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_UserId1",
                table: "BaseMessages",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UsersId",
                table: "GroupUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseMessages");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
