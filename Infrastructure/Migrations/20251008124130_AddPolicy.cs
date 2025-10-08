using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "policy",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    policy_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    policy_content = table.Column<string>(type: "TEXT", nullable: false),
                    policy_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    effective_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_policy", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_policy_policy_type",
                table: "policy",
                column: "policy_type");

            migrationBuilder.CreateIndex(
                name: "IX_policy_policy_type_is_active_shop_id",
                table: "policy",
                columns: new[] { "policy_type", "is_active", "shop_id" });

            migrationBuilder.CreateIndex(
                name: "IX_policy_policy_type_version",
                table: "policy",
                columns: new[] { "policy_type", "version" });

            migrationBuilder.CreateIndex(
                name: "IX_policy_shop_id",
                table: "policy",
                column: "shop_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "policy");
        }
    }
}
