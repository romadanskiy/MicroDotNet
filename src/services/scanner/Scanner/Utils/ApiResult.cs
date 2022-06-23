using Microsoft.AspNetCore.Mvc;

namespace Scanner;

public class ApiResult : JsonResult
{
    public ApiResult() : this(new ApiResultData())
    {
    }

    public ApiResult(List<string> messages) : this(new ApiResultData(messages))
    {
    }

    public ApiResult(object data) : this(new ApiResultData(data))
    {
    }

    public ApiResult(Exception exception) : this(new ApiResultData(new List<string> { exception.Message }))
    {
    }

    private ApiResult(ApiResultData data) : base(data)
    {
        ContentType = new System.Net.Mime.ContentType("application/json; charset=utf-8").ToString();
    }
}