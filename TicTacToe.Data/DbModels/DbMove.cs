using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Data.DbModels
{
  public class DbMove
  {
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public byte Row { get; set; }
    public byte Column { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public DbPlayer Player { get; set; }
    public DbGame Game { get; set; }
  }

  public class DbUserConfiguration : IEntityTypeConfiguration<DbMove>
  {
    public void Configure(EntityTypeBuilder<DbMove> builder)
    {
      builder
        .ToTable("Moves");

      builder
        .HasKey(m => m.Id);

      builder
        .HasOne(m => m.Player)
        .WithMany(p => p.Moves);

      builder
        .HasOne(m => m.Game)
        .WithMany(p => p.Moves);
    }
  }
}
