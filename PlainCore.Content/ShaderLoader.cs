using PlainCore.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Veldrid;

namespace PlainCore.Content
{
    public class ShaderLoader : IAssetLoader
    {
        private static IDictionary<string, VertexLayoutDescription> cachedNamedVertexLayouts;

        public bool IsSupported(Type type)
        {
            return type == typeof(ShaderSetDescription);
        }

        public object Load(ContentLoader contentLoader, JsonElement parameters, Type type)
        {
            try
            {
                var shaderProperty = parameters.GetProperty("shaders");
                var vertexLayoutProperty = parameters.GetProperty("vertexLayout");
                var vertexLayoutDescription = ParseVertexLayoutDescription(vertexLayoutProperty);
                var shaderDescriptions = ParseShaderDescriptions(shaderProperty);
                var shaders = shaderDescriptions.Select(sd => contentLoader.Factory.CreateShader(sd)).ToArray();
                return new ShaderSetDescription(new[] { vertexLayoutDescription }, shaders);
            }
            catch(Exception e)
            {
                throw new AssetLoadException($"Failed to load asset of type {type.Name}: {e.Message}", e);
            }
        }

        private IEnumerable<ShaderDescription> ParseShaderDescriptions(JsonElement json)
        {
            return null;
        }

        private VertexLayoutDescription ParseVertexLayoutDescription(JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.String)
            {
                var vertexName = json.GetString();
                if (cachedNamedVertexLayouts == null)
                {
                    LoadVertexTypes();
                }
                return cachedNamedVertexLayouts[vertexName];
            }
            else if (json.ValueKind == JsonValueKind.Array)
            {
                var elements = json.EnumerateArray().Select(element =>
                {
                    if (element.ValueKind == JsonValueKind.Object)
                    {
                        var name = element.GetProperty("name").GetString();
                        var dataType = element.GetProperty("type").GetString();
                        var format = (VertexElementFormat)Enum.Parse(typeof(VertexElementFormat), dataType, true);
                        return new VertexElementDescription(name, format, VertexElementSemantic.TextureCoordinate);
                    }
                    else
                    {
                        throw new InvalidOperationException("Elements of vertexLayout must be Objects");
                    }
                }).ToArray();
                return new VertexLayoutDescription(elements);
            }
            else
            {
                throw new InvalidOperationException("vertexLayout must be a String or an Array");
            }
        }

        private static void LoadVertexTypes()
        {
            cachedNamedVertexLayouts = Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies()
                .SelectMany(assemblyName =>
                {
                    var assembly = Assembly.Load(assemblyName);
                    return assembly
                        .DefinedTypes
                        .Where(ti =>
                            ti
                            .ImplementedInterfaces
                            .Contains(typeof(IVertex)))
                        .Select(m => new KeyValuePair<string, VertexLayoutDescription>(m.FullName, ((IVertex)assembly.CreateInstance(m.FullName)).VertexLayout));
                })
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
