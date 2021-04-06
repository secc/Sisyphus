using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sisyphus.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "AppLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    DateTime = table.Column<DateTime>( type: "datetime2", nullable: false ),
                    LogType = table.Column<int>( type: "int", nullable: false ),
                    Message = table.Column<string>( type: "nvarchar(max)", nullable: true )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_AppLogs", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Key = table.Column<string>( type: "nvarchar(450)", nullable: true ),
                    Value = table.Column<string>( type: "nvarchar(max)", nullable: true )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_AppSettings", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "Bundles",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Name = table.Column<string>( type: "nvarchar(max)", nullable: true ),
                    Description = table.Column<string>( type: "nvarchar(max)", nullable: true )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Bundles", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Name = table.Column<string>( type: "nvarchar(max)", nullable: true ),
                    FullyQualifiedTypeName = table.Column<string>( type: "nvarchar(max)", nullable: true )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Providers", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Name = table.Column<string>( type: "nvarchar(max)", nullable: false ),
                    ProviderId = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Settings = table.Column<string>( type: "nvarchar(max)", nullable: true )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Operations", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Operations_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateTable(
                name: "BundleOpperations",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    BundleId = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    OperationId = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Order = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_BundleOpperations", x => x.Id );
                    table.ForeignKey(
                        name: "FK_BundleOpperations_Bundles_BundleId",
                        column: x => x.BundleId,
                        principalTable: "Bundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                    table.ForeignKey(
                        name: "FK_BundleOpperations_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_Key",
                table: "AppSettings",
                column: "Key" );

            migrationBuilder.CreateIndex(
                name: "IX_BundleOpperations_BundleId",
                table: "BundleOpperations",
                column: "BundleId" );

            migrationBuilder.CreateIndex(
                name: "IX_BundleOpperations_OperationId",
                table: "BundleOpperations",
                column: "OperationId" );

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ProviderId",
                table: "Operations",
                column: "ProviderId" );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "AppLogs" );

            migrationBuilder.DropTable(
                name: "AppSettings" );

            migrationBuilder.DropTable(
                name: "BundleOpperations" );

            migrationBuilder.DropTable(
                name: "Bundles" );

            migrationBuilder.DropTable(
                name: "Operations" );

            migrationBuilder.DropTable(
                name: "Providers" );
        }
    }
}
