using BLL.DTO;
using BLL.Models;
using DAL;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using GarbageType = DAL.Entity.GarbageType;

namespace BLL.Services;

/// <summary>
/// Сервис для работы с данными об отходах
/// </summary>
public class GarbageService
{
    private readonly ScannerDbContext _dbContext;

    public GarbageService(ScannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Сохранить в бд данные о новом товаре
    /// </summary>
    public async Task AddGarbageInformationAsync(GarbageInformation info, long? userId = null)
    {
        if (!await IsBarcodeAlreadyExistAsync(info.Barcode))
        {
            var garbageTypes = await _dbContext.GarbageType
                .Where(x => info.GarbageTypes.Select(x => x.Id).Contains(x.Id)).ToListAsync();

            if (garbageTypes.Count != info.GarbageTypes.Count())
            {
                throw new ApplicationException("Категории отходов указаны неверно!");
            }

            var newGarbageInfo = new Garbage()
            {
                Picture = info.ImagePath,
                Name = info.Name,
                Description = info.Description,
                Barcode = info.Barcode,
                GarbageTypes = garbageTypes
            };

            _dbContext.Garbage.Add(newGarbageInfo);
            
            if (userId.HasValue)
            {
                var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);
            
                if (user == null)
                {
                    throw new ApplicationException("Не найден пользователь, выполнивший запрос!");
                }

                _dbContext.UserGarbage.Add(new UserGarbage()
                {
                    User = user,
                    Garbage = newGarbageInfo,
                    ScannedDateTime = DateTime.UtcNow
                });
            }
            
            await _dbContext.SaveChangesAsync();

            return;
        }

        throw new ApplicationException("Данный штрихкод уже зарегистрирован в системе!");
    }

    /// <summary>
    /// Получить информацию о товаре
    /// </summary>
    public async Task<GarbageInformation> GetInformationByBarcodeAsync(string barcode, long? userId = null)
    {
        if (string.IsNullOrEmpty(barcode))
        {
            throw new ApplicationException("Не указан штрихкод!");
        }

        var garbageInfo = await _dbContext.Garbage.Where(x => x.Barcode == barcode).Include(x => x.GarbageTypes)
            .FirstOrDefaultAsync();

        if (garbageInfo == null)
        {
            throw new ApplicationException("По данному штрихкоду не найдено информации!");
        }

        if (userId.HasValue)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);
            
            if (user == null)
            {
                throw new ApplicationException("Не найден пользователь, выполнивший запрос!");
            }

            _dbContext.UserGarbage.Add(new UserGarbage()
            {
                User = user,
                Garbage = garbageInfo,
                ScannedDateTime = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();
        }

        return new GarbageInformation(garbageInfo.Picture, garbageInfo.Name, garbageInfo.Description,
            garbageInfo.Barcode, garbageInfo.GarbageTypes);
    }

    /// <summary>
    /// Обновить информацию о товаре
    /// </summary>
    public async Task EditGarbageInformationAsync(GarbageInformation info)
    {
        var dbInfo = await _dbContext.Garbage.Include(x=>x.GarbageTypes).FirstOrDefaultAsync(x => x.Barcode == info.Barcode);
        if (dbInfo != null)
        {
            var garbageTypes = await _dbContext.GarbageType
                .Where(x => info.GarbageTypes.Select(x => x.Id).Contains(x.Id)).ToListAsync();

            if (garbageTypes.Count != info.GarbageTypes.Count())
            {
                throw new ApplicationException("Категории отходов указаны неверно!");
            }

            dbInfo.Picture = info.ImagePath;
            dbInfo.Name = info.Name;
            dbInfo.Description = info.Description;
            dbInfo.Barcode = info.Barcode;
            dbInfo.GarbageTypes.Clear();
            dbInfo.GarbageTypes.AddRange(garbageTypes);
            
            await _dbContext.SaveChangesAsync();

            return;
        }

        throw new ApplicationException("Информация о товаре не обнаружена, редактирование невозможно!");
    }

    public async Task<List<BLL.DTO.GarbageType>> GetGarbageTypesAsync()
    {
        
        var garbageTypes =  await _dbContext.GarbageType.Select(x => new BLL.DTO.GarbageType(x.Id, x.Name)).ToListAsync();

        if (garbageTypes.Any())
        {
            return garbageTypes;
        }

        throw new ApplicationException("Не удалось получить информацию о категориях отходов!");
    }

    public async Task<List<GarbageInformation>> GetGarbagesScanedByUserAsync(long? userId)
    {
        if (!userId.HasValue)
        {
            throw new ApplicationException("Не удалось получить информацию о пользователе, отправившем запрос!");
        }

        var scanedGarbages = await _dbContext.UserGarbage
            .Where(x => x.User.Id == userId)
            .OrderByDescending(x => x.ScannedDateTime)
            .Select(x => new GarbageInformation(x.Garbage.Picture, x.Garbage.Name, x.Garbage.Description,
                x.Garbage.Barcode, x.Garbage.GarbageTypes))
            .ToListAsync();

        return scanedGarbages.DistinctBy(x => x.Barcode).ToList();
    }


    public async Task SaveGarbageFromApiToUserHistory(UserGarbageHistorySaveData data)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(x=>x.Id == data.UserId);

        if (user == null)
        {
            throw new ApplicationException("Не удалось получить данные пользователя, выполнившего запрос!");
        }

        _dbContext.UserGarbageFromApi.Add(new UserGarbageFromApi()
        {
            User = user,
            Barcode = data.Barcode
        });

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<string>> GetGarbageFromApiUserHistory(long? userId)
    {
        if (userId == null)
        {
            throw new ApplicationException("Не удалось получить данные пользователя, выполнившего запрос!");
        }

        return await _dbContext.UserGarbageFromApi
            .Where(x => x.User.Id == userId)
            .Select(x => x.Barcode)
            .ToListAsync();
    }

    private async Task<bool> IsBarcodeAlreadyExistAsync(string barcode)
    {
        var garbage =  await _dbContext.Garbage.Where(x => x.Barcode == barcode).FirstOrDefaultAsync();
        return garbage != null;
    }
}