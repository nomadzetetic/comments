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
    public DbSet<Reaction> Likes { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Resource>(entity =>
      {
        entity.HasKey(x => x.ResourceId);
        entity.Property(x => x.ResourceId).HasMaxLength(1000).IsRequired();
        entity.Property(x => x.Replies).HasDefaultValue(0);
        entity.Property(x => x.Likes).HasDefaultValue(0);
        entity.Property(x => x.Dislikes).HasDefaultValue(0);
        entity.HasOne(x => x.Tenant)
          .WithMany()
          .HasForeignKey(x => x.TenantId)
          .IsRequired();
        entity.HasIndex(x => new { x.ResourceId, x.TenantId }).IsUnique();
      });
      
      modelBuilder.Entity<Tenant>(entity =>
      {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();
        entity.HasIndex(x => x.Name).IsUnique();
        entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
        entity.Property(x => x.Tokens).HasColumnType("jsonb").IsRequired();
      });

      modelBuilder.Entity<Account>(entity =>
      {
        entity.HasKey(x => new { x.Id, x.TenantId });
        entity.Property(x => x.Id).IsRequired();
        entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
        entity.HasOne(x => x.Tenant)
          .WithMany()
          .HasForeignKey(x => x.TenantId)
          .IsRequired();

        entity.Ignore(x => x.AvatarUrl);
      });

      modelBuilder.Entity<Comment>(entity =>
      {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity
          .HasMany(x => x.SubComments)
          .WithOne(x => x.Parent)
          .HasForeignKey(x => x.ParentId)
          .OnDelete(DeleteBehavior.Cascade);

        entity
          .HasOne(x => x.Account)
          .WithMany()
          .HasForeignKey(x => x.AccountId);

        entity
          .HasOne(x => x.Tenant)
          .WithMany()
          .HasForeignKey(x => x.TenantId);

        entity
          .HasOne(x => x.Resource)
          .WithMany()
          .HasForeignKey(x => x.ResourceId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        entity.Property(x => x.Message).IsRequired().HasColumnType("text");
        entity.Property(x => x.Replies).HasDefaultValue(0);
        entity.Property(x => x.Likes).HasDefaultValue(0);
        entity.Property(x => x.Dislikes).HasDefaultValue(0);
      });

      modelBuilder.Entity<Reaction>(entity =>
      {
        entity.HasKey(x => new {x.CommentId, AuthorId = x.AccountId});
        entity.Property(x => x.Value).IsRequired();
        entity.HasOne(x => x.Account)
          .WithMany()
          .HasForeignKey(x => x.AccountId);

        entity.HasOne(x => x.Comment)
          .WithMany()
          .HasForeignKey(x => x.CommentId);
      });
    }
  }
}