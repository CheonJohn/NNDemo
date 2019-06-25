using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace NN.Models
{
    public class DB
    {
        public static string ConnectionString
        {
            get
            {
                var configuration = GetConfiguration();
                return configuration.GetSection("Data").GetSection("AzureSqlServerConnectionString").Value;
            }
        }
        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }



    }
}