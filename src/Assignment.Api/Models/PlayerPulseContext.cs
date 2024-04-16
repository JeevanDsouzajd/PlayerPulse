using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Api.Models;

public partial class PlayerPulseContext : DbContext
{
    public PlayerPulseContext()
    {
    }

    public PlayerPulseContext(DbContextOptions<PlayerPulseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auction> Auctions { get; set; }

    public virtual DbSet<AuctionBid> AuctionBids { get; set; }

    public virtual DbSet<AuctionRule> AuctionRules { get; set; }

    public virtual DbSet<AuctionTeam> AuctionTeams { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerAuctionStatus> PlayerAuctionStatuses { get; set; }

    public virtual DbSet<PlayerSport> PlayerSports { get; set; }

    public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }

    public virtual DbSet<PlayerValuation> PlayerValuations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

    public virtual DbSet<TeamUser> TeamUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JEEVAN-LAP\\SQLEXPRESS01;Database=PlayerPulse;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auction__3214EC07E4018ED8");

            entity.ToTable("Auction");

            entity.Property(e => e.BiddingMechanism)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.League)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationEndTime).HasColumnType("datetime");
            entity.Property(e => e.RegistrationStartTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuctionBid>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuctionB__3214EC0758F832D9");

            entity.ToTable("AuctionBid");

            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.BidAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BidTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Auction).WithMany(p => p.AuctionBids)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionBi__Aucti__5441852A");

            entity.HasOne(d => d.Player).WithMany(p => p.AuctionBids)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionBi__Playe__5535A963");

            entity.HasOne(d => d.Team).WithMany(p => p.AuctionBids)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionBi__TeamI__5629CD9C");
        });

        modelBuilder.Entity<AuctionRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuctionR__3214EC07378734E1");

            entity.ToTable("AuctionRule");

            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.RuleType)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RuleValue).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Auction).WithMany(p => p.AuctionRules)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionRu__Aucti__4D94879B");
        });

        modelBuilder.Entity<AuctionTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuctionT__3214EC076DED254D");

            entity.ToTable("AuctionTeam");

            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.RegistrationTime).HasColumnType("datetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Auction).WithMany(p => p.AuctionTeams)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionTe__Aucti__5070F446");

            entity.HasOne(d => d.Team).WithMany(p => p.AuctionTeams)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionTe__TeamI__5165187F");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Level__3214EC0741DDC9B4");

            entity.ToTable("Level");

            entity.HasIndex(e => e.Name, "UQ__Level__737584F622E6DF2F").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Player__3214EC074234AC68");

            entity.ToTable("Player");

            entity.HasIndex(e => e.Mobile, "UQ__Player__6FAE0782D7C52CA2").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Player__A9D10534F3C0B440").IsUnique();

            entity.HasIndex(e => e.PlayerCode, "UQ__Player__EBD233B23786A989").IsUnique();

            entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PlayerCode)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VerifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PlayerAuctionStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerAu__3214EC071D181C57");

            entity.ToTable("PlayerAuctionStatus");

            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.ValuatedPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Auction).WithMany(p => p.PlayerAuctionStatuses)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerAuc__Aucti__6477ECF3");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerAuctionStatuses)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerAuc__Playe__6383C8BA");

            entity.HasOne(d => d.Team).WithMany(p => p.PlayerAuctionStatuses)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerAuc__TeamI__656C112C");
        });

        modelBuilder.Entity<PlayerSport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerSp__3214EC07AF274B90");

            entity.ToTable("PlayerSport");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.SportId).HasColumnName("SportID");

            entity.HasOne(d => d.Level).WithMany(p => p.PlayerSports)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSpo__Level__60A75C0F");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerSports)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSpo__Playe__5EBF139D");

            entity.HasOne(d => d.Sport).WithMany(p => p.PlayerSports)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSpo__Sport__5FB337D6");
        });

        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerSt__3214EC07B77A25E9");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.SportId).HasColumnName("SportID");
            entity.Property(e => e.StatisticType)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerStatistics)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSta__Playe__6C190EBB");

            entity.HasOne(d => d.Sport).WithMany(p => p.PlayerStatistics)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSta__Sport__6D0D32F4");
        });

        modelBuilder.Entity<PlayerValuation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerVa__3214EC074577B039");

            entity.ToTable("PlayerValuation");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.ValuationPoints).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerValuations)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerVal__Playe__4222D4EF");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0740AC9397");

            entity.ToTable("Role");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sport__3214EC07C472B38C");

            entity.ToTable("Sport");

            entity.HasIndex(e => e.Name, "UQ__Sport__737584F6BE575AD3").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC0725C4FD86");

            entity.ToTable("Team");

            entity.HasIndex(e => e.TeamCode, "UQ__Team__550135081EE7DA7D").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TeamCode)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamPlay__3214EC07F5BA6F5B");

            entity.ToTable("TeamPlayer");

            entity.Property(e => e.ContractEndDate).HasColumnType("datetime");
            entity.Property(e => e.ContractStartDate).HasColumnType("datetime");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PurchasedAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Player).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamPlaye__Playe__693CA210");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamPlaye__TeamI__68487DD7");
        });

        modelBuilder.Entity<TeamUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamUser__3214EC073515A571");

            entity.ToTable("TeamUser");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamUser__TeamID__47DBAE45");

            entity.HasOne(d => d.User).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamUser__UserID__48CFD27E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC076A0DCFFD");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342269C381").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleId__3A81B327");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
