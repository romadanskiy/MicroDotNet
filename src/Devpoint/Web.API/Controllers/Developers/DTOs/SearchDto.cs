using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers.Developers.DTOs;

public class SearchDto
{
    [FromQuery(Name = "search")]
    public string Search { get; set; } = "";
    [FromQuery(Name = "take")]
    public int Take { get; set; } = 3;
    [FromQuery(Name = "skip")]
    public int Skip { get; set; } = 0;
}