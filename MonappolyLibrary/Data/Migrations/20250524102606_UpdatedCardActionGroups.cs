using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCardActionGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupLengthType",
                table: "CardActionGroups");

            migrationBuilder.DropColumn(
                name: "IsForced",
                table: "CardActionGroups");

            migrationBuilder.DropColumn(
                name: "IsKeep",
                table: "CardActionGroups");

            migrationBuilder.DropColumn(
                name: "Player",
                table: "CardActionGroups");

            migrationBuilder.DropColumn(
                name: "WrappedPlayConditions",
                table: "CardActionGroups");

            migrationBuilder.CreateTable(
                name: "KeepActionConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActionGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsUntilNeeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlayCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupLengthType = table.Column<int>(type: "INTEGER", nullable: false),
                    LengthValue = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeepActionConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeepActionConditions_CardActionGroups_ActionGroupId",
                        column: x => x.ActionGroupId,
                        principalTable: "CardActionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeepActionConditions_ActionGroupId",
                table: "KeepActionConditions",
                column: "ActionGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeepActionConditions");

            migrationBuilder.AddColumn<int>(
                name: "GroupLengthType",
                table: "CardActionGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsForced",
                table: "CardActionGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsKeep",
                table: "CardActionGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Player",
                table: "CardActionGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WrappedPlayConditions",
                table: "CardActionGroups",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
