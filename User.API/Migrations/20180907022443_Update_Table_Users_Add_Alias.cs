using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace User.API.Migrations
{
    public partial class Update_Table_Users_Add_Alias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Users");
        }
    }
}
