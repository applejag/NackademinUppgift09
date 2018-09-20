using System.Collections.Generic;
using System.Linq;

namespace Nauktion.Helpers
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, string> SerializeToDictionary(this object data)
        {
            return data.GetType().GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(data)?.ToString() ?? "");
        }
    }
}