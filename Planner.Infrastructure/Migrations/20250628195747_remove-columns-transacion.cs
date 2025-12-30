using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removecolumnstransacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "NextRecurrenceDate",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "RecurrenceType",
                table: "Transactions",
                newName: "RepeatType");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionOrigemId",
                table: "Transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionOrigemId",
                table: "Transactions",
                column: "TransactionOrigemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Transactions_TransactionOrigemId",
                table: "Transactions",
                column: "TransactionOrigemId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Transactions_TransactionOrigemId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionOrigemId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionOrigemId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "RepeatType",
                table: "Transactions",
                newName: "RecurrenceType");

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRecurrenceDate",
                table: "Transactions",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
