using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class Number2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "RSS",
               columns: new[] { "Id", "Address" },
               values: new object[]
               {
                    Guid.NewGuid(),
                    "https://news.tut.by/rss/all.rss"
               });
            migrationBuilder.InsertData(
              table: "RSS",
              columns: new[] { "Id", "Address" },
              values: new object[]
              {
                    Guid.NewGuid(),
                    "http://s13.ru/rss"
              });
            migrationBuilder.InsertData(
               table: "RSS",
               columns: new[] { "Id", "Address" },
               values: new object[]
               {
                    Guid.NewGuid(),
                    "https://www.onliner.by/feed"
               });          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
