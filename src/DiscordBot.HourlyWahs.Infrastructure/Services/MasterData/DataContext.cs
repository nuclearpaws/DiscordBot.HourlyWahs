using DiscordBot.HourlyWahs.Infrastructure.Services.MasterData.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.MasterData
{
    internal class MasterDataContext : DbContext
    {
        public DbSet<Server> Servers { get; private set; }
        public DbSet<TargetChannel> TargetChannels { get; private set; }

        public MasterDataContext(DbContextOptions<MasterDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DiscordServer entity mapping:
            var server = modelBuilder.Entity<Server>();

            server
                .ToTable($"{nameof(Server)}s")
                .HasKey(e => e.ServerId);

            server
                .Property(e => e.ServerId)
                .HasColumnName(nameof(Server.ServerId))
                .HasColumnType("TEXT")
                .IsRequired(true);

            server
                .Property(e => e.DateAdded)
                .HasColumnName(nameof(Server.DateAdded))
                .HasColumnType("TEXT")
                .IsRequired(true);

            server
                .HasMany(e => e.TargetChannels)
                .WithOne(e => e.Server)
                .HasForeignKey(e => e.ServerId)
                .OnDelete(DeleteBehavior.Cascade);
            // /// //

            // TargetChannel entity mapping:
            var targetChannel = modelBuilder.Entity<TargetChannel>();

            targetChannel
                .ToTable($"{nameof(TargetChannel)}s")
                .HasKey(e => new { e.ServerId, e.ChannelId });

            targetChannel
                .Property(e => e.ServerId)
                .HasColumnName(nameof(TargetChannel.ServerId))
                .HasColumnType("TEXT")
                .IsRequired(true);

            targetChannel
                .Property(e => e.ChannelId)
                .HasColumnName(nameof(TargetChannel.ChannelId))
                .HasColumnType("TEXT")
                .IsRequired(true);

            targetChannel
                .Property(e => e.DateAdded)
                .HasColumnName(nameof(TargetChannel.DateAdded))
                .HasColumnType("TEXT")
                .IsRequired(true);

            targetChannel
                .HasOne(e => e.Server)
                .WithMany(e => e.TargetChannels)
                .HasForeignKey(e => e.ServerId)
                .OnDelete(DeleteBehavior.NoAction);
            // /// //

            base.OnModelCreating(modelBuilder);
        }
    }
}