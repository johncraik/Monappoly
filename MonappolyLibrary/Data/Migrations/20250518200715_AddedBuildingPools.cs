using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBuildingPools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_BuildingGroups_BuildingGroupId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_BuildingGroupId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "BuildingGroupId",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Buildings",
                newName: "BuildingPoolId");

            migrationBuilder.CreateTable(
                name: "BuildingPools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Count = table.Column<uint>(type: "INTEGER", nullable: false),
                    BuildingGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingPools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingPools_BuildingGroups_BuildingGroupId",
                        column: x => x.BuildingGroupId,
                        principalTable: "BuildingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingPoolId",
                table: "Buildings",
                column: "BuildingPoolId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingPools_BuildingGroupId",
                table: "BuildingPools",
                column: "BuildingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_BuildingPools_BuildingPoolId",
                table: "Buildings",
                column: "BuildingPoolId",
                principalTable: "BuildingPools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_BuildingPools_BuildingPoolId",
                table: "Buildings");

            migrationBuilder.DropTable(
                name: "BuildingPools");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_BuildingPoolId",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "BuildingPoolId",
                table: "Buildings",
                newName: "Count");

            migrationBuilder.AddColumn<int>(
                name: "BuildingGroupId",
                table: "Buildings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingGroupId",
                table: "Buildings",
                column: "BuildingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_BuildingGroups_BuildingGroupId",
                table: "Buildings",
                column: "BuildingGroupId",
                principalTable: "BuildingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
