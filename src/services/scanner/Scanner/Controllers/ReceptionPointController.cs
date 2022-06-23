using BLL.DTO;
using BLL.Models;
using BLL.Models.Types;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scanner.Models;

namespace Scanner.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceptionPointController: Controller
{
    private readonly ReceptionPointService _receptionPointService;
    
    public ReceptionPointController(ReceptionPointService receptionPointService)
    {
        _receptionPointService = receptionPointService;
    }
    
    [HttpPost]
    [Route("AddReceptionPoint")]
    public async Task<IActionResult> AddReceptionPointAsync([FromForm] ReceptionPointDto receptionPointDto)
    {
        try
        {
            var receptionPointAddress = new Address(receptionPointDto.Address, receptionPointDto.Longitude,
                receptionPointDto.Latitude);
            var garbageTypes = receptionPointDto.GarbageTypeIds?.Select(x => new GarbageType(x)).ToList();
            
            var receptionPoint = new ReceptionPoint(receptionPointDto.Name, receptionPointDto.Description,
                receptionPointAddress, garbageTypes);

            if (receptionPoint.Validate())
            {
                await _receptionPointService.AddReceptionPointAsync(receptionPoint);
                return new ApiResult();
            }

            return new ApiResult(receptionPoint.ValidationErrorMessages);
        }
        catch (ApplicationException ex)
        {
            return new ApiResult(ex);
        }
        catch
        {
            return new ApiResult(new List<string> { "Не удалось выполнить запрос!" });
        }
    }
    
    [HttpGet]
    [Route("GetReceptionPoints")]
    public async Task<IActionResult> GetReceptionPointsAsync([FromQuery] IEnumerable<long> garbageTypeIds)
    {
        try
        {
            var receptionPoints = await _receptionPointService.GetReceptionPointsAsync(garbageTypeIds);
            return new ApiResult(receptionPoints);
        }
        catch (ApplicationException ex)
        {
            return new ApiResult(ex);
        }
        catch
        {
            return new ApiResult(new List<string> { "Не удалось выполнить запрос!" });
        }
    }
}