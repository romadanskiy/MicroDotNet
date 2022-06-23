using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BLL.Models.Helpers;

public class ImageHelper
{
    private const string imageFolder = "images";
    private static string[] imageExtensions = { ".jpeg", ".png", ".jpg"};
    private IHostingEnvironment _hostingEnvironment;

    public ImageHelper(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string> SaveImageToLocalStorageAsync(IFormFile file, string serverAddress)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }
        
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension))
        {
            extension = imageExtensions[0];
        }
        else if (!imageExtensions.Contains(extension.ToLower()))
        {
            throw new ApplicationException("Изображение имеет недопустимое расширение! Допустимые: \".jpeg\", \".png\", \".jpg\"");
        }

        var fileName = GetFileName(extension);
        var webPath = Path.Combine( "http://" ,
            serverAddress, imageFolder,   
            fileName); 
        webPath = webPath.Replace(@"\", "/");
        
        var localPath = Path.Combine(  
            _hostingEnvironment.WebRootPath, imageFolder,   
            fileName); 
        
        using (var stream = new FileStream(localPath, FileMode.Create))  
        {  
            await file.CopyToAsync(stream);  
        }

        return webPath;
    }

    private string GetFileName(string extension)
    {
        var rnd = new Random();
        return $"{DateTime.Now.ToString("yyyy-M-d")}-{DateTime.Now.ToString("HH-mm-ss")}-{rnd.Next(10000)}{extension}";
    }
}