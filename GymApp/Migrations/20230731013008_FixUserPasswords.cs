using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApp.Migrations
{
    /// <inheritdoc />
    public partial class FixUserPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "e6e93ba2-161a-4568-97ba-d8e9931f9ce8",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "3184c030-2893-4f0b-a6ca-4a92f8a8fefd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d816df9-a2b3-4a6b-a111-69ce4a5d1332",
                column: "ConcurrencyStamp",
                value: "bd705754-ead4-4ce5-ac4c-3b5ff9130a55");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79d54256-912f-43fd-a014-38c007b8777e",
                column: "ConcurrencyStamp",
                value: "c1fd55bb-98bd-4a12-a409-c6ce258c8788");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab88a4ed-ba01-4f69-b1a3-9c1a8ebeea6b",
                column: "ConcurrencyStamp",
                value: "fac8f4b9-39bf-45e1-bf47-570c7c32d53b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4537842-50de-4734-a878-00e1b3a5639c",
                column: "ConcurrencyStamp",
                value: "394f5dcd-f51c-4b70-9a22-2024c7c55522");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "167af088-83bf-4770-b2e7-90bcab44a837",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9798a3de-0794-475d-9309-4e0a5d7cf3cb", "AQAAAAEAACcQAAAAEN0w88jfocgDr3waiyj80XyuasthXNTQ9IYL0ViCsGpjTITO9DnN8KmRBn9y2J568A==", "a02cf68f-b610-4593-a1c0-1e3d6fef6ee1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "517ef1d8-6ae9-417e-a50b-43393c317efa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e1389d4-af4a-4f79-8198-e9f3416f627d", "AQAAAAEAACcQAAAAEPbH7nslzidjLZpUr3ue89qd+no9zfAR4D31uZJ5yDYMxstEtPd2tJyi9JZ1eAOA/g==", "68019e75-35af-4821-a32d-031de75ccf0c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "3184c030-2893-4f0b-a6ca-4a92f8a8fefd",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "e6e93ba2-161a-4568-97ba-d8e9931f9ce8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d816df9-a2b3-4a6b-a111-69ce4a5d1332",
                column: "ConcurrencyStamp",
                value: "f8e7cff7-ef02-49cc-8f8b-445ac67ab140");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79d54256-912f-43fd-a014-38c007b8777e",
                column: "ConcurrencyStamp",
                value: "c6686e43-ad87-4f51-9441-9df4f0dd205b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab88a4ed-ba01-4f69-b1a3-9c1a8ebeea6b",
                column: "ConcurrencyStamp",
                value: "b2ad73f9-4eb6-43d6-a9da-23c20e41007f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4537842-50de-4734-a878-00e1b3a5639c",
                column: "ConcurrencyStamp",
                value: "fd29591b-0c88-448a-bfe9-8860fe6058fe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "167af088-83bf-4770-b2e7-90bcab44a837",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab7395c1-2e43-453c-9cd8-c3613a8f1cec", "AQAAAAEAACcQAAAAEDmB/RBA8Dt+OK+Z8pXFlYE1R0XEvHlNvvU21ozpFKqnqDDpNDFIqROIk34VtYmH1A==", "c6550937-6b14-4306-9e59-1a0c66f076e7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "517ef1d8-6ae9-417e-a50b-43393c317efa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a5b5b49-6f0a-4990-9bf8-ca7b8bdf2309", "AQAAAAEAACcQAAAAEDDm5OzELKaUexskXl/zVay6tRRWQKrRPQX9xUIL0f9Qwi8yb6sUFB6zNyD04UPNZw==", "db9d4bb1-ce7d-482a-b101-7cf7fd2fd5ed" });
        }
    }
}
