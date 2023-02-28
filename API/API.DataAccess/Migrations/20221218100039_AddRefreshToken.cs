using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.DataAccess.Migrations
{
    public partial class AddRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAppRefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppRefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAppRefreshToken_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAppRefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationClaim_ApplicationClaimId",
                table: "UserApplicationClaim",
                column: "ApplicationClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationClaim_UserId",
                table: "UserApplicationClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppRefreshToken_ApplicationId",
                table: "UserAppRefreshToken",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppRefreshToken_UserId",
                table: "UserAppRefreshToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserApplicationClaim_ApplicationClaim_ApplicationClaimId",
                table: "UserApplicationClaim",
                column: "ApplicationClaimId",
                principalTable: "ApplicationClaim",
                principalColumn: "ApplicationClaimsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserApplicationClaim_User_UserId",
                table: "UserApplicationClaim",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserApplicationClaim_ApplicationClaim_ApplicationClaimId",
                table: "UserApplicationClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_UserApplicationClaim_User_UserId",
                table: "UserApplicationClaim");

            migrationBuilder.DropTable(
                name: "UserAppRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_UserApplicationClaim_ApplicationClaimId",
                table: "UserApplicationClaim");

            migrationBuilder.DropIndex(
                name: "IX_UserApplicationClaim_UserId",
                table: "UserApplicationClaim");
        }
    }
}
