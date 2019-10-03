using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PlainCore.Content
{
    public class ContentLoader
    {
        private const string ASSET_LOADER_PROPERTY_NAME = "loader";

        private readonly IDictionary<string, object> loadedAssets = new Dictionary<string, object>();
        private readonly IDictionary<string, IAssetLoader> assetLoaders = new Dictionary<string, IAssetLoader>();
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

            this.contentManifest = contentManifest.Clone();
            RootDirectory = dir;
        }

        public string RootDirectory { get; }

        public void RegisterAssetLoader(IAssetLoader loader)
        {
            RegisterAssetLoader(loader.GetType().Name, loader);
        }

        public void RegisterAssetLoader(string name, IAssetLoader loader)
        {
            assetLoaders.Add(name, loader);
        }

        public T Load<T>(string name)
        {
            if (loadedAssets.TryGetValue(name, out var loadedAsset))
            {
                return (T)loadedAsset;
            }
            else
            {
                var asset = LoadAsset<T>(name);
                loadedAssets.Add(name, asset);
                return asset;
            }
        }

        protected T LoadAsset<T>(string name)
        {
            var assetElement = contentManifest.GetProperty(name);
            var assetLoaderName = assetElement.GetProperty(ASSET_LOADER_PROPERTY_NAME).GetString();
            var assetLoader = GetAssetLoader(assetLoaderName);
            Type assetType = typeof(T);
            if (assetLoader.IsSupported(assetType))
            {
                return (T)assetLoader.Load(assetElement, assetType, RootDirectory);
            }
            else
            {
                throw new NotSupportedException($"Error loading asset {name}: Asset loader {assetLoaderName} does not support loading {assetType.Name}");
            }
        }

        protected IAssetLoader GetAssetLoader(string name)
        {
            if (assetLoaders.TryGetValue(name, out var loader))
            {
                return loader;
            }
            else
            {
                Type loaderType = Type.GetType(name);
                if (loaderType != null && loaderType.GetInterfaces().Contains(typeof(IAssetLoader)))
                {
                    var loaderInstance = (IAssetLoader)loaderType.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>());
                    assetLoaders.Add(name, loaderInstance);
                    return loaderInstance;
                }
                else
                {
                    throw new NotSupportedException($"Asset loader {name} not registered, and loading via reflection failed");
                }
            }
        }
    }
}
