using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class SessionExtensions
    {
        public enum SessionNames { Comment, Request, Payment /*CommentList, RequestList, PaymentList*/, Email }
        public static void SetObject(this ISession session, string key, object value)
        {
            //var simpleJsonString = JsonSerializer.Create();
            //using (JsonWriter jw = new JsonTextWriter)
            //{
            //    var newstring = simpleJsonString.Serialize(jw, value);
            //}

            //List<string> longJson = new List<string>();
            //foreach (var val in value)
            //{

            //}

            var jsonstring = JsonConvert.SerializeObject(value,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                        });
            session.SetString(key, jsonstring);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
