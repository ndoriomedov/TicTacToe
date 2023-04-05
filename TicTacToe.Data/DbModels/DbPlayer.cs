using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Data.DbModels
{
  public class DbPlayer
  {
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public int Points { get; set; }

    public ICollection<DbGame> Games { get; set; }
    public ICollection<DbMove> Moves { get; set; }

    public DbPlayer()
    {
      Games = new HashSet<DbGame>();
      Moves = new HashSet<DbMove>();
    }
  }

  public class DbPlayerConfiguration : IEntityTypeConfiguration<DbPlayer>
  {
    public void Configure(EntityTypeBuilder<DbPlayer> builder)
    {
      builder
        .ToTable("Players");

      builder
        .HasKey(p => p.Id);

      builder
        .HasIndex(p => p.Login)
        .IsUnique();

      builder
        .Property(p => p.Login)
        .HasMaxLength(100)
        .IsRequired();

      builder
        .Property(p => p.PasswordHash)
        .IsRequired();

      builder
        .Property(p => p.Points)
        .HasDefaultValue(0);

      builder
        .HasMany(p => p.Games)
        .WithMany(g => g.Players);

      builder
        .HasMany(p => p.Moves)
        .WithOne(m => m.Player);
    }
  }
}
