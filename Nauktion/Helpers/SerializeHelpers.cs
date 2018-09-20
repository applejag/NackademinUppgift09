using System.Collections.Generic;
using System.Linq;

namespace Nauktion.Helpers
{
    public static class SerializeHelpers
    {
        public static Dictionary<string, string> SerializeToDictionary(this object data)
        {
            return data.GetType().GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(data)?.ToString() ?? "");
        }

        public static string SerializeToJSArray(this IEnumerable<string> data)
        {
            return $"[{string.Join(", ", data.Select(s => s is null ? "null" : $"\"{s}\""))}]";
        }

        public static string SerializeToJSArray(this IEnumerable<int> data)
        {
            return $"[{string.Join(", ", data)}]";
        }
    }
}