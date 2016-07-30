using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineMe.Migrations
{
    public partial class MyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    MediaId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AngerScore = table.Column<double>(nullable: false),
                    CaptionSoloText = table.Column<string>(nullable: true),
                    CaptionsSoloConf = table.Column<double>(nullable: false),
                    CaptureDate = table.Column<DateTime>(nullable: false),
                    ContemptScore = table.Column<double>(nullable: false),
                    DisgustScore = table.Column<double>(nullable: false),
                    FearScore = table.Column<double>(nullable: false),
                    HappinessScore = table.Column<double>(nullable: false),
                    HighestEmotion = table.Column<string>(nullable: true),
                    IsMerged = table.Column<bool>(nullable: false),
                    MediaName = table.Column<string>(nullable: true),
                    NeutralScore = table.Column<double>(nullable: false),
                    SadnessScore = table.Column<double>(nullable: false),
                    SurpriseScore = table.Column<double>(nullable: false),
                    TagsSpacesSeperated = table.Column<string>(nullable: true),
                    adultScore = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.MediaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medias");
        }
    }
}
