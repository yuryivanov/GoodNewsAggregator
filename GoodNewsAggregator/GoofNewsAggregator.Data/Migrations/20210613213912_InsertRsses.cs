using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class InsertRsses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              table: "RSS",
              columns: new[] { "Id", "Address" },
              values: new object[]
              {
                    new Guid("B637E8D9-DC89-4FEB-951C-23D3BAA7C48D"),
                    "http://s13.ru/rss"
              });
            migrationBuilder.InsertData(
              table: "RSS",
              columns: new[] { "Id", "Address" },
              values: new object[]
              {
                    new Guid("972036B6-175F-4251-B2D9-296A77B65169"),
                    "https://www.onliner.by/feed"
              });
            migrationBuilder.InsertData(
               table: "RSS",
               columns: new[] { "Id", "Address" },
               values: new object[]
               {
                   new Guid("F68FFBD2-3AE5-4E80-BE43-6021233C6EC9"),
                    "https://4pda.to/feed/"
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
