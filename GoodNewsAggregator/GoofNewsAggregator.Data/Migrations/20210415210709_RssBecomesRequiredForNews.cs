using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class RssBecomesRequiredForNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_RSS_RSSId",
                table: "News");

            migrationBuilder.AlterColumn<Guid>(
                name: "RSSId",
                table: "News",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_News_RSS_RSSId",
                table: "News",
                column: "RSSId",
                principalTable: "RSS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_RSS_RSSId",
                table: "News");

            migrationBuilder.AlterColumn<Guid>(
                name: "RSSId",
                table: "News",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_News_RSS_RSSId",
                table: "News",
                column: "RSSId",
                principalTable: "RSS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
