using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InsuranceManagement.Migrations
{
    public partial class AddFK_UserforPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "userID",
                table: "payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_payments_userID",
                table: "payments",
                column: "userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User",
                table: "payments",
                column: "userID",
                principalTable: "users",
                principalColumn: "userID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_userID",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "userID",
                table: "payments");
        }
    }
}
