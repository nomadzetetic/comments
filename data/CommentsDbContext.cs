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
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Reaction> Reactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Resource>(entity =>
      {
        entity.ToTable("resources");
        entity.HasKey(x => x.ResourceKey);
        entity.Property(x => x.ResourceKey).HasMaxLength(1000).IsRequired();
        entity.Property(x => x.Replies).HasDefaultValue(0);
        entity.Property(x => x.Likes).HasDefaultValue(0);
        entity.Property(x => x.Dislikes).HasDefaultValue(0);
      });

      modelBuilder.Entity<Account>(entity =>
      {
        entity.ToTable("accounts");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.DisplayName).IsRequired().HasMaxLength(50);
        entity.Ignore(x => x.AvatarUrl);
      });

      modelBuilder.Entity<Comment>(entity =>
      {
        entity.ToTable("comments");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity
          .HasMany(x => x.SubComments)
          .WithOne(x => x.Parent)
          .HasForeignKey(x => x.ParentId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity
          .HasOne(x => x.Account)
          .WithMany()
          .HasForeignKey(x => x.AccountId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity
          .HasOne(x => x.Resource)
          .WithMany()
          .HasForeignKey(x => x.ResourceKey)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity.Property(x => x.Message).IsRequired().HasColumnType("text");
        entity.Property(x => x.Replies).HasDefaultValue(0);
        entity.Property(x => x.Likes).HasDefaultValue(0);
        entity.Property(x => x.Dislikes).HasDefaultValue(0);
      });

      modelBuilder.Entity<Reaction>(entity =>
      {
        entity.ToTable("reactions");
        entity.HasKey(x => new {x.CommentId, x.AccountId, x.ResourceKey});
        entity.Property(x => x.Value).IsRequired();
        
        entity
          .HasOne(x => x.Resource)
          .WithMany()
          .HasForeignKey(x => x.ResourceKey)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
        
        entity.HasOne(x => x.Account)
          .WithMany()
          .HasForeignKey(x => x.AccountId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Comment)
          .WithMany()
          .HasForeignKey(x => x.CommentId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
      });
    }
  }
}
