using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCurrencyProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Date",
                table: "UserMonthlyBudgets");

            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Mid",
                table: "UserMonthlyBudgets");

            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Date",
                table: "SavingGoals");

            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Mid",
                table: "SavingGoals");

            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Date",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Mid",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Date",
                table: "ExpensesPlanners");

            migrationBuilder.DropColumn(
                name: "OriginalExchangeRate_Mid",
                table: "ExpensesPlanners");

            migrationBuilder.RenameColumn(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "UserMonthlyBudgets",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "SavingGoals",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "Incomes",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "ExpensesPlanners",
                newName: "CurrencyCode");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "UserMonthlyBudgets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "SavingGoals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "Incomes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "ExpensesPlanners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "UserMonthlyBudgets",
                newName: "OriginalExchangeRate_CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "SavingGoals",
                newName: "OriginalExchangeRate_CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Incomes",
                newName: "OriginalExchangeRate_CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "ExpensesPlanners",
                newName: "OriginalExchangeRate_CurrencyCode");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "UserMonthlyBudgets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "OriginalExchangeRate_Date",
                table: "UserMonthlyBudgets",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalExchangeRate_Mid",
                table: "UserMonthlyBudgets",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "SavingGoals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "OriginalExchangeRate_Date",
                table: "SavingGoals",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalExchangeRate_Mid",
                table: "SavingGoals",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "Incomes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "OriginalExchangeRate_Date",
                table: "Incomes",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalExchangeRate_Mid",
                table: "Incomes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalExchangeRate_CurrencyCode",
                table: "ExpensesPlanners",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "OriginalExchangeRate_Date",
                table: "ExpensesPlanners",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalExchangeRate_Mid",
                table: "ExpensesPlanners",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
