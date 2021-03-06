using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace lookaroond
{
    public static class HttpContextExtensions
    {
        public static async Task<string> ReadRequestBodyAsString(this HttpContext ctx)
        {
            var request = ctx.Request;
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }

            request.Body.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            request.Body.Seek(0, SeekOrigin.Begin);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}