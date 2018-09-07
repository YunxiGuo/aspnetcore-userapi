using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace User.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FormatFilePath = table.Column<string>(nullable: true),
                    OriginFilePath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Address = table.Column<string>(nullable: true),
                    Avater = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    Company = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Gender = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameCard = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTags",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    Tag = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTags", x => new { x.AppUserId, x.Tag });
                });

            migrationBuilder.CreateTable(
                name: "UserPropertities",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPropertities", x => new { x.AppUserId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_UserPropertities_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPFiles");

            migrationBuilder.DropTable(
                name: "UserPropertities");

            migrationBuilder.DropTable(
                name: "UserTags");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
