using System;

namespace API.Errors;

public class ApiErrorResponse(int _statusCode,string _message,string? _details )
{
    public int StatusCode { get; set; }=_statusCode;
    public string Message { get; set; }=_message;
    public string? Details { get; set; }=_details;
}
