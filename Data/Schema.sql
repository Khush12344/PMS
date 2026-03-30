-- ============================================================
-- PMS - Petition Management System
-- Karnataka Forest Department - Vigilance Wing
-- SQL Server Schema Script
-- ============================================================

-- ── Master Tables ────────────────────────────────────────────

CREATE TABLE Districts (
    DistrictId   INT IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(100) NOT NULL,
    IsActive     BIT NOT NULL DEFAULT 1
);

CREATE TABLE Taluks (
    TalukId      INT IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(100) NOT NULL,
    DistrictId   INT NOT NULL,
    IsActive     BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Taluks_Districts FOREIGN KEY (DistrictId)
        REFERENCES Districts(DistrictId) ON DELETE NO ACTION
);

CREATE TABLE ComplaintCategories (
    CategoryId   INT IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(200) NOT NULL,
    Description  NVARCHAR(MAX) NULL,
    IsActive     BIT NOT NULL DEFAULT 1
);

-- ── Users ─────────────────────────────────────────────────────

CREATE TABLE Users (
    UserId        INT IDENTITY(1,1) PRIMARY KEY,
    MobileNumber  VARCHAR(15) NOT NULL,
    Name          NVARCHAR(200) NULL,
    Email         NVARCHAR(200) NULL,
    -- Role: Public | OfficeAssistant | APCCF | CircleOfficer | DivisionOfficer | FMSOfficer
    Role          VARCHAR(50) NOT NULL,
    -- OfficeType: Circle | Division | FMSDivision (for field officers only)
    OfficeType    VARCHAR(50) NULL,
    OtpHash       VARCHAR(256) NULL,
    OtpExpiry     DATETIME NULL,
    IsActive      BIT NOT NULL DEFAULT 1,
    CreatedAt     DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT UQ_Users_Mobile UNIQUE (MobileNumber)
);

-- ── Petitions ─────────────────────────────────────────────────

CREATE TABLE Petitions (
    PetitionId              INT IDENTITY(1,1) PRIMARY KEY,
    PetitionApplicationId   VARCHAR(30) NOT NULL,   -- e.g. PMS-2024-00001

    -- Complainant Address (optional - identity hidden)
    ComplainantName         NVARCHAR(200) NULL,
    HouseNo                 NVARCHAR(100) NULL,
    Street                  NVARCHAR(200) NULL,
    Area                    NVARCHAR(200) NULL,
    ComplainantDistrictId   INT NULL,
    ComplainantTalukId      INT NULL,
    ComplainantVillage      NVARCHAR(200) NULL,
    ComplainantPincode      VARCHAR(10) NULL,

    -- Complaint Details
    ComplaintCategoryId     INT NOT NULL,
    Description             NVARCHAR(MAX) NOT NULL,

    -- Incident Location
    LocationDistrictId      INT NOT NULL,
    LocationTalukId         INT NULL,
    LocationVillage         NVARCHAR(200) NULL,
    LocationPincode         VARCHAR(10) NULL,

    -- Contact
    MobileNumber            VARCHAR(15) NOT NULL,

    -- Status: Submitted | UnderScrutiny | RecommendedForAction | RecommendedForDrop
    --         | Assigned | Dropped | InProgress | SubmittedForClosure | Closed | Reopened
    Status                  VARCHAR(50) NOT NULL DEFAULT 'Submitted',
    IsOfflineEntry          BIT NOT NULL DEFAULT 0,
    SubmittedAt             DATETIME NOT NULL DEFAULT GETUTCDATE(),

    -- Office Assistant Scrutiny
    ScrutinizedByUserId     INT NULL,
    ScrutinizedAt           DATETIME NULL,
    -- OARecommendation: ForAction | ForDrop
    OARecommendation        VARCHAR(50) NULL,
    RecommendedOfficeType   VARCHAR(50) NULL,
    RecommendedOfficerId    INT NULL,
    OARemarks               NVARCHAR(MAX) NULL,

    -- APCCF Assignment
    AssignedOfficerId       INT NULL,
    AssignedOfficeType      VARCHAR(50) NULL,
    AssignedAt              DATETIME NULL,
    APCCFRemarks            NVARCHAR(MAX) NULL,

    CONSTRAINT UQ_Petition_AppId UNIQUE (PetitionApplicationId),
    CONSTRAINT FK_Pet_ComplainantDistrict FOREIGN KEY (ComplainantDistrictId)
        REFERENCES Districts(DistrictId) ON DELETE NO ACTION,
    CONSTRAINT FK_Pet_ComplainantTaluk FOREIGN KEY (ComplainantTalukId)
        REFERENCES Taluks(TalukId) ON DELETE NO ACTION,
    CONSTRAINT FK_Pet_Category FOREIGN KEY (ComplaintCategoryId)
        REFERENCES ComplaintCategories(CategoryId) ON DELETE NO ACTION,
    CONSTRAINT FK_Pet_LocationDistrict FOREIGN KEY (LocationDistrictId)
        REFERENCES Districts(DistrictId) ON DELETE NO ACTION,
    CONSTRAINT FK_Pet_LocationTaluk FOREIGN KEY (LocationTalukId)
        REFERENCES Taluks(TalukId) ON DELETE NO ACTION,
    CONSTRAINT FK_Pet_AssignedOfficer FOREIGN KEY (AssignedOfficerId)
        REFERENCES Users(UserId) ON DELETE NO ACTION,
    CONSTRAINT FK_Pet_ScrutinizedBy FOREIGN KEY (ScrutinizedByUserId)
        REFERENCES Users(UserId) ON DELETE NO ACTION,
    CONSTRAINT FK_Pet_RecommendedOfficer FOREIGN KEY (RecommendedOfficerId)
        REFERENCES Users(UserId) ON DELETE NO ACTION
);

-- ── Petition Attachments ──────────────────────────────────────
-- PDF documents and Photos only (no audio/video)

CREATE TABLE PetitionAttachments (
    AttachmentId        INT IDENTITY(1,1) PRIMARY KEY,
    PetitionId          INT NOT NULL,
    -- FileType: Document | Photo
    FileType            VARCHAR(20) NOT NULL,
    StoredFileName      NVARCHAR(500) NOT NULL,    -- GUID-based server name
    OriginalFileName    NVARCHAR(500) NOT NULL,    -- Original upload filename
    FileSizeBytes       BIGINT NOT NULL,
    UploadedAt          DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_PetAtt_Petition FOREIGN KEY (PetitionId)
        REFERENCES Petitions(PetitionId) ON DELETE CASCADE
);

-- ── Petition Workflow Log ─────────────────────────────────────
-- Every stage transition recorded here.
-- PublicLabel is what the citizen sees on the tracking page.
-- InternalRemarks is internal only - NEVER exposed to public.

CREATE TABLE PetitionWorkflowLogs (
    LogId               INT IDENTITY(1,1) PRIMARY KEY,
    PetitionId          INT NOT NULL,
    ActorUserId         INT NULL,                  -- NULL for system entries
    Action              VARCHAR(100) NOT NULL,
    PublicLabel         NVARCHAR(200) NULL,        -- Shown on public tracking
    InternalRemarks     NVARCHAR(MAX) NULL,        -- Internal only
    FromStatus          VARCHAR(50) NOT NULL,
    ToStatus            VARCHAR(50) NOT NULL,
    ActionAt            DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_WfLog_Petition FOREIGN KEY (PetitionId)
        REFERENCES Petitions(PetitionId) ON DELETE CASCADE,
    CONSTRAINT FK_WfLog_Actor FOREIGN KEY (ActorUserId)
        REFERENCES Users(UserId) ON DELETE NO ACTION
);

-- ── Action Reports (Field Officer) ────────────────────────────

CREATE TABLE ActionReports (
    ReportId                INT IDENTITY(1,1) PRIMARY KEY,
    PetitionId              INT NOT NULL,
    OfficerId               INT NOT NULL,
    -- ReportType: Interim | Final
    ReportType              VARCHAR(20) NOT NULL,
    ReportText              NVARCHAR(MAX) NOT NULL,
    SubmittedAt             DATETIME NOT NULL DEFAULT GETUTCDATE(),
    -- IsApproved: NULL=pending, 1=accepted, 0=rejected
    IsApproved              BIT NULL,
    APCCFDecisionRemarks    NVARCHAR(MAX) NULL,
    DecisionAt              DATETIME NULL,
    CONSTRAINT FK_AR_Petition FOREIGN KEY (PetitionId)
        REFERENCES Petitions(PetitionId) ON DELETE CASCADE,
    CONSTRAINT FK_AR_Officer FOREIGN KEY (OfficerId)
        REFERENCES Users(UserId) ON DELETE NO ACTION
);

CREATE TABLE ActionReportAttachments (
    AttachmentId        INT IDENTITY(1,1) PRIMARY KEY,
    ReportId            INT NOT NULL,
    FileType            VARCHAR(20) NOT NULL,       -- Document | Photo
    StoredFileName      NVARCHAR(500) NOT NULL,
    OriginalFileName    NVARCHAR(500) NOT NULL,
    FileSizeBytes       BIGINT NOT NULL,
    UploadedAt          DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_ARA_Report FOREIGN KEY (ReportId)
        REFERENCES ActionReports(ReportId) ON DELETE CASCADE
);

-- ── Indexes ───────────────────────────────────────────────────

CREATE INDEX IX_Petitions_Status ON Petitions(Status);
CREATE INDEX IX_Petitions_Mobile ON Petitions(MobileNumber);
CREATE INDEX IX_Petitions_AssignedOfficer ON Petitions(AssignedOfficerId);
CREATE INDEX IX_WorkflowLogs_PetitionId ON PetitionWorkflowLogs(PetitionId);
CREATE INDEX IX_WorkflowLogs_ActionAt ON PetitionWorkflowLogs(ActionAt);

-- ── Seed Data ─────────────────────────────────────────────────

INSERT INTO ComplaintCategories (Name, IsActive) VALUES
('Illegal Felling of Trees', 1),
('Encroachment of Forest Land', 1),
('Poaching / Wildlife Crime', 1),
('Corruption / Bribery', 1),
('Misappropriation of Funds', 1),
('Illegal Mining / Quarrying', 1),
('Forest Fire (Negligence)', 1),
('Other', 1);

INSERT INTO Districts (Name) VALUES
('Bengaluru Urban'), ('Bengaluru Rural'), ('Mysuru'), ('Tumakuru'),
('Shivamogga'), ('Dakshina Kannada'), ('Uttara Kannada'), ('Kodagu'),
('Hassan'), ('Chikkamagaluru'), ('Mandya'), ('Chamarajanagar'),
('Ramanagara'), ('Kolar'), ('Chitradurga'), ('Davanagere'),
('Haveri'), ('Dharwad'), ('Gadag'), ('Belagavi'),
('Vijayapura'), ('Bagalkot'), ('Ballari'), ('Koppal'),
('Raichur'), ('Yadgir'), ('Kalaburagi'), ('Bidar'),
('Udupi'), ('Chikkaballapur'), ('Vijayanagara');

-- Default system users (OTP will be set on first login)
INSERT INTO Users (MobileNumber, Name, Role, IsActive, CreatedAt) VALUES
('9000000001', 'APCCF Vigilance',  'APCCF',           1, GETUTCDATE()),
('9000000002', 'Office Assistant', 'OfficeAssistant',  1, GETUTCDATE());
