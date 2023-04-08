using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalago.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Igreja_Ministerio_MinisterioId",
                table: "Igreja");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Igreja");

            migrationBuilder.AlterColumn<int>(
                name: "MinisterioId",
                table: "Igreja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinisterioId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Igreja_Ministerio_MinisterioId",
                table: "Igreja",
                column: "MinisterioId",
                principalTable: "Ministerio",
                principalColumn: "MinisterioId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Igreja_Ministerio_MinisterioId",
                table: "Igreja");

            migrationBuilder.DropColumn(
                name: "MinisterioId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "MinisterioId",
                table: "Igreja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Igreja",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Igreja_Ministerio_MinisterioId",
                table: "Igreja",
                column: "MinisterioId",
                principalTable: "Ministerio",
                principalColumn: "MinisterioId");
        }
    }
}
