using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Zima.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    OperationKey = table.Column<string>(maxLength: 128, nullable: false),
                    LatestVersionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Version = table.Column<string>(maxLength: 128, nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    UploadDate = table.Column<long>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packets_Id",
                table: "Packets",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packets_ProjectId",
                table: "Packets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Id",
                table: "Projects",
                column: "Id",
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packets_Projects_ProjectId",
                table: "Packets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Packets");
        }
    }
}
