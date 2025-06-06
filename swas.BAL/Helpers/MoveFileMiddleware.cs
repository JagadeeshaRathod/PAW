using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Helpers
{
    public class MoveFileMiddleware
    {
        private readonly RequestDelegate _next;

        public MoveFileMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                MoveFileProgram.MoveFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Middleware Error: " + ex.Message);
            }

            await _next(context); // Continue processing the request
        }
    }
}
