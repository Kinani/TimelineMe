using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineMe.Migrations
{
    public partial class MySecondMigiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    MediaId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CaptionSoloText = table.Column<string>(nullable: true),
                    CaptionsSoloConf = table.Column<double>(nullable: false),
                    CaptureDate = table.Column<DateTime>(nullable: false),
                    IsMerged = table.Column<bool>(nullable: false),
                    MediaName = table.Column<string>(nullable: true),
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
