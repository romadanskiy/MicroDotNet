using BLL.Models;
using BLL.Models.Types;
using DAL;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ReceptionPointService
{
    private readonly ScannerDbContext _dbContext;
    
    public ReceptionPointService(ScannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddReceptionPointAsync(ReceptionPoint receptionPointData)
    {
        if (receptionPointData == null)
        {
            throw new ApplicationException("Не удалось получить данные о пункте приема отходов!");
        }

        var garbageTypes = await _dbContext.GarbageType
            .Where(x => receptionPointData.GarbageTypes.Select(y => y.Id).Contains(x.Id))
            .ToListAsync();
        
        var dbReceptionPoint = new GarbageReceptionPoint()
        {
            Name = receptionPointData.Name,
            Address = receptionPointData.Address.FullAddress,
            Latitude = receptionPointData.Address.Latitude,
            Longitude = receptionPointData.Address.Longitude,
            Description = receptionPointData.Description,
            GarbageTypes = garbageTypes
        };
        _dbContext.GarbageReceptionPoint.Add(dbReceptionPoint);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<ReceptionPoint>> GetReceptionPointsAsync(IEnumerable<long> garbageTypeIds)
    {
        if (garbageTypeIds == null || !garbageTypeIds.Any())
        {
            return await _dbContext.GarbageReceptionPoint.Select(x => new ReceptionPoint(x.Name, x.Description,
                new Address(x.Address, x.Longitude, x.Latitude),
                x.GarbageTypes.Select(y => new DTO.GarbageType(y.Id, y.Name)).ToList(), x.Id)).ToListAsync();
        }
        
        var receptionPoints =  await _dbContext.GarbageType
            .Where(x=> garbageTypeIds.Contains(x.Id))
            .Include(x=>x.GarbageReceptionPoints)
            .SelectMany(x=>x.GarbageReceptionPoints)
            .Select(x => new ReceptionPoint(
                x.Name, 
                x.Description,
                new Address(x.Address, x.Longitude, x.Latitude),
                x.GarbageTypes.Select(y => new DTO.GarbageType(y.Id, y.Name)).ToList(),
                x.Id))
            .ToListAsync();

        return receptionPoints.DistinctBy(x => x.Id).ToList();
    }
}