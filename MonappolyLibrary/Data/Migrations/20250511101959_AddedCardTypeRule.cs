using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCardTypeRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rule",
                table: "CardTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rule",
                table: "CardTypes");
        }
    }
}
