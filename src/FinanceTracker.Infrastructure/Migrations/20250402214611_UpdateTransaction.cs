using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionSource",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ExchangeRate_Mid",
                table: "Transactions",
                newName: "TargetExchangeRate_Mid");

            migrationBuilder.RenameColumn(
                name: "ExchangeRate_Date",
                table: "Transactions",
                newName: "TargetExchangeRate_Date");

            migrationBuilder.RenameColumn(
                name: "ExchangeRate_CurrencyCode",
                table: "Transactions",
                newName: "TargetExchangeRate_CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transactions",
                newName: "OriginalAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExpenses",
                table: "UserMonthlyBudgets",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BudgetExchangeRate_CurrencyCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BudgetExchangeRate_Date",
                table: "Transactions",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BudgetExchangeRate_Mid",
                table: "Transactions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CalculatedAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalExpenses",
                table: "UserMonthlyBudgets");

            migrationBuilder.DropColumn(
                name: "BudgetExchangeRate_CurrencyCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BudgetExchangeRate_Date",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BudgetExchangeRate_Mid",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CalculatedAmount",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TargetExchangeRate_Mid",
                table: "Transactions",
                newName: "ExchangeRate_Mid");

            migrationBuilder.RenameColumn(
                name: "TargetExchangeRate_Date",
                table: "Transactions",
                newName: "ExchangeRate_Date");

            migrationBuilder.RenameColumn(
                name: "TargetExchangeRate_CurrencyCode",
                table: "Transactions",
                newName: "ExchangeRate_CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "OriginalAmount",
                table: "Transactions",
                newName: "Amount");

            migrationBuilder.AddColumn<int>(
                name: "TransactionSource",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
