using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineMe.Migrations
{
    public partial class NewMigration : Migration
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

            migrationBuilder.CreateTable(
                name: "MediaGroups",
                columns: table => new
                {
                    MediaGroupId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AngerScoreMean = table.Column<double>(nullable: false),
                    CompostionFileName = table.Column<string>(nullable: true),
                    ContemptScoreMean = table.Column<double>(nullable: false),
                    DisgustScoreMean = table.Column<double>(nullable: false),
                    FearScoreMean = table.Column<double>(nullable: false),
                    FirstCreatedDate = table.Column<DateTime>(nullable: false),
                    HappinessScoreMean = table.Column<double>(nullable: false),
                    HighestEmotionMean = table.Column<string>(nullable: true),
                    LastEditDate = table.Column<DateTime>(nullable: false),
                    NeutralScoreMean = table.Column<double>(nullable: false),
                    SadnessScoreMean = table.Column<double>(nullable: false),
                    SurpriseScoreMean = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaGroups", x => x.MediaGroupId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "MediaGroups");
        }
    }
}
