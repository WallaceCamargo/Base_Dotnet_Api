﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalago.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Igreja_IgrejaId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IgrejaId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Igreja_IgrejaId",
                table: "AspNetUsers",
                column: "IgrejaId",
                principalTable: "Igreja",
                principalColumn: "IgrejaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Igreja_IgrejaId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IgrejaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Igreja_IgrejaId",
                table: "AspNetUsers",
                column: "IgrejaId",
                principalTable: "Igreja",
                principalColumn: "IgrejaId");
        }
    }
}
