using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PicShare_GCPDemo.Migrations
{
    public partial class UpdatePictureFilePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Picture");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Picture",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Picture");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Picture",
                nullable: true);
        }
    }
}
