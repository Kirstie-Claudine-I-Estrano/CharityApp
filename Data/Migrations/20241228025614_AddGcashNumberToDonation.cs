using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharityApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGcashNumberToDonation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GcashNumber",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GcashNumber",
                table: "Donations");
        }
    }
}
