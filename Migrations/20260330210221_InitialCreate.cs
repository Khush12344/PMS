using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PMS.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComplaintCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameKN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    KGISCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.DistrictId);
                });

            migrationBuilder.CreateTable(
                name: "Taluks",
                columns: table => new
                {
                    TalukId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameKN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasterCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taluks", x => x.TalukId);
                    table.ForeignKey(
                        name: "FK_Taluks_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    TownId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KGISTownId = table.Column<int>(type: "int", nullable: false),
                    KGISCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TownType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.TownId);
                    table.ForeignKey(
                        name: "FK_Towns_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hoblis",
                columns: table => new
                {
                    HobliId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameKN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TalukId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hoblis", x => x.HobliId);
                    table.ForeignKey(
                        name: "FK_Hoblis_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId");
                    table.ForeignKey(
                        name: "FK_Hoblis_Taluks_TalukId",
                        column: x => x.TalukId,
                        principalTable: "Taluks",
                        principalColumn: "TalukId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OfficeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HouseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    TalukId = table.Column<int>(type: "int", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId");
                    table.ForeignKey(
                        name: "FK_Users_Taluks_TalukId",
                        column: x => x.TalukId,
                        principalTable: "Taluks",
                        principalColumn: "TalukId");
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    VillageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameKN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HobliId = table.Column<int>(type: "int", nullable: false),
                    TalukId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.VillageId);
                    table.ForeignKey(
                        name: "FK_Villages_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId");
                    table.ForeignKey(
                        name: "FK_Villages_Hoblis_HobliId",
                        column: x => x.HobliId,
                        principalTable: "Hoblis",
                        principalColumn: "HobliId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Villages_Taluks_TalukId",
                        column: x => x.TalukId,
                        principalTable: "Taluks",
                        principalColumn: "TalukId");
                });

            migrationBuilder.CreateTable(
                name: "Petitions",
                columns: table => new
                {
                    PetitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    PetitionApplicationId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ComplainantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplainantDistrictId = table.Column<int>(type: "int", nullable: true),
                    ComplainantIsUrban = table.Column<bool>(type: "bit", nullable: true),
                    ComplainantTalukId = table.Column<int>(type: "int", nullable: true),
                    ComplainantHobliId = table.Column<int>(type: "int", nullable: true),
                    ComplainantVillageId = table.Column<int>(type: "int", nullable: true),
                    ComplainantTownId = table.Column<int>(type: "int", nullable: true),
                    ComplainantVillage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplainantPincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplaintCategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationDistrictId = table.Column<int>(type: "int", nullable: false),
                    LocationIsUrban = table.Column<bool>(type: "bit", nullable: true),
                    LocationTalukId = table.Column<int>(type: "int", nullable: true),
                    LocationHobliId = table.Column<int>(type: "int", nullable: true),
                    LocationVillageId = table.Column<int>(type: "int", nullable: true),
                    LocationTownId = table.Column<int>(type: "int", nullable: true),
                    LocationVillage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationPincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsOfflineEntry = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedManagerId = table.Column<int>(type: "int", nullable: true),
                    AssignedOAId = table.Column<int>(type: "int", nullable: true),
                    AssignedToOAAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ManagerRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScrutinizedByUserId = table.Column<int>(type: "int", nullable: true),
                    ScrutinizedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OARecommendation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecommendedOfficeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecommendedOfficerId = table.Column<int>(type: "int", nullable: true),
                    OARemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OADropReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OADroppedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedOfficerId = table.Column<int>(type: "int", nullable: true),
                    AssignedOfficeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    APCCFRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APCCFDocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SummaryByOAId = table.Column<int>(type: "int", nullable: true),
                    SummarySubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SummaryRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId1 = table.Column<int>(type: "int", nullable: true),
                    UserId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Petitions", x => x.PetitionId);
                    table.ForeignKey(
                        name: "FK_Petitions_ComplaintCategories_ComplaintCategoryId",
                        column: x => x.ComplaintCategoryId,
                        principalTable: "ComplaintCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Petitions_Districts_ComplainantDistrictId",
                        column: x => x.ComplainantDistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId");
                    table.ForeignKey(
                        name: "FK_Petitions_Districts_LocationDistrictId",
                        column: x => x.LocationDistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Petitions_Hoblis_ComplainantHobliId",
                        column: x => x.ComplainantHobliId,
                        principalTable: "Hoblis",
                        principalColumn: "HobliId");
                    table.ForeignKey(
                        name: "FK_Petitions_Hoblis_LocationHobliId",
                        column: x => x.LocationHobliId,
                        principalTable: "Hoblis",
                        principalColumn: "HobliId");
                    table.ForeignKey(
                        name: "FK_Petitions_Taluks_ComplainantTalukId",
                        column: x => x.ComplainantTalukId,
                        principalTable: "Taluks",
                        principalColumn: "TalukId");
                    table.ForeignKey(
                        name: "FK_Petitions_Taluks_LocationTalukId",
                        column: x => x.LocationTalukId,
                        principalTable: "Taluks",
                        principalColumn: "TalukId");
                    table.ForeignKey(
                        name: "FK_Petitions_Towns_ComplainantTownId",
                        column: x => x.ComplainantTownId,
                        principalTable: "Towns",
                        principalColumn: "TownId");
                    table.ForeignKey(
                        name: "FK_Petitions_Towns_LocationTownId",
                        column: x => x.LocationTownId,
                        principalTable: "Towns",
                        principalColumn: "TownId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_AssignedManagerId",
                        column: x => x.AssignedManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_AssignedOAId",
                        column: x => x.AssignedOAId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_AssignedOfficerId",
                        column: x => x.AssignedOfficerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_RecommendedOfficerId",
                        column: x => x.RecommendedOfficerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_ScrutinizedByUserId",
                        column: x => x.ScrutinizedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_SummaryByOAId",
                        column: x => x.SummaryByOAId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Users_UserId2",
                        column: x => x.UserId2,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Petitions_Villages_ComplainantVillageId",
                        column: x => x.ComplainantVillageId,
                        principalTable: "Villages",
                        principalColumn: "VillageId");
                    table.ForeignKey(
                        name: "FK_Petitions_Villages_LocationVillageId",
                        column: x => x.LocationVillageId,
                        principalTable: "Villages",
                        principalColumn: "VillageId");
                });

            migrationBuilder.CreateTable(
                name: "ActionReports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetitionId = table.Column<int>(type: "int", nullable: false),
                    OfficerId = table.Column<int>(type: "int", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReportText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    APCCFDecisionRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionReports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ActionReports_Petitions_PetitionId",
                        column: x => x.PetitionId,
                        principalTable: "Petitions",
                        principalColumn: "PetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionReports_Users_OfficerId",
                        column: x => x.OfficerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PetitionAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetitionId = table.Column<int>(type: "int", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitionAttachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_PetitionAttachments_Petitions_PetitionId",
                        column: x => x.PetitionId,
                        principalTable: "Petitions",
                        principalColumn: "PetitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetitionWorkflowLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetitionId = table.Column<int>(type: "int", nullable: false),
                    ActorUserId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublicLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InternalRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ToStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitionWorkflowLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_PetitionWorkflowLogs_Petitions_PetitionId",
                        column: x => x.PetitionId,
                        principalTable: "Petitions",
                        principalColumn: "PetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetitionWorkflowLogs_Users_ActorUserId",
                        column: x => x.ActorUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ActionReportAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionReportAttachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_ActionReportAttachments_ActionReports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "ActionReports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ComplaintCategories",
                columns: new[] { "CategoryId", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, null, true, "Illegal Felling of Trees" },
                    { 2, null, true, "Encroachment of Forest Land" },
                    { 3, null, true, "Poaching / Wildlife Crime" },
                    { 4, null, true, "Corruption / Bribery" },
                    { 5, null, true, "Misappropriation of Funds" },
                    { 6, null, true, "Illegal Mining / Quarrying" },
                    { 7, null, true, "Forest Fire (Negligence)" },
                    { 8, null, true, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionReportAttachments_ReportId",
                table: "ActionReportAttachments",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionReports_OfficerId",
                table: "ActionReports",
                column: "OfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionReports_PetitionId",
                table: "ActionReports",
                column: "PetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Hoblis_DistrictId",
                table: "Hoblis",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Hoblis_TalukId",
                table: "Hoblis",
                column: "TalukId");

            migrationBuilder.CreateIndex(
                name: "IX_PetitionAttachments_PetitionId",
                table: "PetitionAttachments",
                column: "PetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_AssignedManagerId",
                table: "Petitions",
                column: "AssignedManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_AssignedOAId",
                table: "Petitions",
                column: "AssignedOAId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_AssignedOfficerId",
                table: "Petitions",
                column: "AssignedOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantDistrictId",
                table: "Petitions",
                column: "ComplainantDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantHobliId",
                table: "Petitions",
                column: "ComplainantHobliId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantTalukId",
                table: "Petitions",
                column: "ComplainantTalukId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantTownId",
                table: "Petitions",
                column: "ComplainantTownId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplainantVillageId",
                table: "Petitions",
                column: "ComplainantVillageId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplaintCategoryId",
                table: "Petitions",
                column: "ComplaintCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationDistrictId",
                table: "Petitions",
                column: "LocationDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationHobliId",
                table: "Petitions",
                column: "LocationHobliId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationTalukId",
                table: "Petitions",
                column: "LocationTalukId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationTownId",
                table: "Petitions",
                column: "LocationTownId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_LocationVillageId",
                table: "Petitions",
                column: "LocationVillageId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_PetitionApplicationId",
                table: "Petitions",
                column: "PetitionApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_RecommendedOfficerId",
                table: "Petitions",
                column: "RecommendedOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ScrutinizedByUserId",
                table: "Petitions",
                column: "ScrutinizedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_SummaryByOAId",
                table: "Petitions",
                column: "SummaryByOAId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_UserId",
                table: "Petitions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_UserId1",
                table: "Petitions",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_UserId2",
                table: "Petitions",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_PetitionWorkflowLogs_ActorUserId",
                table: "PetitionWorkflowLogs",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PetitionWorkflowLogs_PetitionId",
                table: "PetitionWorkflowLogs",
                column: "PetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Taluks_DistrictId",
                table: "Taluks",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Towns_DistrictId",
                table: "Towns",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DistrictId",
                table: "Users",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MobileNumber",
                table: "Users",
                column: "MobileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TalukId",
                table: "Users",
                column: "TalukId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_DistrictId",
                table: "Villages",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_HobliId",
                table: "Villages",
                column: "HobliId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_TalukId",
                table: "Villages",
                column: "TalukId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionReportAttachments");

            migrationBuilder.DropTable(
                name: "PetitionAttachments");

            migrationBuilder.DropTable(
                name: "PetitionWorkflowLogs");

            migrationBuilder.DropTable(
                name: "ActionReports");

            migrationBuilder.DropTable(
                name: "Petitions");

            migrationBuilder.DropTable(
                name: "ComplaintCategories");

            migrationBuilder.DropTable(
                name: "Towns");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Villages");

            migrationBuilder.DropTable(
                name: "Hoblis");

            migrationBuilder.DropTable(
                name: "Taluks");

            migrationBuilder.DropTable(
                name: "Districts");
        }
    }
}
