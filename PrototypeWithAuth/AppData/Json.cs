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
    public static class Json
    {
        public static string Serialize(object value)
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

            return jsonstring;
        }

        public static T Deserialize<T>(this string jsonString)
        {
            return jsonString == null ? default(T) : JsonConvert.DeserializeObject<T>(jsonString);
        }

    }
}
