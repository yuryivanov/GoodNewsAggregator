using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class AddFullNameColumnToCommentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Comments");
        }
    }
}
