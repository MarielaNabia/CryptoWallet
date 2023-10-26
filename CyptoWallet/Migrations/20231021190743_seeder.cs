using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyptoWallet.Migrations
{
    public partial class seeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "AccountTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Pesos" },
                    { 2, "Dólares" },
                    { 3, "BTC" }
                });

            migrationBuilder.InsertData(
                table: "OperationType",
                columns: new[] { "OperationTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Transferencia" },
                    { 2, "Compra" },
                    { 3, "Depósito" },
                    { 4, "Venta" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OperationType",
                keyColumn: "OperationTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OperationType",
                keyColumn: "OperationTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OperationType",
                keyColumn: "OperationTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OperationType",
                keyColumn: "OperationTypeId",
                keyValue: 4);
        }
    }
}
