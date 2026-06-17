using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixPostgresLowercaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbldepartment",
                columns: table => new
                {
                    departmentid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departmentname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbldepartment", x => x.departmentid);
                });

            migrationBuilder.CreateTable(
                name: "tblstudent",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    course = table.Column<string>(type: "text", nullable: true),
                    semester = table.Column<int>(type: "integer", nullable: true),
                    cgpa = table.Column<decimal>(type: "numeric", nullable: true),
                    sgpa = table.Column<decimal>(type: "numeric", nullable: true),
                    password = table.Column<string>(type: "text", nullable: false),
                    dob = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    hometown = table.Column<string>(type: "text", nullable: true),
                    mobile = table.Column<string>(type: "text", nullable: true),
                    departmentid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblstudent", x => x.id);
                    table.ForeignKey(
                        name: "FK_tblstudent_tbldepartment_departmentid",
                        column: x => x.departmentid,
                        principalTable: "tbldepartment",
                        principalColumn: "departmentid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblstudent_departmentid",
                table: "tblstudent",
                column: "departmentid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblstudent");

            migrationBuilder.DropTable(
                name: "tbldepartment");
        }
    }
}
