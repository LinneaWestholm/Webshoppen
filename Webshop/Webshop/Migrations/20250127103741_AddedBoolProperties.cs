using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class AddedBoolProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "ShoppingCarts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOnFrontPage",
                table: "Products",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShowOnFrontPage",
                table: "Products");
        }
    }
}
