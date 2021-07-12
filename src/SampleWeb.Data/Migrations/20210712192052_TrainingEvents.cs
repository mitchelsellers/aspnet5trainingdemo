using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleWeb.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class TrainingEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingEvents",
                columns: table => new
                {
                    TrainingEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingEvents", x => x.TrainingEventId);
                });

            migrationBuilder.CreateTable(
                name: "TrainingClasses",
                columns: table => new
                {
                    TrainingClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingEventId = table.Column<int>(type: "int", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaxAttendees = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingClasses", x => x.TrainingClassId);
                    table.ForeignKey(
                        name: "FK_TrainingClasses_TrainingEvents_TrainingEventId",
                        column: x => x.TrainingEventId,
                        principalTable: "TrainingEvents",
                        principalColumn: "TrainingEventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingClasses_TrainingEventId",
                table: "TrainingClasses",
                column: "TrainingEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingClasses");

            migrationBuilder.DropTable(
                name: "TrainingEvents");
        }
    }
}
