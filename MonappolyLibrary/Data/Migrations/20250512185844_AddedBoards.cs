using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonappolyLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBoards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
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
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardBoardSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoardIndex = table.Column<uint>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SpaceType = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardTypeId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_CardBoardSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardBoardSpaces_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardBoardSpaces_CardTypes_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenericBoardSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoardIndex = table.Column<uint>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SpaceType = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_GenericBoardSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenericBoardSpaces_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyBoardSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoardIndex = table.Column<uint>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SpaceType = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost = table.Column<uint>(type: "INTEGER", nullable: false),
                    PropertyType = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertySet = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_PropertyBoardSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyBoardSpaces_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxBoardSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoardIndex = table.Column<uint>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SpaceType = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TaxAmount = table.Column<uint>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_TaxBoardSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxBoardSpaces_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardBoardSpaces_BoardId",
                table: "CardBoardSpaces",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardBoardSpaces_CardTypeId",
                table: "CardBoardSpaces",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GenericBoardSpaces_BoardId",
                table: "GenericBoardSpaces",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyBoardSpaces_BoardId",
                table: "PropertyBoardSpaces",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxBoardSpaces_BoardId",
                table: "TaxBoardSpaces",
                column: "BoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardBoardSpaces");

            migrationBuilder.DropTable(
                name: "GenericBoardSpaces");

            migrationBuilder.DropTable(
                name: "PropertyBoardSpaces");

            migrationBuilder.DropTable(
                name: "TaxBoardSpaces");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
