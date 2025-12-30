using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreditCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCard_Accounts_AccountId",
                table: "CreditCard");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreditCard",
                table: "CreditCard");

            migrationBuilder.RenameTable(
                name: "CreditCard",
                newName: "CreditCards");

            migrationBuilder.RenameIndex(
                name: "IX_CreditCard_AccountId",
                table: "CreditCards",
                newName: "IX_CreditCards_AccountId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Transactions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CreditCardId",
                table: "Transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CreditCards",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "ClosingDay",
                table: "CreditCards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "CreditCards",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CreditCards",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DueDay",
                table: "CreditCards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CreditCards",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreditCards",
                table: "CreditCards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreditCardId",
                table: "Transactions",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_AccountId_Description",
                table: "CreditCards",
                columns: new[] { "AccountId", "Description" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_IsActive",
                table: "CreditCards",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_IsDeleted",
                table: "CreditCards",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_Accounts_AccountId",
                table: "CreditCards",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_CreditCards_CreditCardId",
                table: "Transactions",
                column: "CreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_Accounts_AccountId",
                table: "CreditCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_CreditCards_CreditCardId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CreditCardId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreditCards",
                table: "CreditCards");

            migrationBuilder.DropIndex(
                name: "IX_CreditCards_AccountId_Description",
                table: "CreditCards");

            migrationBuilder.DropIndex(
                name: "IX_CreditCards_IsActive",
                table: "CreditCards");

            migrationBuilder.DropIndex(
                name: "IX_CreditCards_IsDeleted",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ClosingDay",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "DueDay",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CreditCards");

            migrationBuilder.RenameTable(
                name: "CreditCards",
                newName: "CreditCard");

            migrationBuilder.RenameIndex(
                name: "IX_CreditCards_AccountId",
                table: "CreditCard",
                newName: "IX_CreditCard_AccountId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CreditCard",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreditCard",
                table: "CreditCard",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCard_Accounts_AccountId",
                table: "CreditCard",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
