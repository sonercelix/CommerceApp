using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace Web.Helper
{
    public class ApiHelper
    {
        IConfiguration AppSetting;
        public ApiHelper()
        {
            AppSetting = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();
        }
        public HttpClient Initial()
        {
            string apiUrl = AppSetting["ApiURL"];
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }
    }
}