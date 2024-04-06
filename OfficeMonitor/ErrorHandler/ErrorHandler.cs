using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OfficeMonitor.DTOs;
using OfficeMonitor.ErrorHandler.Errors;

namespace OfficeMonitor.ErrorHandler
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _delegate;

        public ErrorHandler(RequestDelegate @delegate)
        {
            _delegate = @delegate;
        }

        //todo ErrorDto doesnt put into response
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _delegate(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new 
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ForbidenException ex)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsJsonAsync(ex);
            }
        }
    }
}
