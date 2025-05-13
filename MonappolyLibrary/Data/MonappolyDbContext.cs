using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.GameModels.Boards;
using MonappolyLibrary.GameModels.Boards.Spaces;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.MiscGameObjs;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.Data;

public class MonappolyDbContext : DbContext
{
    private readonly int _tenantId;
    public MonappolyDbContext(DbContextOptions<MonappolyDbContext> options, UserInfo userInfo)
        : base(options)
    {
        _tenantId = userInfo.TenantId;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply global filter for all types inheriting from DataModel
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(DataModel).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(MonappolyDbContext).GetMethod(nameof(SetGlobalQuery), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(this, new object[] { modelBuilder });

            }
        }
    }
    
    private void SetGlobalQuery<T>(ModelBuilder modelBuilder) where T : DataModel
    {
        modelBuilder.Entity<T>().HasQueryFilter(e =>
            (e.TenantId == _tenantId || e.TenantId < 0) && !e.IsDeleted);
    }


    #region Cards

    public DbSet<Card> Cards { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
    public DbSet<CardDeck> CardDecks { get; set; }
    public DbSet<ActionGroup> CardActionGroups { get; set; }

    #endregion
    
    
    #region Boards
    
    public DbSet<Board> Boards { get; set; }
    public DbSet<GenericBoardSpace> GenericBoardSpaces { get; set; }
    public DbSet<TaxBoardSpace> TaxBoardSpaces { get; set; }
    public DbSet<CardBoardSpace> CardBoardSpaces { get; set; }
    public DbSet<PropertyBoardSpace> PropertyBoardSpaces { get; set; }
    
    #endregion

    #region MiscGameObjs

    public DbSet<DiceDataModel> DiceModels { get; set; }
    public DbSet<PlayerPieceDataModel> PlayerPieces { get; set; }
    public DbSet<BuildingGroup> BuildingGroups { get; set; }
    public DbSet<BuildingDataModel> Buildings { get; set; }

    #endregion
}