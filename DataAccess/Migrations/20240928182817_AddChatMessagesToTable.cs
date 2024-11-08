using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessagesToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRCodeForMessaging",
                table: "Tables");

            migrationBuilder.RenameColumn(
                name: "QRCodeForOrderAndPay",
                table: "Tables",
                newName: "QRCodeTable");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QRCodeTable",
                table: "Tables",
                newName: "QRCodeForOrderAndPay");

            migrationBuilder.AddColumn<string>(
                name: "QRCodeForMessaging",
                table: "Tables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
