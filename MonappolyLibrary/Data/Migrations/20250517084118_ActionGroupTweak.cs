using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActionGroupTweak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayCondition",
                table: "CardActionGroups");

            migrationBuilder.AddColumn<string>(
                name: "WrappedPlayConditions",
                table: "CardActionGroups",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WrappedPlayConditions",
                table: "CardActionGroups");

            migrationBuilder.AddColumn<int>(
                name: "PlayCondition",
                table: "CardActionGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
