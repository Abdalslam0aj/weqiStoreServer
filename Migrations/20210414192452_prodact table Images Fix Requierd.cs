using Microsoft.EntityFrameworkCore.Migrations;

namespace weqi_store_api.Migrations
{
    public partial class prodacttableImagesFixRequierd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Products_productId",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                newName: "ProductImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImage_productId",
                table: "ProductImages",
                newName: "IX_ProductImages_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                columns: new[] { "imageId", "productId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_productId",
                table: "ProductImages",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_productId",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProductImage");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_productId",
                table: "ProductImage",
                newName: "IX_ProductImage_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage",
                columns: new[] { "imageId", "productId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Products_productId",
                table: "ProductImage",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
