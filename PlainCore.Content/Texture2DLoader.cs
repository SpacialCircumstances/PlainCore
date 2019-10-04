using System;
using System.IO;
using System.Text.Json;

namespace PlainCore.Content
{
    public class Texture2DLoader : IAssetLoader
    {
        public bool IsSupported(Type type)
        {
            return type == typeof(Texture2D);
        }

        public object Load(ContentLoader contentLoader, JsonElement parameters, Type type)
        {
            try 
            {
                var pathElementValue = parameters.GetProperty("path").GetString();
                var mipmap = parameters.GetPropertyOrDefault("mipmap", (el) => el.GetBoolean(), () => true);
                var srgb = parameters.GetPropertyOrDefault("srgb", (el) => el.GetBoolean(), () => false);
                var path = Path.Combine(contentLoader.RootDirectory, pathElementValue);
                return Texture2D.FromFile(contentLoader, path, mipmap, srgb);
            } 
            catch(Exception e)
            {
                throw new AssetLoadException($"Failed to load asset of type {type.Name}: {e.Message}", e);
            }
        }
    }
}
