using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ciberpaz_api.Migrations
{
    /// <inheritdoc />
    public partial class HomeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "home",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GovIcon = table.Column<string>(type: "text", nullable: true),
                    AppIcon = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_home", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    GovIcon = table.Column<string>(type: "text", nullable: true),
                    AppIcon = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "viewitem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HomeId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Route = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    WebUrl = table.Column<string>(type: "text", nullable: true),
                    Embed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viewitem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_viewitem_home_HomeId",
                        column: x => x.HomeId,
                        principalTable: "home",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViewItemDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Route = table.Column<string>(type: "text", nullable: true),
                    WebUrl = table.Column<string>(type: "text", nullable: true),
                    Embed = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    HomeDtoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewItemDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewItemDto_HomeDto_HomeDtoId",
                        column: x => x.HomeDtoId,
                        principalTable: "HomeDto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_viewitem_HomeId",
                table: "viewitem",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewItemDto_HomeDtoId",
                table: "ViewItemDto",
                column: "HomeDtoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "viewitem");

            migrationBuilder.DropTable(
                name: "ViewItemDto");

            migrationBuilder.DropTable(
                name: "home");

            migrationBuilder.DropTable(
                name: "HomeDto");
        }
    }
}
