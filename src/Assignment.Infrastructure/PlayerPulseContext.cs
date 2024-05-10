using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModels;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Api.Models.PlayerPulseModel;

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

    public virtual DbSet<PlayerAuction> PlayerAuctions { get; set; }

    public virtual DbSet<PlayerPulseAction> PlayerPulseActions { get; set; }

    public virtual DbSet<PlayerPulsePermission> PlayerPulsePermissions { get; set; }

    public virtual DbSet<PlayerPulseRole> PlayerPulseRoles { get; set; }

    public virtual DbSet<PlayerPulseRoleActionPermission> PlayerPulseRoleActionPermissions { get; set; }

    public virtual DbSet<PlayerPulseUser> PlayerPulseUsers { get; set; }

    public virtual DbSet<PlayerSport> PlayerSports { get; set; }

    public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }

    public virtual DbSet<PlayerValuation> PlayerValuations { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<SportStatistic> SportStatistics { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

    public virtual DbSet<TeamUser> TeamUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JEEVAN-LAP\\SQLEXPRESS01;Database=PlayerPulse;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auction__3214EC0717C69EE4");

            entity.ToTable("Auction");

            entity.Property(e => e.BiddingMechanism)
                .IsRequired()
                .HasConversion<string>()
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
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Sport).WithMany(p => p.Auctions)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("FK_Auction_Sport");
        });

        modelBuilder.Entity<AuctionBid>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuctionB__3214EC0741C28946");

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
                .HasConstraintName("FK__AuctionBi__Aucti__44CA3770");

            entity.HasOne(d => d.Player).WithMany(p => p.AuctionBids)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionBi__Playe__45BE5BA9");

            entity.HasOne(d => d.Team).WithMany(p => p.AuctionBids)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionBi__TeamI__46B27FE2");
        });

        modelBuilder.Entity<AuctionRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuctionR__3214EC0734FA14CA");

            entity.ToTable("AuctionRule");

            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.RuleValue).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Auction).WithMany(p => p.AuctionRules)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionRu__Aucti__12FDD1B2");

            entity.HasOne(d => d.Rule).WithMany(p => p.AuctionRules)
                .HasForeignKey(d => d.RuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionRu__RuleI__13F1F5EB");
        });

        modelBuilder.Entity<AuctionTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuctionT__3214EC071C45AFE4");

            entity.ToTable("AuctionTeam");

            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.BalanceAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BudgetAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RegistrationTime).HasColumnType("datetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Auction).WithMany(p => p.AuctionTeams)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionTe__Aucti__40F9A68C");

            entity.HasOne(d => d.Team).WithMany(p => p.AuctionTeams)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuctionTe__TeamI__41EDCAC5");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Level__3214EC07E627896C");

            entity.ToTable("Level");

            entity.HasIndex(e => e.Name, "UQ__Level__737584F6A6983084").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Player__3214EC07494EE0BE");

            entity.ToTable("Player");

            entity.HasIndex(e => e.PlayerCode, "IX_Player_PlayerCode");

            entity.HasIndex(e => e.Mobile, "UQ__Player__6FAE07829AC0597F").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Player__A9D10534CED5848F").IsUnique();

            entity.HasIndex(e => e.PlayerCode, "UQ__Player__EBD233B22193FAD1").IsUnique();

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
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
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

            entity.HasOne(d => d.User).WithMany(p => p.Players)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Player__UserId__681373AD");
        });

        modelBuilder.Entity<PlayerAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerAu__3214EC07F647B65D");

            entity.ToTable("PlayerAuction");

            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ValuatedPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Auction).WithMany(p => p.PlayerAuctions)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerAuc__Aucti__17C286CF");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerAuctions)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerAuc__Playe__16CE6296");
        });

        modelBuilder.Entity<PlayerPulseAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Action__3214EC079E03DF6C");

            entity.ToTable("PlayerPulseAction");

            entity.Property(e => e.ActionName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<PlayerPulsePermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07790FB5D4");

            entity.ToTable("PlayerPulsePermission");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.PermissionName)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<PlayerPulseRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0737545920");

            entity.ToTable("PlayerPulseRole");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<PlayerPulseRoleActionPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleActi__3214EC07E7DE419C");

            entity.ToTable("PlayerPulseRoleActionPermission");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Action).WithMany(p => p.PlayerPulseRoleActionPermissions)
                .HasForeignKey(d => d.ActionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoleActio__Actio__65370702");

            entity.HasOne(d => d.Permission).WithMany(p => p.PlayerPulseRoleActionPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoleActio__Permi__662B2B3B");

            entity.HasOne(d => d.Role).WithMany(p => p.PlayerPulseRoleActionPermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoleActio__RoleI__6442E2C9");
        });

        modelBuilder.Entity<PlayerPulseUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07A0F0FF2D");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534BBC2F0B4").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Otp).HasColumnName("OTP");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.PlayerPulseUsers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleId__2B0A656D");
        });

        modelBuilder.Entity<PlayerSport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerSp__3214EC0777B24229");

            entity.ToTable("PlayerSport");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.SportId).HasColumnName("SportID");

            entity.HasOne(d => d.Level).WithMany(p => p.PlayerSports)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSpo__Level__00DF2177");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerSports)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSpo__Playe__7EF6D905");

            entity.HasOne(d => d.Sport).WithMany(p => p.PlayerSports)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSpo__Sport__7FEAFD3E");
        });

        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerSt__3214EC0723162B45");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerStatistics)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSta__Playe__0B5CAFEA");

            entity.HasOne(d => d.StatisticType).WithMany(p => p.PlayerStatistics)
                .HasForeignKey(d => d.StatisticTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerSta__Stati__0C50D423");
        });

        modelBuilder.Entity<PlayerValuation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerVa__3214EC0790E86576");

            entity.ToTable("PlayerValuation");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.ValuationPoints).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerValuations)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayerVal__Playe__32AB8735");
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rules__3214EC07E4F2FE73");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RuleType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sport__3214EC077EFF4D6F");

            entity.ToTable("Sport");

            entity.HasIndex(e => e.Name, "UQ__Sport__737584F6CD10D9CC").IsUnique();

            entity.HasIndex(e => e.SportCode, "UQ__Sport__BF4C8B801B9EF6D5").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SportCode)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SportStatistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SportSta__3214EC07D6CF08F8");

            entity.ToTable("SportStatistic");

            entity.Property(e => e.SportId).HasColumnName("SportID");
            entity.Property(e => e.StatisticType)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Sport).WithMany(p => p.SportStatistics)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SportStat__Sport__03BB8E22");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC07B5B8CCF7");

            entity.ToTable("Team");

            entity.HasIndex(e => e.TeamCode, "UQ__Team__55013508393A6DE2").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TeamCode)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamPlay__3214EC07E9FEB040");

            entity.ToTable("TeamPlayer");

            entity.Property(e => e.ContractEndDate).HasColumnType("datetime");
            entity.Property(e => e.ContractStartDate).HasColumnType("datetime");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.PurchasedAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Player).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamPlaye__Playe__59C55456");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamPlaye__TeamI__58D1301D");
        });

        modelBuilder.Entity<TeamUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamUser__3214EC07964AC096");

            entity.ToTable("TeamUser");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamUser__TeamID__3864608B");

            entity.HasOne(d => d.User).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamUser__UserID__395884C4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
