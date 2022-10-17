using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthRun.Migrations
{
    public partial class UserFollowing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaceAttendees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isHost = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceAttendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaceAttendees_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFollowing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ObserverId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollowing_AspNetUsers_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceAttendees_AppUserId",
                table: "RaceAttendees",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowing_ObserverId",
                table: "UserFollowing",
                column: "ObserverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceAttendees");

            migrationBuilder.DropTable(
                name: "UserFollowing");
        }
    }
}
