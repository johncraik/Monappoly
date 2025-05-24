using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class MissingDataModelInKeepConditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "KeepActionConditions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "KeepActionConditions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "KeepActionConditions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "KeepActionConditions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KeepActionConditions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "KeepActionConditions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "KeepActionConditions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "KeepActionConditions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "KeepActionConditions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "KeepActionConditions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "KeepActionConditions");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "KeepActionConditions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KeepActionConditions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "KeepActionConditions");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "KeepActionConditions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "KeepActionConditions");
        }
    }
}
