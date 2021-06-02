using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class AdminRoleAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
             table: "Roles",
             columns: new[] { "Id", "Name" },
             values: new object[]
             {
                    Guid.NewGuid(),
                    "Admin"
             });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
