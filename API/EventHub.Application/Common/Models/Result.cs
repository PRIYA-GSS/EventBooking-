namespace EventHub.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

   
}
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public T? Data {get;set;}
   
}
