using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Referral.Migrations
{
    public partial class addStripeAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBusiness",
                table: "Clients",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StripeAccountId",
                table: "Clients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeAccountLink",
                table: "Clients",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBusiness",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "StripeAccountId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "StripeAccountLink",
                table: "Clients");
        }
    }
}
