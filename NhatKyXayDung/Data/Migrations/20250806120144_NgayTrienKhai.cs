﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NhatKyXayDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class NgayTrienKhai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTao",
                table: "NhatKy",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTrienKhai",
                table: "NhatKy",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayTrienKhai",
                table: "NhatKy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTao",
                table: "NhatKy",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
