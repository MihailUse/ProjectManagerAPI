using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdateAssignees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Teams_AssigneeTeamId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssigneeTeamId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssigneeTeamId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "AssigneeTeams",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigneeTeams", x => new { x.TaskId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_AssigneeTeams_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssigneeTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssigneeTeams_TeamId",
                table: "AssigneeTeams",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssigneeTeams");

            migrationBuilder.AddColumn<Guid>(
                name: "AssigneeTeamId",
                table: "Tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeTeamId",
                table: "Tasks",
                column: "AssigneeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Teams_AssigneeTeamId",
                table: "Tasks",
                column: "AssigneeTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
