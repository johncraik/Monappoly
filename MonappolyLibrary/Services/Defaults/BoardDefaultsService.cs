using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;
using MonappolyLibrary.Data.Defaults.Dictionaries;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Boards;
using MonappolyLibrary.GameModels.Boards.Spaces;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.MiscGameObjs;

namespace MonappolyLibrary.Services.Defaults;

public class BoardDefaultsService
{
    private readonly MonappolyDbContext _context;
    private readonly CsvReader<GenericSpaceUpload> _genericUpload;
    private readonly CsvReader<TaxSpaceUpload> _taxUpload;
    private readonly CsvReader<CardSpaceUpload> _cardUpload;
    private readonly CsvReader<PropertySpaceUpload> _propUpload;

    public BoardDefaultsService(MonappolyDbContext context)
    {
        _context = context;
        _genericUpload = new CsvReader<GenericSpaceUpload>();
        _taxUpload = new CsvReader<TaxSpaceUpload>();
        _cardUpload = new CsvReader<CardSpaceUpload>();
        _propUpload = new CsvReader<PropertySpaceUpload>();
    }

    public async Task EnsureBoardDefaults()
    {
        var gid = await CreateBuildings();
        var bid = await EnsureBoardCreated(gid);
        await EnsureBoardSpaces(bid);
    }

    private async Task<int> CreateBuildings()
    {
        var group = await _context.BuildingGroups.MonopolyDefaults().ToListAsync();
        var groupId = 0;
        if (group.Count != 1)
        {
            foreach (var g in group)
            {
                g.ForceDelete();
            }
            await _context.SaveChangesAsync();
            
            var buildingGroup = new BuildingGroup
            {
                TenantId = DefaultsDictionary.MonopTenant,
                Name = BoardDefaultsDictionary.BuildingGroupName,
                Description = "Standard Monopoly Buildings",
                IsDeleted = false
            };
            buildingGroup.FillCreated();
            await _context.BuildingGroups.AddAsync(buildingGroup);
            await _context.SaveChangesAsync();
            
            groupId = buildingGroup.Id;
        }
        else
        {
            groupId = group.FirstOrDefault()?.Id ?? throw new Exception("Building group not found");
        }
        
        var pools = await _context.BuildingPools.MonopolyDefaults().ToListAsync();
        var housePoolId = 0;
        var hotelPoolId = 0;
        if (pools.Count != 2)
        {
            foreach (var pool in pools)
            {
                pool.ForceDelete();
            }
            await _context.SaveChangesAsync();

            var housePool = new BuildingPool
            {
                TenantId = DefaultsDictionary.MonopTenant,
                Name = BoardDefaultsDictionary.HouseBuildingPoolName,
                BuildingGroupId = groupId,
                Count = 32
            };
            housePool.FillCreated();
            await _context.BuildingPools.AddAsync(housePool);
            
            var hotelPool = new BuildingPool
            {
                TenantId = DefaultsDictionary.MonopTenant,
                Name = BoardDefaultsDictionary.HotelBuildingPoolName,
                BuildingGroupId = groupId,
                Count = 12
            };
            hotelPool.FillCreated();
            await _context.BuildingPools.AddAsync(hotelPool);
            
            await _context.SaveChangesAsync();
            housePoolId = housePool.Id;
            hotelPoolId = hotelPool.Id;
        }
        else
        {
            housePoolId = pools.FirstOrDefault(p => p.Name == BoardDefaultsDictionary.HouseBuildingPoolName)?.Id
                ?? throw new Exception("House building pool not found");
            hotelPoolId = pools.FirstOrDefault(p => p.Name == BoardDefaultsDictionary.HotelBuildingPoolName)?.Id
                ?? throw new Exception("Hotel building pool not found");
        }

        var buildings = await _context.Buildings.MonopolyDefaults().ToListAsync();
        if (buildings.Count != 2)
        {
            foreach (var building in buildings)
            {
                building.ForceDelete();
            }
            await _context.SaveChangesAsync();

            await CreateBuilding(true, housePoolId);
            await CreateBuilding(false, hotelPoolId);
        }

        return groupId;
    }
    
    private async Task CreateBuilding(bool isHouse, int poolId)
    {
        var building = new BuildingDataModel
        {
            TenantId = DefaultsDictionary.MonopTenant,
            Name = isHouse ? "House" : "Hotel",
            BuildingRule = BuildingRule.Standard,
            BuildOnRule = BuildOnRule.Standard,
            RentRule = RentRule.Standard,
            RentMultiplier = 0,
            BuildingPoolId = poolId,
            BuildingCostMultiplier = 1,
            CapType = BuildingCap.None,
        };
        building.FillCreated();
        await _context.Buildings.AddAsync(building);
        await _context.SaveChangesAsync();
    }

    private async Task<int> EnsureBoardCreated(int gid)
    {
        var board = await _context.Boards.MonopolyDefaults().ToListAsync();
        var boardId = 0;
        if(board.Count != 1)
        {
            foreach (var b in board)
            {
                b.ForceDelete();
            }
            await _context.SaveChangesAsync();

            var newBoard = new Board
            {
                TenantId = DefaultsDictionary.MonopTenant,
                Name = BoardDefaultsDictionary.StandardBoard,
                Description = "Standard Monopoly Board",
                BuildingGroupId = gid,
                IsDeleted = false
            };
            newBoard.FillCreated();
            await _context.Boards.AddAsync(newBoard);
            await _context.SaveChangesAsync();

            boardId = newBoard.Id;
        }
        else
        {
            boardId = board.FirstOrDefault()?.Id ?? throw new Exception("Board not found");
        }

        return boardId;
    }

    private async Task EnsureBoardSpaces(int bid)
    {
        var genericSpaces = await _context.GenericBoardSpaces.MonopolyDefaults().ToListAsync();
        if (genericSpaces.Count != 4)
        {
            foreach (var gs in genericSpaces)
            {
                gs.ForceDelete();
            }
            await _context.SaveChangesAsync();

            await CreateGenericSpaces(bid);
        }
        
        var taxSpaces = await _context.TaxBoardSpaces.MonopolyDefaults().ToListAsync();
        if (taxSpaces.Count != 2)
        {
            foreach (var t in taxSpaces)
            {
                t.ForceDelete();
            }
            await _context.SaveChangesAsync();

            await CreateTaxSpaces(bid);
        }
        
        var cardSpaces = await _context.CardBoardSpaces.MonopolyDefaults().ToListAsync();
        if (cardSpaces.Count != 6)
        {
            foreach (var c in cardSpaces)
            {
                c.ForceDelete();
            }
            await _context.SaveChangesAsync();

            await CreateCardSpaces(bid);
        }
        
        var propSpaces = await _context.PropertyBoardSpaces.MonopolyDefaults().ToListAsync();
        if (propSpaces.Count != 28)
        {
            foreach (var p in propSpaces)
            {
                p.ForceDelete();
            }
            await _context.SaveChangesAsync();

            await CreatePropertySpaces(bid);
        }
    }

    private async Task CreateGenericSpaces(int bid)
    {
        var file = File.OpenRead($"{DefaultsService.DefaultsPath}GenericSpaces.csv");
        var records = _genericUpload.UploadFile(file);
        if(records == null) throw new Exception("Failed to read GenericSpaces.csv");

        var spaces = new List<GenericBoardSpace>();
        foreach (var r in records)
        {
            var space = new GenericBoardSpace
            {
                TenantId = DefaultsDictionary.MonopTenant,
                BoardId = bid,
                BoardIndex = (uint)r.Index,
                Name = r.Name,
                SpaceType = r.Type,
                Action = r.Action,
                IsDeleted = false
            };
            space.FillCreated();
            spaces.Add(space);
        }

        await _context.GenericBoardSpaces.AddRangeAsync(spaces);
        await _context.SaveChangesAsync();
    }
    
    private async Task CreateTaxSpaces(int bid)
    {
        var file = File.OpenRead($"{DefaultsService.DefaultsPath}TaxSpaces.csv");
        var records = _taxUpload.UploadFile(file);
        if(records == null) throw new Exception("Failed to read TaxSpaces.csv");

        var spaces = new List<TaxBoardSpace>();
        foreach (var r in records)
        {
            var space = new TaxBoardSpace
            {
                TenantId = DefaultsDictionary.MonopTenant,
                BoardId = bid,
                BoardIndex = (uint)r.Index,
                Name = r.Name,
                SpaceType = r.Type,
                TaxAmount = r.Tax,
                IsDeleted = false
            };
            space.FillCreated();
            spaces.Add(space);
        }

        await _context.TaxBoardSpaces.AddRangeAsync(spaces);
        await _context.SaveChangesAsync();
    }
    
    private async Task CreateCardSpaces(int bid)
    {
        var chanceType = await _context.CardTypes.MonopolyDefaults()
            .FirstOrDefaultAsync(t => t.Name == CardDefaultsDictionary.Chance);
        var comType = await _context.CardTypes.MonopolyDefaults()
            .FirstOrDefaultAsync(t => t.Name == CardDefaultsDictionary.ComChest);
        if(chanceType == null || comType == null)
            throw new Exception("Card types not found");
        
        var file = File.OpenRead($"{DefaultsService.DefaultsPath}CardSpaces.csv");
        var records = _cardUpload.UploadFile(file);
        if(records == null) throw new Exception("Failed to read CardSpaces.csv");

        var spaces = new List<CardBoardSpace>();
        foreach (var r in records)
        {
            var space = new CardBoardSpace
            {
                TenantId = DefaultsDictionary.MonopTenant,
                BoardId = bid,
                BoardIndex = (uint)r.Index,
                Name = r.Name.Contains("Com") ? comType.Name : chanceType.Name,
                SpaceType = r.Type,
                CardTypeId = r.Name.Contains("Com") ? comType.Id : chanceType.Id,
                IsDeleted = false
            };
            space.FillCreated();
            spaces.Add(space);
        }

        await _context.CardBoardSpaces.AddRangeAsync(spaces);
        await _context.SaveChangesAsync();
    }
    
    private async Task CreatePropertySpaces(int bid)
    {
        var file = File.OpenRead($"{DefaultsService.DefaultsPath}PropertySpaces.csv");
        var records = _propUpload.UploadFile(file);
        if(records == null) throw new Exception("Failed to read PropertySpaces.csv");

        var spaces = new List<PropertyBoardSpace>();
        foreach (var r in records)
        {
            var space = new PropertyBoardSpace
            {
                TenantId = DefaultsDictionary.MonopTenant,
                BoardId = bid,
                BoardIndex = (uint)r.Index,
                Name = r.Name,
                SpaceType = r.Type,
                PropertyType = r.PropertyType,
                PropertySet = r.PropertySet,
                Cost = r.Cost,
                IsDeleted = false
            };
            space.FillCreated();
            spaces.Add(space);
        }

        await _context.PropertyBoardSpaces.AddRangeAsync(spaces);
        await _context.SaveChangesAsync();
    }
}