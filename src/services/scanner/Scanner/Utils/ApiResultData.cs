using Microsoft.AspNetCore.Mvc;

namespace Scanner;

public class ApiResultData
{
    public bool Success { get; private set; }
    
    public List<string> Messages { get; private set; }
    
    public object Data { get; private set; }

    public ApiResultData(List<string> messages)
    {
        Success = false;
        Messages = messages;
    }
    
    public ApiResultData(object data)
    {
        Success = true;
        Data = data;
    }

    public ApiResultData()
    {
        Success = true;
    }
}