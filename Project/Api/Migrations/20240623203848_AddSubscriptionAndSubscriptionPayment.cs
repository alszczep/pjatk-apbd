using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionAndSubscriptionPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscription",
                schema: "apbdproject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RenewalPeriodInMonths = table.Column<int>(type: "int", nullable: false),
                    BasePriceForRenewalPeriod = table.Column<decimal>(type: "money", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoftwareProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "apbdproject",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_SoftwareProduct_SoftwareProductId",
                        column: x => x.SoftwareProductId,
                        principalSchema: "apbdproject",
                        principalTable: "SoftwareProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPayment",
                schema: "apbdproject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "money", nullable: false),
                    PeriodLastDay = table.Column<DateOnly>(type: "date", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayment_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalSchema: "apbdproject",
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ClientId",
                schema: "apbdproject",
                table: "Subscription",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SoftwareProductId",
                schema: "apbdproject",
                table: "Subscription",
                column: "SoftwareProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayment_SubscriptionId",
                schema: "apbdproject",
                table: "SubscriptionPayment",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionPayment",
                schema: "apbdproject");

            migrationBuilder.DropTable(
                name: "Subscription",
                schema: "apbdproject");
        }
    }
}
