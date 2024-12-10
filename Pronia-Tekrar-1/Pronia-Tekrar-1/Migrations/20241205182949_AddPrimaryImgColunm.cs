using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia_Tekrar_1.Migrations
{
    /// <inheritdoc />
    public partial class AddPrimaryImgColunm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PrimaryImg",
                table: "productImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryImg",
                table: "productImages");
        }
    }
}
