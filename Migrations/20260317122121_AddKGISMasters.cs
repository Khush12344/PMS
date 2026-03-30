using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PMS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddKGISMasters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tables already created via SQL — just add new Petition columns
            // and create FK indexes EF needs to track relationships

            // ── New Petition columns ──────────────────────────────
            migrationBuilder.AddColumn<bool>(
                name: "ComplainantIsUrban",
                table: "Petitions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComplainantHobliId",
                table: "Petitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComplainantVillageId",
                table: "Petitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComplainantTownId",
                table: "Petitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LocationIsUrban",
                table: "Petitions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationHobliId",
                table: "Petitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationVillageId",
                table: "Petitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationTownId",
                table: "Petitions",
                type: "int",
                nullable: true);

            // OADrop columns — may already exist from previous migration, use SQL check
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('Petitions') AND name='OADropReason')
                    ALTER TABLE Petitions ADD OADropReason nvarchar(max) NULL;
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('Petitions') AND name='OADroppedAt')
                    ALTER TABLE Petitions ADD OADroppedAt datetime2 NULL;
            ");

            // ── Indexes for new Petition FK columns ───────────────
            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantHobliId",
                table: "Petitions",
                column: "ComplainantHobliId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantTownId",
                table: "Petitions",
                column: "ComplainantTownId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantVillageId",
                table: "Petitions",
                column: "ComplainantVillageId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationHobliId",
                table: "Petitions",
                column: "LocationHobliId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationTownId",
                table: "Petitions",
                column: "LocationTownId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationVillageId",
                table: "Petitions",
                column: "LocationVillageId");

            // ── Indexes for Hoblis, Towns, Villages (tables already exist) ──
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Hoblis_TalukId')
                    CREATE INDEX IX_Hoblis_TalukId ON Hoblis(TalukId);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Hoblis_DistrictId')
                    CREATE INDEX IX_Hoblis_DistrictId ON Hoblis(DistrictId);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Towns_DistrictId')
                    CREATE INDEX IX_Towns_DistrictId ON Towns(DistrictId);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Villages_HobliId')
                    CREATE INDEX IX_Villages_HobliId ON Villages(HobliId);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Villages_TalukId')
                    CREATE INDEX IX_Villages_TalukId ON Villages(TalukId);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Villages_DistrictId')
                    CREATE INDEX IX_Villages_DistrictId ON Villages(DistrictId);
            ");

            // ── FK constraints for new Petition columns ───────────
            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Hoblis_ComplainantHobliId",
                table: "Petitions",
                column: "ComplainantHobliId",
                principalTable: "Hoblis",
                principalColumn: "HobliId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Hoblis_LocationHobliId",
                table: "Petitions",
                column: "LocationHobliId",
                principalTable: "Hoblis",
                principalColumn: "HobliId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Towns_ComplainantTownId",
                table: "Petitions",
                column: "ComplainantTownId",
                principalTable: "Towns",
                principalColumn: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Towns_LocationTownId",
                table: "Petitions",
                column: "LocationTownId",
                principalTable: "Towns",
                principalColumn: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Villages_ComplainantVillageId",
                table: "Petitions",
                column: "ComplainantVillageId",
                principalTable: "Villages",
                principalColumn: "VillageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Petitions_Villages_LocationVillageId",
                table: "Petitions",
                column: "LocationVillageId",
                principalTable: "Villages",
                principalColumn: "VillageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Petitions_Hoblis_ComplainantHobliId", table: "Petitions");
            migrationBuilder.DropForeignKey(name: "FK_Petitions_Hoblis_LocationHobliId", table: "Petitions");
            migrationBuilder.DropForeignKey(name: "FK_Petitions_Towns_ComplainantTownId", table: "Petitions");
            migrationBuilder.DropForeignKey(name: "FK_Petitions_Towns_LocationTownId", table: "Petitions");
            migrationBuilder.DropForeignKey(name: "FK_Petitions_Villages_ComplainantVillageId", table: "Petitions");
            migrationBuilder.DropForeignKey(name: "FK_Petitions_Villages_LocationVillageId", table: "Petitions");

            migrationBuilder.DropIndex(name: "IX_Petitions_ComplainantHobliId", table: "Petitions");
            migrationBuilder.DropIndex(name: "IX_Petitions_ComplainantTownId", table: "Petitions");
            migrationBuilder.DropIndex(name: "IX_Petitions_ComplainantVillageId", table: "Petitions");
            migrationBuilder.DropIndex(name: "IX_Petitions_LocationHobliId", table: "Petitions");
            migrationBuilder.DropIndex(name: "IX_Petitions_LocationTownId", table: "Petitions");
            migrationBuilder.DropIndex(name: "IX_Petitions_LocationVillageId", table: "Petitions");

            migrationBuilder.DropColumn(name: "ComplainantIsUrban", table: "Petitions");
            migrationBuilder.DropColumn(name: "ComplainantHobliId", table: "Petitions");
            migrationBuilder.DropColumn(name: "ComplainantVillageId", table: "Petitions");
            migrationBuilder.DropColumn(name: "ComplainantTownId", table: "Petitions");
            migrationBuilder.DropColumn(name: "LocationIsUrban", table: "Petitions");
            migrationBuilder.DropColumn(name: "LocationHobliId", table: "Petitions");
            migrationBuilder.DropColumn(name: "LocationVillageId", table: "Petitions");
            migrationBuilder.DropColumn(name: "LocationTownId", table: "Petitions");
        }
    }
}