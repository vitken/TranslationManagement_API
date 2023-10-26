﻿using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TranslationJobs_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<JobStatus>(type: "INTEGER", nullable: true),
                    OriginalContent = table.Column<string>(type: "TEXT", nullable: true),
                    TranslatedContent = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translators_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    HourlyRate = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<TranslatorStatus>(type: "INTEGER", nullable: true),
                    CreditCardNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translators", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslationJobs_tbl");

            migrationBuilder.DropTable(
                name: "Translators_tbl");
        }
    }
}