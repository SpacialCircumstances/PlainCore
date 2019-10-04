using System;
using System.Text.Json;

namespace PlainCore.Content
{
    public static class JsonExtensions
    {
        public static JsonElement GetPropertyOrThrow(this JsonElement element, string propertyName, Func<Exception> except)
        {
            if (element.TryGetProperty(propertyName, out var propElement))
            {
                return propElement;
            }
            else
            {
                throw except();
            }
        }

        public static T GetPropertyOrDefault<T>(this JsonElement element, string propertyName, Func<JsonElement, T> success, Func<T> def)
        {
            if (element.TryGetProperty(propertyName, out var propElement))
            {
                return success(propElement);
            }
            else
            {
                return def();
            }
        }

        public static string GetStringOrDefault(this JsonElement element, string def)
        {
            if (element.ValueKind == JsonValueKind.String)
            {
                return element.GetString();
            } 
            else
            {
                return def;
            }
        }

        public static bool GetBoolOrDefault(this JsonElement element, bool def)
        {
            if (element.ValueKind == JsonValueKind.False)
            {
                return false;
            }
            else if (element.ValueKind == JsonValueKind.True)
            {
                return true;
            }
            else
            {
                return def;
            }
        }
    }
}
