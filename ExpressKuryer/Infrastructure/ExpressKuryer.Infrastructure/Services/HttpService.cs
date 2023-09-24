using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Infrastructure.Services
{
    public class HttpService
    {
        static IHttpContextAccessor _httpContextAccessor;
        static HttpService()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }


        public static string Url() => $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
        public static string ImageUrl() => "";

		public static string StorageUrl(string storagePath, string? fileName)
        {
            string storageUrl = null;
            if (fileName != null)
            {
                storageUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{storagePath}{fileName}";
            }
            else
            {
                storageUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{storagePath}";
            }
            return storageUrl;
        }

    }
}
