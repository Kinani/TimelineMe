using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineMe.Migrations
{
    public partial class MyThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AngerScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContemptScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DisgustScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FearScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HappinessScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HighestEmotion",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NeutralScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SadnessScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SurpriseScore",
                table: "Medias",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AngerScore",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "ContemptScore",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "DisgustScore",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "FearScore",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "HappinessScore",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "HighestEmotion",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "NeutralScore",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "SadnessScore",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "SurpriseScore",
                table: "Medias");
        }
    }
}
