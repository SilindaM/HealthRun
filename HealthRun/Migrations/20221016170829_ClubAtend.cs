using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthRun.Migrations
{
    public partial class ClubAtend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubAttendees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isHost = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAttendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubAttendees_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubAttendees_AppUserId",
                table: "ClubAttendees",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubAttendees");
        }
    }
}
