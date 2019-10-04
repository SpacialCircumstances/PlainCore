﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Veldrid;

namespace PlainCore.Content
{
    public class ContentLoader: IGraphicsContext
    {
        private const string ASSET_LOADER_PROPERTY_NAME = "loader";

        private readonly IDictionary<string, object> loadedAssets = new Dictionary<string, object>();
        private readonly IDictionary<string, IAssetLoader> assetLoaders = new Dictionary<string, IAssetLoader>();
        private readonly JsonElement contentManifest;
        private readonly IGraphicsContext graphicsContext;

        public ContentLoader(IGraphicsContext graphicsContext, JsonElement contentManifest, string rootDirectory)
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

            this.graphicsContext = graphicsContext;
            this.contentManifest = contentManifest.Clone();
            RootDirectory = dir;
            LoadDefaultAssetLoaders();
        }

        protected virtual void LoadDefaultAssetLoaders()
        {
            var texture2DLoader = new Texture2DLoader();
            RegisterAssetLoader("Texture", texture2DLoader);
            RegisterAssetLoader("Texture2D", texture2DLoader);
            RegisterAssetLoader(texture2DLoader);
        }

        public string RootDirectory { get; }

        public GraphicsDevice Device => graphicsContext.Device;

        public ResourceFactory Factory => graphicsContext.Factory;

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
            Type assetType = typeof(T);
            if (contentManifest.TryGetProperty(name, out var assetElement))
            {
                var assetLoaderName = assetElement.GetProperty(ASSET_LOADER_PROPERTY_NAME).GetString();
                var assetLoader = GetAssetLoaderByName(assetLoaderName);
                if (assetLoader.IsSupported(assetType))
                {
                    return (T)assetLoader.Load(this, assetElement, assetType);
                }
                else
                {
                    throw new NotSupportedException($"Error loading asset {name}: Asset loader {assetLoaderName} does not support loading {assetType.Name}");
                }
            }
            else
            {
                var assetLoader = GetAssetLoaderForType(assetType) ?? throw new NotSupportedException($"Error loading asset {name}: No asset loader specified and no compatible asset loader registered for ${assetType.Name}");
                //We do not need to check for support here because GetAssetLoaderForType does that
                return (T)assetLoader.Load(this, assetElement, assetType);
            }
        }

        public IAssetLoader GetAssetLoaderForType(Type type)
        {
            return assetLoaders.First(loader => loader.Value.IsSupported(type)).Value;
        }

        public IAssetLoader GetAssetLoaderByName(string name)
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
