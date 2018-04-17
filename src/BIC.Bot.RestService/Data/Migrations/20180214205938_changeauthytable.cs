using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BIC.Bot.RestService.Data.Migrations
{
    public partial class changeauthytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcionToExecute",
                table: "AuthyAuthorizatios",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeAction",
                table: "AuthyAuthorizatios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcionToExecute",
                table: "AuthyAuthorizatios");

            migrationBuilder.DropColumn(
                name: "TypeAction",
                table: "AuthyAuthorizatios");
        }
    }
}
