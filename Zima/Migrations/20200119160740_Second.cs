using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Zima.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Packets_LatestVersionId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LatestVersionId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LatestVersionId",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LatestVersionId",
                table: "Projects",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LatestVersionId",
                table: "Projects",
                column: "LatestVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Packets_LatestVersionId",
                table: "Projects",
                column: "LatestVersionId",
                principalTable: "Packets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
