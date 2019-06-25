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
    public static class Util
    {
        public static string toJson<T>(this List<T> data) where T : class 
        {
            return JsonConvert.SerializeObject(data,Formatting.Indented);
        }

       
        
    }
}