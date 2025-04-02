using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNormalizedEmailsAndUserNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "7165afc3-1557-4ca8-99fa-2a8fc2f99a9c",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "e6e93ba2-161a-4568-97ba-d8e9931f9ce8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d816df9-a2b3-4a6b-a111-69ce4a5d1332",
                column: "ConcurrencyStamp",
                value: "5ecef228-e01c-4225-9247-4b86b607f732");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79d54256-912f-43fd-a014-38c007b8777e",
                column: "ConcurrencyStamp",
                value: "f320b08b-f5db-4396-80c0-30f82cafe4f3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab88a4ed-ba01-4f69-b1a3-9c1a8ebeea6b",
                column: "ConcurrencyStamp",
                value: "56323d9b-f6f6-4f40-813b-fbacfcf50918");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4537842-50de-4734-a878-00e1b3a5639c",
                column: "ConcurrencyStamp",
                value: "f991ccb8-3bd6-48eb-96f4-c06b3ad247eb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "167af088-83bf-4770-b2e7-90bcab44a837",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "df510b15-208a-4f86-b935-6988a40a4e7a", "MEDO.DOOOD2211@GMAIL.COM", "MEDO.DOOOD2211@GMAIL.COM", "AQAAAAEAACcQAAAAELfTNggYoTZxGFCvonAhwHRarLz0Q2F6yGdjP8t2sMj7rNL/9/tYILEVeG0cPEvKYQ==", "59ad822b-adf6-4c81-a1ba-d775205f5ae8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "517ef1d8-6ae9-417e-a50b-43393c317efa",
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b22ee37f-7ef9-453c-9708-6b752f5f18d0", true, "ASLGKAJSLDG@GMAIL.COM", "ASLGKAJSLDG@GMAIL.COM", "AQAAAAEAACcQAAAAECgzmjc4tqhf3sgDrBegiPPwvJ1RFrorFwLZ/ntZ4sRPurQ5F00iPdxbYXow8OXWqw==", "cfd3f125-aa3f-4b95-b95d-c012750ce7ae" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "e6e93ba2-161a-4568-97ba-d8e9931f9ce8",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "7165afc3-1557-4ca8-99fa-2a8fc2f99a9c");

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
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9798a3de-0794-475d-9309-4e0a5d7cf3cb", null, null, "AQAAAAEAACcQAAAAEN0w88jfocgDr3waiyj80XyuasthXNTQ9IYL0ViCsGpjTITO9DnN8KmRBn9y2J568A==", "a02cf68f-b610-4593-a1c0-1e3d6fef6ee1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "517ef1d8-6ae9-417e-a50b-43393c317efa",
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e1389d4-af4a-4f79-8198-e9f3416f627d", false, null, null, "AQAAAAEAACcQAAAAEPbH7nslzidjLZpUr3ue89qd+no9zfAR4D31uZJ5yDYMxstEtPd2tJyi9JZ1eAOA/g==", "68019e75-35af-4821-a32d-031de75ccf0c" });
        }
    }
}
