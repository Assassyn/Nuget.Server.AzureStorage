namespace Nuget.Server.AzureStorage
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    internal static class ObjectExtension
    {
        public static string ToJson(this object item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static T FromJson<T>(this string item)
        {
            return JsonConvert.DeserializeObject<T>(item);
        }

        public static bool ToBool(this string item)
        {
            return bool.Parse(item);
        }
    }
}
