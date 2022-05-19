using System.Security.Claims;
using BLL.DTO;
using BLL.Models;
using BLL.Models.Helpers;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scanner.Models;

namespace Scanner.Controllers;

[ApiController]
[Route("[controller]")]
public class GarbageController : Controller
{
    private string serverAddress = "http://192.168.1.67:5000";
    
    private readonly ImageHelper _ImageHelper;
    private readonly GarbageService _garbageService;

    public GarbageController(ImageHelper ImageHelper, GarbageService garbageService)
    {
        _ImageHelper = ImageHelper;
        _garbageService = garbageService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetInfoAsync(string barcode)
    {
        try
        {
            var garbageInfo = await _garbageService.GetInformationByBarcodeAsync(barcode);
            return new ApiResult(garbageInfo);
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
    [Route("GetGarbageInfoByAuthorizedUser")]
    [Authorize(Roles = "AuthorizedUser")]
    public async Task<IActionResult> GetInfoByAuthorizedUserAsync(string barcode)
    {
        try
        {
            var userId = HttpContext.User.GetUserId();
            var garbageInfo = await _garbageService.GetInformationByBarcodeAsync(barcode, userId);
            return new ApiResult(garbageInfo);
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
    [Route("GetGarbagesScanedByAuthorizedUser")]
    [Authorize(Roles = "AuthorizedUser")]
    public async Task<IActionResult> GetGarbagesScanedByAuthorizedUserAsync()
    {
        try
        {
            var userId = HttpContext.User.GetUserId();
            var garbageInfo = await _garbageService.GetGarbagesScanedByUserAsync(userId);
            return new ApiResult(garbageInfo);
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

    [HttpPost]
    [Route("AddGarbageInfo")]
    public async Task<IActionResult> AddGarbageInfoAsync([FromForm]AddGarbageInfoDto garbageInfoDto)
    {
        try
        {
            var imagePath = await _ImageHelper.SaveImageToLocalStorageAsync(garbageInfoDto.Image, serverAddress);
            var garbageInfo = new GarbageInformation(imagePath, garbageInfoDto.Name, garbageInfoDto.Description, garbageInfoDto.Barcode, garbageInfoDto.GarbageTypes);
            if (garbageInfo.Validate())
            {
                await _garbageService.AddGarbageInformationAsync(garbageInfo);
                return new ApiResult();
            }

            return new ApiResult(garbageInfo.ValidationErrorMessages);
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
    
    [HttpPost]
    [Route("EditGarbageInfo")]
    public async Task<IActionResult> EditGarbageInfoAsync([FromForm]AddGarbageInfoDto garbageInfoDto)
    {
        try
        {
            var imagePath = await _ImageHelper.SaveImageToLocalStorageAsync(garbageInfoDto.Image, serverAddress);
            var garbageInfo = new GarbageInformation(imagePath, garbageInfoDto.Name, garbageInfoDto.Description, garbageInfoDto.Barcode, garbageInfoDto.GarbageTypes);
            if (garbageInfo.Validate())
            {
                await _garbageService.EditGarbageInformationAsync(garbageInfo);
                return new ApiResult();
            }

            return new ApiResult(garbageInfo.ValidationErrorMessages);
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
    [Route("GetGarbageTypes")]
    public async Task<IActionResult> GetGarbageTypesAsync()
    {
        try
        {
            return new ApiResult(await _garbageService.GetGarbageTypesAsync());
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

    [HttpPost]
    [Route("AddGarbageFromApiToUserHistory")]
    [Authorize(Roles = "AuthorizedUser")]
    public async Task<IActionResult> AddGarbageFromApiToUserHistoryAsync([FromForm] string barcode)
    {
        try
        {
            var userId = HttpContext.User.GetUserId();
            var addToHistoryData = new UserGarbageHistorySaveData(barcode, userId);

            if (addToHistoryData.Validate())
            {
                await _garbageService.SaveGarbageFromApiToUserHistory(addToHistoryData);
                return new ApiResult();
            }
            return new ApiResult(addToHistoryData.ValidationErrorMessages);
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
    [Route("GetGarbageFromApiUserHistory")]
    [Authorize(Roles = "AuthorizedUser")]
    public async Task<IActionResult> GetGarbageFromApiUserHistoryAsync()
    {
        try
        {
            var userId = HttpContext.User.GetUserId();
            return new ApiResult((object)await _garbageService.GetGarbageFromApiUserHistory(userId));
            
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