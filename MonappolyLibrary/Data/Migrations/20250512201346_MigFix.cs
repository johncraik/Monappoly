using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
