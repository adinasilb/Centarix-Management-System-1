﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class TempRequestExtensions
    {
        public static void SerializeJson(this TempRequestJson jsonRequest, object value)
        {
            var jsonstring = JsonConvert.SerializeObject(value,
                       Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });
            jsonRequest.RequestJson = jsonstring;
        }

        public static T DeserializeJson<T>(this TempRequestJson jsonRequest)
        {
            var value = jsonRequest.RequestJson;
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

    }
}
