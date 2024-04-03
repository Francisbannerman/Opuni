using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Referral.Migrations
{
    public partial class addAmountPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "Clients",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Clients");
        }
    }
}
