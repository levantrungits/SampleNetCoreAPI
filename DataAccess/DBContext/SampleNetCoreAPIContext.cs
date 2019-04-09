using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DBContext
{
    public class SampleNetCoreAPIContext : DbContext
    {
        public SampleNetCoreAPIContext(DbContextOptions<SampleNetCoreAPIContext> options)
            : base(options) { }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<SampleNetCoreConfig> SampleNetCoreConfig { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleNetCoreConfig>(entity =>
            {
                entity.ToTable("sample_net_core_config");
            });

            modelBuilder.Entity<Blog>(entity => 
            {
                entity.ToTable("blogs");
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");
                entity.HasIndex(e => e.BlogId)
                    .HasName("FK_Post_Blog_BlogId_idx");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_Post_Blog_BlogId");
            });
        }
    }
}
