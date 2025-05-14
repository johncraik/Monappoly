using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class CardTypeNameChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rule",
                table: "CardTypes",
                newName: "TypeRule");

            migrationBuilder.RenameColumn(
                name: "Condition",
                table: "CardTypes",
                newName: "PlayCondition");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeRule",
                table: "CardTypes",
                newName: "Rule");

            migrationBuilder.RenameColumn(
                name: "PlayCondition",
                table: "CardTypes",
                newName: "Condition");
        }
    }
}
