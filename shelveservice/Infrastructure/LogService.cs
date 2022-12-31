namespace shelveservice.Infrastructure
{
    public class LogService<T> : ILogService<T>
    {
        private readonly ILogger<T> _logService;
        public LogService(ILoggerFactory loggerFactory)
        {
            this._logService = loggerFactory.CreateLogger<T>();
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            this._logService.LogError(exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            this._logService.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            this._logService.LogWarning(message, args);
        }
    }
}

