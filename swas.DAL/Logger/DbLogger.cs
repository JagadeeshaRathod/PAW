using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace swas.DAL.Logger
{

    public class DbLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbLogger(string categoryName, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _categoryName = categoryName;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public async Task Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            //if (!IsEnabled(logLevel))
            //    return;       
            if (logLevel != LogLevel.Error)  // Log only if it's an error
                return;

            var message = formatter(state, exception);
            var ipAddress = GetIpAddress();
            // Capture the current thread ID and event name
            int threadId = Thread.CurrentThread.ManagedThreadId;  // Get the current thread ID
            string eventName = eventId.Name ?? "Unknown Event";

            var logEntry = new LogEntry
            {
                Message = message,
                LogLevel = logLevel.ToString(),
                ThreadId = threadId,
                EventId = eventId.Id,
                EventName = eventName,
                IpAddress = ipAddress,
                CreatedAt = DateTime.Now,
                ExceptionMessage = exception?.Message,
                ExceptionStackTrace = exception?.StackTrace,
                ExceptionSource = exception?.Source
            };

            await _context.Errors.AddAsync(logEntry);
            await _context.SaveChangesAsync();
        }

        // The correct signature for the Log method
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _ = Log(logLevel, eventId, state, exception, formatter); // Fire and forget
        }

        private string GetIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "192.168.10.19";
        }
    }



}
