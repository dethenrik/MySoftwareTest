using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareTest.Migrations
{
    /// <inheritdoc />
    public partial class AddCprAndTodoItemRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "Task",
                table: "TodoItems",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "CprTables",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TodoItems",
                newName: "Task");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CprTables",
                newName: "Username");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "TodoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
