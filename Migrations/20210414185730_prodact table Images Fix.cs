using Microsoft.EntityFrameworkCore.Migrations;

namespace weqi_store_api.Migrations
{
    public partial class prodacttableImagesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "videoUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    imageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    productId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => new { x.imageId, x.productId });
                    table.ForeignKey(
                        name: "FK_ProductImage_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_productId",
                table: "ProductImage",
                column: "productId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropColumn(
                name: "videoUrl",
                table: "Products");
        }
    }
}
