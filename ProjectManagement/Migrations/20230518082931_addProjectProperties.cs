using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class addProjectProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectScope",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Results",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sponsor",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stakeholders",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TotalBudget",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    GoalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.GoalId);
                    table.ForeignKey(
                        name: "FK_Goals_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    MilestoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.MilestoneId);
                    table.ForeignKey(
                        name: "FK_Milestones_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectId",
                table: "Users",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_ProjectId",
                table: "Goals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_ProjectId",
                table: "Milestones",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Projects_ProjectId",
                table: "Users",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Projects_ProjectId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProjectId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectScope",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Results",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Sponsor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Stakeholders",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TotalBudget",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    ProjectsProjectId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.ProjectsProjectId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Projects_ProjectsProjectId",
                        column: x => x.ProjectsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UsersId",
                table: "ProjectUser",
                column: "UsersId");
        }
    }
}
