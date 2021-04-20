using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace weqi_store_api.Migrations
{
    public partial class producthasmanyimagesrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    imageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => new { x.imageId, x.ProductId });                    
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
