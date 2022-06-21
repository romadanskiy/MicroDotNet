using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Web.API.Controllers.Storage;

[ApiController]
[Route("storage")]
public class StorageController : Controller
{
    [HttpPost("upload"), DisableRequestSizeLimit]
    public async Task<IActionResult> Upload()
    {
        try
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            
            if (file.Length > 0)
            {
                var fileName = await SaveFile(file);
                return Ok(fileName);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }
    
    [HttpPost("upload-md"), DisableRequestSizeLimit]
    public async Task<IActionResult> UploadMarkdown()
    {
        try
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            
            var initialFileName = file.FileName;
            if (file.Length > 0)
            {
                var fileName = await SaveFile(file);

                var response = new
                {
                    msg = fileName,
                    code = 0,
                    data = new
                    {
                        succMap = new Dictionary<string, string>()
                        {
                            {initialFileName, fileName},
                        }
                    }
                };

                return Json(response);
            }
            else
            {
                var response = new
                {
                    msg = "File is empty or cannot be parsed",
                    code = 400,
                    data = new
                    {
                        errFiles = new [] { initialFileName },
                    }
                };

                return Json(response);
            }
        }
        catch (Exception ex)
        {
            var response = new
            {
                msg = "Internal server error",
                code = 500
            };

            return Json(response);
        }
    }

    async Task<string> SaveFile(IFormFile file)
    {
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

        var ext = Path.GetExtension(ContentDispositionHeaderValue
            .Parse(file.ContentDisposition).FileName.ToString()
            .Trim('"'));
        var fileName = RandomizeRegister(
            DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper() + Path.GetRandomFileName()
        ) + ext;
        var fullPath = Path.Combine(pathToSave, fileName);
        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }

    string RandomizeRegister(string s)
    {
        var sb = new StringBuilder();
        var random = new Random(Guid.NewGuid().GetHashCode());
        for (int i = 0; i < s.Length; i++)
        {
            var ch = s[i].ToString();
            if (ch == ".")
                ch = ((char)random.Next(97, 123)).ToString();
            if (random.Next(0, 2) == 0)
                ch = ch.ToLower();
            else ch = ch.ToUpper();
            sb.Append(ch);
        }

        return sb.ToString();
    }
}