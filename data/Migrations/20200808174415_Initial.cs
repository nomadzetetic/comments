using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Comments.Data.Migrations
{
  public partial class Initial : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        "accounts",
        table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Banned = table.Column<bool>(nullable: false),
          DisplayName = table.Column<string>(maxLength: 50, nullable: false),
          Created = table.Column<DateTimeOffset>(nullable: false),
          Updated = table.Column<DateTimeOffset>(nullable: false)
        },
        constraints: table => { table.PrimaryKey("PK_accounts", x => x.Id); });

      migrationBuilder.CreateTable(
        "resources",
        table => new
        {
          ResourceKey = table.Column<string>(maxLength: 1000, nullable: false),
          Replies = table.Column<int>(nullable: false, defaultValue: 0),
          Likes = table.Column<int>(nullable: false, defaultValue: 0),
          Dislikes = table.Column<int>(nullable: false, defaultValue: 0),
          Created = table.Column<DateTimeOffset>(nullable: false),
          Updated = table.Column<DateTimeOffset>(nullable: false)
        },
        constraints: table => { table.PrimaryKey("PK_resources", x => x.ResourceKey); });

      migrationBuilder.CreateTable(
        "comments",
        table => new
        {
          Id = table.Column<Guid>(nullable: false),
          ParentId = table.Column<Guid>(nullable: true),
          AccountId = table.Column<Guid>(nullable: false),
          ResourceKey = table.Column<string>(nullable: false),
          Message = table.Column<string>("text", nullable: false),
          Replies = table.Column<int>(nullable: false, defaultValue: 0),
          Likes = table.Column<int>(nullable: false, defaultValue: 0),
          Dislikes = table.Column<int>(nullable: false, defaultValue: 0),
          Created = table.Column<DateTimeOffset>(nullable: false),
          Updated = table.Column<DateTimeOffset>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_comments", x => x.Id);
          table.ForeignKey(
            "FK_comments_accounts_AccountId",
            x => x.AccountId,
            "accounts",
            "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            "FK_comments_comments_ParentId",
            x => x.ParentId,
            "comments",
            "Id",
            onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
            "FK_comments_resources_ResourceKey",
            x => x.ResourceKey,
            "resources",
            "ResourceKey",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateTable(
        "reactions",
        table => new
        {
          CommentId = table.Column<Guid>(nullable: false),
          AccountId = table.Column<Guid>(nullable: false),
          ResourceKey = table.Column<string>(nullable: false),
          Value = table.Column<bool>(nullable: false),
          Created = table.Column<DateTimeOffset>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_reactions", x => new {x.CommentId, x.AccountId, x.ResourceKey});
          table.ForeignKey(
            "FK_reactions_accounts_AccountId",
            x => x.AccountId,
            "accounts",
            "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            "FK_reactions_comments_CommentId",
            x => x.CommentId,
            "comments",
            "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            "FK_reactions_resources_ResourceKey",
            x => x.ResourceKey,
            "resources",
            "ResourceKey",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateIndex(
        "IX_comments_AccountId",
        "comments",
        "AccountId");

      migrationBuilder.CreateIndex(
        "IX_comments_ParentId",
        "comments",
        "ParentId");

      migrationBuilder.CreateIndex(
        "IX_comments_ResourceKey",
        "comments",
        "ResourceKey");

      migrationBuilder.CreateIndex(
        "IX_reactions_AccountId",
        "reactions",
        "AccountId");

      migrationBuilder.CreateIndex(
        "IX_reactions_ResourceKey",
        "reactions",
        "ResourceKey");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "reactions");

      migrationBuilder.DropTable(
        "comments");

      migrationBuilder.DropTable(
        "accounts");

      migrationBuilder.DropTable(
        "resources");
    }
  }
}