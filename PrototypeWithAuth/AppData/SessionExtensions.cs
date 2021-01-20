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
        public enum SessionNames { Comment, Request, Payment /*CommentList, RequestList, PaymentList*/ }
        public static void SetObject(this ISession session, string key, object value)
        {
            var jsonstring = JsonConvert.SerializeObject(value,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
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
