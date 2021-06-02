using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class newRss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Change tut.by rss address to tjournal.ru
            migrationBuilder.Sql("UPDATE RSS SET Address = 'https://4pda.to/feed/' WHERE Id = 'F68FFBD2-3AE5-4E80-BE43-6021233C6EC9'; ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
