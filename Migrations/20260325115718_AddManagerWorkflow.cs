using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Columns already exist in DB — only add indexes and FKs

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_AssignedManagerId",
                table: "Petitions",
                column: "AssignedManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_AssignedOAId",
                table: "Petitions",
                column: "AssignedOAId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_SummaryByOAId",
                table: "Petitions",
                column: "SummaryByOAId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Users_AssignedManagerId",
                table: "Petitions",
                column: "AssignedManagerId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Users_AssignedOAId",
                table: "Petitions",
                column: "AssignedOAId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Users_SummaryByOAId",
                table: "Petitions",
                column: "SummaryByOAId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Petitions_Users_AssignedManagerId",
                table: "Petitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Petitions_Users_AssignedOAId",
                table: "Petitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Petitions_Users_SummaryByOAId",
                table: "Petitions");

            migrationBuilder.DropIndex(
                name: "IX_Petitions_AssignedManagerId",
                table: "Petitions");

            migrationBuilder.DropIndex(
                name: "IX_Petitions_AssignedOAId",
                table: "Petitions");

            migrationBuilder.DropIndex(
                name: "IX_Petitions_SummaryByOAId",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "AssignedManagerId",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "AssignedOAId",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "AssignedToOAAt",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "ManagerRemarks",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "SummaryByOAId",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "SummaryRemarks",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "SummarySubmittedAt",
                table: "Petitions");
        }
    }
}