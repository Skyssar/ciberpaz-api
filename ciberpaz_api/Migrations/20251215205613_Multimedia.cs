using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ciberpaz_api.Migrations
{
    /// <inheritdoc />
    public partial class Multimedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paragraphs_views_ViewId",
                table: "paragraphs");

            migrationBuilder.DropForeignKey(
                name: "FK_sections_views_ViewId",
                table: "sections");

            migrationBuilder.DropTable(
                name: "menuitem");

            migrationBuilder.DropTable(
                name: "views");

            migrationBuilder.RenameColumn(
                name: "ViewId",
                table: "sections",
                newName: "AppViewId");

            migrationBuilder.RenameIndex(
                name: "IX_sections_ViewId",
                table: "sections",
                newName: "IX_sections_AppViewId");

            migrationBuilder.RenameColumn(
                name: "ViewId",
                table: "paragraphs",
                newName: "AppViewId");

            migrationBuilder.RenameIndex(
                name: "IX_paragraphs_ViewId",
                table: "paragraphs",
                newName: "IX_paragraphs_AppViewId");

            migrationBuilder.CreateTable(
                name: "appviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Route = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "multimedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_multimedias", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_paragraphs_appviews_AppViewId",
                table: "paragraphs",
                column: "AppViewId",
                principalTable: "appviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sections_appviews_AppViewId",
                table: "sections",
                column: "AppViewId",
                principalTable: "appviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paragraphs_appviews_AppViewId",
                table: "paragraphs");

            migrationBuilder.DropForeignKey(
                name: "FK_sections_appviews_AppViewId",
                table: "sections");

            migrationBuilder.DropTable(
                name: "appviews");

            migrationBuilder.DropTable(
                name: "multimedias");

            migrationBuilder.RenameColumn(
                name: "AppViewId",
                table: "sections",
                newName: "ViewId");

            migrationBuilder.RenameIndex(
                name: "IX_sections_AppViewId",
                table: "sections",
                newName: "IX_sections_ViewId");

            migrationBuilder.RenameColumn(
                name: "AppViewId",
                table: "paragraphs",
                newName: "ViewId");

            migrationBuilder.RenameIndex(
                name: "IX_paragraphs_AppViewId",
                table: "paragraphs",
                newName: "IX_paragraphs_ViewId");

            migrationBuilder.CreateTable(
                name: "menuitem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    Main = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menuitem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "views",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_views", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_paragraphs_views_ViewId",
                table: "paragraphs",
                column: "ViewId",
                principalTable: "views",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sections_views_ViewId",
                table: "sections",
                column: "ViewId",
                principalTable: "views",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
