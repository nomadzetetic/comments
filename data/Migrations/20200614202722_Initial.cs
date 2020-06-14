using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Comments.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    Tokens = table.Column<List<string>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Commentators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Banned = table.Column<bool>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commentators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commentators_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    ResourceId = table.Column<string>(maxLength: 1000, nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    Replies = table.Column<int>(nullable: false, defaultValue: 0),
                    Likes = table.Column<int>(nullable: false, defaultValue: 0),
                    Dislikes = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_Resources_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    CommentatorId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    ResourceId = table.Column<string>(nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Replies = table.Column<int>(nullable: false, defaultValue: 0),
                    Likes = table.Column<int>(nullable: false, defaultValue: 0),
                    Dislikes = table.Column<int>(nullable: false, defaultValue: 0),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Commentators_CommentatorId",
                        column: x => x.CommentatorId,
                        principalTable: "Commentators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(nullable: false),
                    CommentatorId = table.Column<Guid>(nullable: false),
                    Value = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.CommentId, x.CommentatorId });
                    table.ForeignKey(
                        name: "FK_Likes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Commentators_CommentatorId",
                        column: x => x.CommentatorId,
                        principalTable: "Commentators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commentators_TenantId",
                table: "Commentators",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Commentators_Name_TenantId",
                table: "Commentators",
                columns: new[] { "Name", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentatorId",
                table: "Comments",
                column: "CommentatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ResourceId",
                table: "Comments",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TenantId",
                table: "Comments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CommentatorId",
                table: "Likes",
                column: "CommentatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_TenantId",
                table: "Resources",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceId_TenantId",
                table: "Resources",
                columns: new[] { "ResourceId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Commentators");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
