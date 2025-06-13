using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FruitsAppBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class alterRelationShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Rates_RateForeignKey",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RateForeignKey",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "RateForeignKey",
                table: "Reviews",
                newName: "ProductId");

            migrationBuilder.AddColumn<string>(
                name: "AppUserForeignKey",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AppUserForeignKey",
                table: "Reviews",
                column: "AppUserForeignKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUserForeignKey",
                table: "Reviews",
                column: "AppUserForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUserForeignKey",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AppUserForeignKey",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AppUserForeignKey",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Reviews",
                newName: "RateForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RateForeignKey",
                table: "Reviews",
                column: "RateForeignKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Rates_RateForeignKey",
                table: "Reviews",
                column: "RateForeignKey",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
