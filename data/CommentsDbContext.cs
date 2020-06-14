using Comments.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comments.Data
{
  public class CommentsDbContext : DbContext
  {
    public CommentsDbContext(DbContextOptions<CommentsDbContext> options)
      : base(options)
    {
    }
    
    public DbSet<Author> Authors { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Provider> Providers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Provider>(provider =>
      {
        provider.HasKey(x => x.Id);
        provider.Property(x => x.Id).ValueGeneratedOnAdd();
        provider.HasIndex(x => x.Name).IsUnique();
        provider.Property(x => x.Name).IsRequired().HasMaxLength(50);
        provider.Property(x => x.Tokens).HasColumnType("jsonb").IsRequired();
      });

      modelBuilder.Entity<Author>(author =>
      {
        author.HasKey(x => x.Id);
        author.Property(x => x.Id).ValueGeneratedOnAdd();
        author.Property(x => x.Name).IsRequired().HasMaxLength(50);
        author.HasIndex(x => new { x.Name, x.ProviderId }).IsUnique();
        author.Property(x => x.AvatarUrl).IsRequired(false).HasMaxLength(1000);

        author.HasOne(x => x.Provider)
          .WithMany()
          .HasForeignKey(x => x.ProviderId);
      });

      modelBuilder.Entity<Comment>(comment =>
      {
        comment.HasKey(x => x.Id);
        comment.Property(x => x.Id).ValueGeneratedOnAdd();

        comment
          .HasMany(x => x.SubComments)
          .WithOne(x => x.Parent)
          .HasForeignKey(x => x.ParentId);

        comment
          .HasOne(x => x.Author)
          .WithMany()
          .HasForeignKey(x => x.AuthorId);

        comment
          .HasOne(x => x.Provider)
          .WithMany()
          .HasForeignKey(x => x.ProviderId);

        comment.Property(x => x.ResourceKey).HasMaxLength(500).IsRequired();
        comment.Property(x => x.Message).IsRequired().HasColumnType("text");
        comment.Property(x => x.Deleted).HasDefaultValue(false);
        comment.Property(x => x.RepliesAmount).HasDefaultValue(0);
        comment.Property(x => x.LikesAmount).HasDefaultValue(0);
        comment.Property(x => x.DislikesAmount).HasDefaultValue(0);
      });

      modelBuilder.Entity<Like>(like =>
      {
        like.HasKey(x => new {x.CommentId, x.AuthorId});
        like.Property(x => x.Value).IsRequired();
        like.HasOne(x => x.Author)
          .WithMany()
          .HasForeignKey(x => x.AuthorId);

        like.HasOne(x => x.Comment)
          .WithMany()
          .HasForeignKey(x => x.CommentId);
      });
    }
  }
}