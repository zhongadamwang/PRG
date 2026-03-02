using Microsoft.EntityFrameworkCore.Migrations;

namespace Sanjel.RequestManagement.Core.Migrations;

/// <summary>
/// Database migration: InitialSchemaFixed
/// Generated from domain model metadata
/// </summary>
public partial class M20260302022607_InitialSchemaFixed : Migration
{
    /// <summary>
    /// Create database schema from domain model
    /// </summary>
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Create Request table
        migrationBuilder.CreateTable(
            name: "Requests",
            columns: table => new
            {
                RequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Priority = table.Column<int>(type: "int", nullable: false),
                ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                SourceEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AssignedEngineerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AssignedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AcknowledgmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                StateDiagramId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Requests", x => x.RequestId);
                table.ForeignKey(
                    name: "FK_Requests_StateDiagrams_StateDiagramId",
                    column: x => x.StateDiagramId,
                    principalTable: "StateDiagrams",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        // Create DataElement table
        migrationBuilder.CreateTable(
            name: "DataElements",
            columns: table => new
            {
                ElementId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ElementType = table.Column<int>(type: "int", nullable: false),
                RawValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ValidatedValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ValidationStatus = table.Column<int>(type: "int", nullable: false),
                SourceLocation = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ValidationNotes = table.Column<string>(type: "nvarchar(450)", nullable: false),
                StateDiagramId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DataElements", x => x.ElementId);
                table.ForeignKey(
                    name: "FK_DataElements_StateDiagrams_StateDiagramId",
                    column: x => x.StateDiagramId,
                    principalTable: "StateDiagrams",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        // Create StateDiagram table
        migrationBuilder.CreateTable(
            name: "StateDiagrams",
            columns: table => new
            {
                DiagramId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                DiagramName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                FilePath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Version = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ParsingConfidence = table.Column<float>(type: "real", nullable: false),
                ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                DiagramType = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StateDiagrams", x => x.DiagramId);
            });

        // Create ReviewPackage table
        migrationBuilder.CreateTable(
            name: "ReviewPackages",
            columns: table => new
            {
                PackageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                SubmittingEngineerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AssignedReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ReviewCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ReviewStatus = table.Column<int>(type: "int", nullable: false),
                WorkSummary = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ReviewFeedback = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReviewPackages", x => x.PackageId);
            });

        // Create Notification table
        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                NotificationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RecipientType = table.Column<int>(type: "int", nullable: false),
                NotificationType = table.Column<int>(type: "int", nullable: false),
                DeliveryMethod = table.Column<int>(type: "int", nullable: false),
                SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Content = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ActionButtons = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.NotificationId);
            });

        // Create performance indexes
        // Index on Request.ClientId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_Requests_ClientId",
            table: "Requests",
            column: "ClientId");

        // Index on Request.SourceEmail for performance
        migrationBuilder.CreateIndex(
            name: "IX_Requests_SourceEmail",
            table: "Requests",
            column: "SourceEmail");

        // Index on Request.AssignedEngineerId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_Requests_AssignedEngineerId",
            table: "Requests",
            column: "AssignedEngineerId");

        // Index on DataElement.ElementId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_DataElements_ElementId",
            table: "DataElements",
            column: "ElementId");

        // Index on DataElement.RequestId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_DataElements_RequestId",
            table: "DataElements",
            column: "RequestId");

        // Index on StateDiagram.DiagramId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_StateDiagrams_DiagramId",
            table: "StateDiagrams",
            column: "DiagramId");

        // Index on StateDiagram.ClientId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_StateDiagrams_ClientId",
            table: "StateDiagrams",
            column: "ClientId");

        // Index on ReviewPackage.PackageId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_ReviewPackages_PackageId",
            table: "ReviewPackages",
            column: "PackageId");

        // Index on ReviewPackage.RequestId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_ReviewPackages_RequestId",
            table: "ReviewPackages",
            column: "RequestId");

        // Index on ReviewPackage.SubmittingEngineerId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_ReviewPackages_SubmittingEngineerId",
            table: "ReviewPackages",
            column: "SubmittingEngineerId");

        // Index on ReviewPackage.AssignedReviewerId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_ReviewPackages_AssignedReviewerId",
            table: "ReviewPackages",
            column: "AssignedReviewerId");

        // Index on Notification.RequestId foreign key
        migrationBuilder.CreateIndex(
            name: "IX_Notifications_RequestId",
            table: "Notifications",
            column: "RequestId");

    }

    /// <summary>
    /// Rollback database schema changes
    /// </summary>
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropTable(
            name: "ReviewPackages");

        migrationBuilder.DropTable(
            name: "StateDiagrams");

        migrationBuilder.DropTable(
            name: "DataElements");

        migrationBuilder.DropTable(
            name: "Requests");

    }
}