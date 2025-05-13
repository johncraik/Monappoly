using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;
using MonappolyLibrary.Data.Defaults.Dictionaries;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards;

namespace MonappolyLibrary.Services.Defaults;

public class CardDefaultsService
{
    private readonly MonappolyDbContext _context;
    private readonly CsvReader<CardUpload> _csvReader;

    public CardDefaultsService(MonappolyDbContext context,
        CsvReader<CardUpload> csvReader)
    {
        _context = context;
        _csvReader = csvReader;
    }
    
    public class CardUpload
    {
        public string Text { get; set; }
    }
    
    public async Task EnsureCardDefaults()
    {
        await EnsureCardTypes();
        await EnsureCardDeck();
        
        await EnsureCards(CardDefaultsDictionary.Chance);
        await EnsureCards(CardDefaultsDictionary.ComChest);
    }

    private async Task EnsureCardTypes()
    {
        var types = await _context.CardTypes.MonopolyDefaults()
            .ToListAsync();

        if (types.Count != 2)
        {
            foreach (var type in types)
            {
                type.ForceDelete();
            }
            await _context.SaveChangesAsync();
            
           await CreateType(CardDefaultsDictionary.Chance, "Monopoly Chance Cards", CardDefaultsDictionary.ChanceColour, CardTypeCondition.Chance);
           await CreateType(CardDefaultsDictionary.ComChest, "Monopoly Community Chest Cards", CardDefaultsDictionary.ComChestColour, CardTypeCondition.CommunityChest);
        }
    }

    private async Task CreateType(string name, string desc, string col, CardTypeCondition con)
    {
        var chance = new CardType
        {
            TenantId = DefaultsDictionary.MonopTenant,
            Name = name,
            Colour = col,
            Description = desc,
            Rule = CardTypeRule.Default,
            Condition = con,
            IsDeleted = false
        };
        
        chance.FillCreated();
        await _context.CardTypes.AddAsync(chance);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureCardDeck()
    {
        var deck = await _context.CardDecks.MonopolyDefaults()
            .ToListAsync();

        if (deck.Count != 1)
        {
            foreach (var d in deck)
            {
                d.ForceDelete();
            }
            await _context.SaveChangesAsync();
            
            var cardDeck = new CardDeck
            {
                TenantId = DefaultsDictionary.MonopTenant,
                Name = CardDefaultsDictionary.StandardDeck,
                Description = "Default Deck for Monopoly",
                Difficulty = CardDeckDifficulty.Easy,
                IsDeleted = false
            };
            cardDeck.FillCreated();
            await _context.CardDecks.AddAsync(cardDeck);
            await _context.SaveChangesAsync();
        }
    }

    private async Task EnsureCards(string typeName)
    {
        var type = await _context.CardTypes.MonopolyDefaults()
            .FirstOrDefaultAsync(t => t.Name == typeName);
        var deck = await _context.CardDecks.MonopolyDefaults()
            .FirstOrDefaultAsync(d => d.Name == CardDefaultsDictionary.StandardDeck);
        if(type == null || deck == null) throw new Exception("Card Type or Deck not found");
        
        var existingCards = await _context.Cards.MonopolyDefaults()
            .Where(c => c.CardTypeId == type.Id && c.CardDeckId == deck.Id)
            .ToListAsync();

        if (existingCards.Count != 16)
        {
            foreach (var card in existingCards)
            {
                card.ForceDelete();
            }
            await _context.SaveChangesAsync();
            
            var file = File.OpenRead($"{DefaultsService.DefaultsPath}{typeName}.csv");
            var records = _csvReader.UploadFile(file);
            if(records == null) throw new Exception("No records found in file");
            
            var cards = new List<Card>();
            foreach (var r in records)
            {
                var card = new Card
                {
                    Text = r.Text,
                    TenantId = DefaultsDictionary.MonopTenant,
                    CardTypeId = type.Id,
                    CardDeckId = deck.Id,
                    IsDeleted = false
                };
                
                card.FillCreated();
                cards.Add(card);
            }
            await _context.Cards.AddRangeAsync(cards);
            await _context.SaveChangesAsync();
        }
    }
}