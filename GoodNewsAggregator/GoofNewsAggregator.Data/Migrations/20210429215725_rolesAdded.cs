using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class rolesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              table: "Roles",
              columns: new[] { "Id", "Name" },
              values: new object[]
              {
                    Guid.NewGuid(),
                    "User"
              });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
