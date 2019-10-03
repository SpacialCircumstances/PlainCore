using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PlainCore.Content
{
    public class ContentLoader
    {
        private readonly IDictionary<string, object> loadedAssets = new Dictionary<string, object>();
        private readonly JsonElement contentManifest;

        public ContentLoader(JsonElement contentManifest, string rootDirectory)
        {
            if (contentManifest.ValueKind != JsonValueKind.Object)
            {
                throw new ArgumentException(nameof(contentManifest), "Content manifest must be a JSON object");
            }

            var dir = Path.GetFullPath(rootDirectory);
            if (!Directory.Exists(dir))
            {
                throw new ArgumentException(nameof(rootDirectory), "Root directory must exist");
            }

            this.contentManifest = contentManifest;
            RootDirectory = dir;
        }

        public string RootDirectory { get; }

        public T Load<T>(string name)
        {
            if (loadedAssets.TryGetValue(name, out var loadedAsset))
            {
                return (T)loadedAsset;
            }
            else
            {
                var asset = LoadAsset(name);
                loadedAssets.Add(name, asset);
                return (T)asset;
            }
        }

        protected object LoadAsset(string name)
        {
            //TODO
            return null;
        }
    }
}
