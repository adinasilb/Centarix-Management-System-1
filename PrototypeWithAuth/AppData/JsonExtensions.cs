using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Helpers;

namespace PrototypeWithAuth.AppData
{
    public static class JsonExtensions
    {
        public static void SerializeViewModel(this TempJson tempJson, object value)
         {
            //var jsonstring = JsonConvert.SerializeObject(value,
            //           Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
            //           {
            //               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //           });
            var jsonstring = JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            tempJson.Json = jsonstring;
        }

        public static T DeserializeJson<T>(this TempJson tempJson)
        {
            var value = tempJson.Json;
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            //dynamic data = Jso
        }

    }
}
