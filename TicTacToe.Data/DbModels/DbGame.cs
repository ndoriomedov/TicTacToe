using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Data.DbModels
{
  public class DbGame
  {
    public Guid Id { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? WinnerId { get; set; }

    public ICollection<DbPlayer> Players { get; set; }
    public ICollection<DbMove> Moves { get; set; }

    public DbGame()
    {
      Players = new HashSet<DbPlayer>();
      Moves = new HashSet<DbMove>();
    }
  }

  public class DbGameConfiguration : IEntityTypeConfiguration<DbGame>
  {
    public void Configure(EntityTypeBuilder<DbGame> builder)
    {
      builder.ToTable("Games");

      builder.HasKey(g => g.Id);

      builder
        .HasMany(g => g.Players)
        .WithMany(p => p.Games);

      builder
        .HasMany(g => g.Moves)
        .WithOne(m => m.Game);
    }
  }
}
