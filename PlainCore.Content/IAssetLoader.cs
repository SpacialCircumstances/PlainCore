using System;
using System.Text.Json;

namespace PlainCore.Content
{
    public interface IAssetLoader
    {
        bool IsSupported(Type type);
        object Load(ContentLoader contentLoader, JsonElement parameters, Type type);
    }
}
