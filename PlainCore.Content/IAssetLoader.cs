using System;
using System.Text.Json;

namespace PlainCore.Content
{
    public interface IAssetLoader
    {
        bool IsSupported(Type type);
        object Load(JsonElement parameters, Type type, string rootDirectory);
    }
}
