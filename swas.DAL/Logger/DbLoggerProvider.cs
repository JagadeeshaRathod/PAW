using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Logger
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DbLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(categoryName, _serviceProvider.GetService<ApplicationDbContext>(), _serviceProvider.GetService<IHttpContextAccessor>());
        }

        public void Dispose() { }
    }



}
