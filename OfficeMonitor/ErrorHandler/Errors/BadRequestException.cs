namespace OfficeMonitor.ErrorHandler.Errors
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
