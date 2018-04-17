using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BIC.Bot.RestService.Data.Migrations
{
    public partial class authy3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationValidityDate",
                table: "AuthyAuthorizatios",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "UsedAuthorization",
                table: "AuthyAuthorizatios",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationValidityDate",
                table: "AuthyAuthorizatios");

            migrationBuilder.DropColumn(
                name: "UsedAuthorization",
                table: "AuthyAuthorizatios");
        }
    }
}
