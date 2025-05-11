using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class CardActionsComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupLengthType",
                table: "CardActionGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupLengthType",
                table: "CardActionGroups");
        }
    }
}
