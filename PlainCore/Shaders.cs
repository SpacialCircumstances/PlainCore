using System.Text;
using Veldrid;

namespace PlainCore
{
    public static class Shaders
    {
        public const string SPRITEBATCH_DEFAULT_VERTEX_SHADER = @"
            #version 450
            layout(location = 0) in vec3 Position;
            layout(location = 1) in vec4 Color;
            layout(location = 2) in vec2 TextureCoordinates;
            layout(set = 0, binding = 0) uniform WorldView {
                mat4 viewMatrix;
            };

            layout(location = 0) out vec4 fsColor;
            layout(location = 1) out vec2 fsTexCoords;
            
            void main() {
                gl_Position = viewMatrix * vec4(Position, 1);
                fsColor = Color;
                fsTexCoords = TextureCoordinates;
            }
        ";

        public const string SPRITEBATCH_DEFAULT_FRAGMENT_SHADER = @"
            #version 450
            layout(location = 0) in vec4 fsColor;
            layout(location = 1) in vec2 fsTexCoords;

            layout(set = 1, binding = 0) uniform texture2D Texture;
            layout(set = 1, binding = 1) uniform sampler TextureSampler;

            layout(location = 0) out vec4 outColor;

            void main() {
                vec4 texColor = texture(sampler2D(Texture, TextureSampler), fsTexCoords);
                outColor = texColor * fsColor;
            }
        ";

        public static readonly ShaderDescription SpritebatchDefaultVertexShader = new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(SPRITEBATCH_DEFAULT_VERTEX_SHADER), "main");

        public static readonly ShaderDescription SpritebatchDefaultFragmentShader = new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(SPRITEBATCH_DEFAULT_FRAGMENT_SHADER), "main");
    }
}
