using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.CardActions;

namespace MonappolyLibrary.Data;

public class MonappolyDbContext : DbContext
{
    public MonappolyDbContext(DbContextOptions<MonappolyDbContext> options)
        : base(options)
    {
    }

    #region Cards

    public DbSet<Card> Cards { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
    public DbSet<CardDeck> CardDecks { get; set; }
    public DbSet<ActionGroup> CardActionGroups { get; set; }

    #endregion
}