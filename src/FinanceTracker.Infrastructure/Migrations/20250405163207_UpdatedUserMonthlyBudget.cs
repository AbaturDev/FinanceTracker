using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserMonthlyBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveThisMonth",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "ResetInterval",
                table: "ExpensesPlanners");

            migrationBuilder.RenameColumn(
                name: "RegularIncome",
                table: "Incomes",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Incomes",
                newName: "RegularIncome");

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveThisMonth",
                table: "Incomes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ResetInterval",
                table: "ExpensesPlanners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
