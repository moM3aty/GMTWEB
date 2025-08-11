﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GMTWEB.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectTypeToWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Websites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Websites");
        }
    }
}
