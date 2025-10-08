using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banner",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    banner_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    banner_image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    banner_url = table.Column<string>(type: "TEXT", nullable: false),
                    banner_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    banner_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    target_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banner", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    user_profile_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client_transfer",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    location_google = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    cached_address_line = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    cached_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cached_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_transfer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "offline_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    order_shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offline_transaction", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "progress_transfer",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    order_shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    estimate_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    begin_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    end_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progress_transfer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shop",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    shop_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    shop_description = table.Column<string>(type: "TEXT", nullable: false),
                    shop_logo = table.Column<string>(type: "TEXT", nullable: false),
                    shop_banner = table.Column<string>(type: "TEXT", nullable: false),
                    shop_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    shop_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    shop_status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    shop_user_profile_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    shop_address_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cart_item",
                columns: table => new
                {
                    cart_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    sku_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    is_selected = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    added_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cached_product_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    cached_product_image = table.Column<string>(type: "TEXT", nullable: false),
                    cached_price = table.Column<double>(type: "double precision", nullable: false),
                    cached_shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    cached_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_item", x => new { x.cart_id, x.sku_id });
                    table.ForeignKey(
                        name: "FK_cart_item_cart_cart_id",
                        column: x => x.cart_id,
                        principalTable: "cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "progress_client",
                columns: table => new
                {
                    progress_transfer_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    client_transfer_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: false),
                    time_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progress_client", x => new { x.progress_transfer_id, x.client_transfer_id });
                    table.ForeignKey(
                        name: "FK_progress_client_client_transfer_client_transfer_id",
                        column: x => x.client_transfer_id,
                        principalTable: "client_transfer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_progress_client_progress_transfer_progress_transfer_id",
                        column: x => x.progress_transfer_id,
                        principalTable: "progress_transfer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "follow",
                columns: table => new
                {
                    shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    user_profile_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow", x => new { x.shop_id, x.user_profile_id });
                    table.ForeignKey(
                        name: "FK_follow_shop_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shop",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "voucher_shop",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    discount = table.Column<float>(type: "real", nullable: false),
                    start_available = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    min_support = table.Column<double>(type: "double precision", nullable: false),
                    max_discount = table.Column<double>(type: "double precision", nullable: false),
                    shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    image = table.Column<string>(type: "TEXT", nullable: false),
                    category_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    used = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucher_shop", x => x.id);
                    table.ForeignKey(
                        name: "FK_voucher_shop_shop_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shop",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wallet_shop",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_shop", x => x.id);
                    table.ForeignKey(
                        name: "FK_wallet_shop_shop_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shop",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shop_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    type = table.Column<bool>(type: "boolean", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    message = table.Column<string>(type: "TEXT", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    wallet_shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    order_shop_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_shop_transaction_wallet_shop_wallet_shop_id",
                        column: x => x.wallet_shop_id,
                        principalTable: "wallet_shop",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_banner_banner_type_is_active_banner_order",
                table: "banner",
                columns: new[] { "banner_type", "is_active", "banner_order" });

            migrationBuilder.CreateIndex(
                name: "IX_banner_start_date_end_date",
                table: "banner",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "IX_cart_user_profile_id",
                table: "cart",
                column: "user_profile_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_cached_at",
                table: "cart_item",
                column: "cached_at");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_cached_shop_id",
                table: "cart_item",
                column: "cached_shop_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_sku_id",
                table: "cart_item",
                column: "sku_id");

            migrationBuilder.CreateIndex(
                name: "IX_client_transfer_address_id",
                table: "client_transfer",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_user_profile_id",
                table: "follow",
                column: "user_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_offline_transaction_created_date",
                table: "offline_transaction",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "IX_offline_transaction_order_shop_id",
                table: "offline_transaction",
                column: "order_shop_id");

            migrationBuilder.CreateIndex(
                name: "IX_progress_client_client_transfer_id",
                table: "progress_client",
                column: "client_transfer_id");

            migrationBuilder.CreateIndex(
                name: "IX_progress_client_sort",
                table: "progress_client",
                column: "sort");

            migrationBuilder.CreateIndex(
                name: "IX_progress_transfer_estimate_time",
                table: "progress_transfer",
                column: "estimate_time");

            migrationBuilder.CreateIndex(
                name: "IX_progress_transfer_order_shop_id",
                table: "progress_transfer",
                column: "order_shop_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shop_shop_email",
                table: "shop",
                column: "shop_email");

            migrationBuilder.CreateIndex(
                name: "IX_shop_shop_status",
                table: "shop",
                column: "shop_status");

            migrationBuilder.CreateIndex(
                name: "IX_shop_shop_user_profile_id",
                table: "shop",
                column: "shop_user_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_shop_transaction_created_date",
                table: "shop_transaction",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "IX_shop_transaction_order_shop_id",
                table: "shop_transaction",
                column: "order_shop_id");

            migrationBuilder.CreateIndex(
                name: "IX_shop_transaction_wallet_shop_id",
                table: "shop_transaction",
                column: "wallet_shop_id");

            migrationBuilder.CreateIndex(
                name: "IX_voucher_shop_code",
                table: "voucher_shop",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voucher_shop_shop_id",
                table: "voucher_shop",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "IX_voucher_shop_start_available_end_is_active",
                table: "voucher_shop",
                columns: new[] { "start_available", "end", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_shop_shop_id",
                table: "wallet_shop",
                column: "shop_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "banner");

            migrationBuilder.DropTable(
                name: "cart_item");

            migrationBuilder.DropTable(
                name: "follow");

            migrationBuilder.DropTable(
                name: "offline_transaction");

            migrationBuilder.DropTable(
                name: "progress_client");

            migrationBuilder.DropTable(
                name: "shop_transaction");

            migrationBuilder.DropTable(
                name: "voucher_shop");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "client_transfer");

            migrationBuilder.DropTable(
                name: "progress_transfer");

            migrationBuilder.DropTable(
                name: "wallet_shop");

            migrationBuilder.DropTable(
                name: "shop");
        }
    }
}
