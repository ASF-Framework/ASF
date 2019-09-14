using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASF.Web.Migrations
{
    public partial class sqllite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Username = table.Column<string>(maxLength: 32, nullable: false),
                    Password = table.Column<string>(maxLength: 32, nullable: false),
                    Telephone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(maxLength: 225, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Roles = table.Column<string>(maxLength: 20000, nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LoginIp = table.Column<string>(maxLength: 20, nullable: true),
                    LoginTime = table.Column<DateTime>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    Expired = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogInfos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(nullable: false),
                    AccountName = table.Column<string>(maxLength: 32, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(maxLength: 200, nullable: false),
                    ClientIp = table.Column<string>(maxLength: 20, nullable: false),
                    PermissionId = table.Column<string>(maxLength: 150, nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false),
                    ApiAddress = table.Column<string>(maxLength: 500, nullable: true),
                    ApiRequest = table.Column<string>(nullable: true),
                    ApiResponse = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    Code = table.Column<string>(nullable: false),
                    ParentId = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false),
                    ApiTemplate = table.Column<string>(maxLength: 500, nullable: true),
                    IsLogger = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Enable = table.Column<bool>(nullable: false),
                    MenuIcon = table.Column<string>(maxLength: 20, nullable: true),
                    MenuRedirect = table.Column<string>(maxLength: 300, nullable: true),
                    MenuHidden = table.Column<bool>(nullable: false),
                    HttpMethods = table.Column<string>(maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Enable = table.Column<bool>(nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Permissions = table.Column<string>(maxLength: 20000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "LogInfos");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
