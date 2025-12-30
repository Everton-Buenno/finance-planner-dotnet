using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    Nationality = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumber_CountryCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    PhoneNumber_Number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Address_Street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address_Complement = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address_ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Address_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    DarkMode = table.Column<bool>(type: "boolean", nullable: false),
                    Plan = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_Cpf",
                table: "users",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
