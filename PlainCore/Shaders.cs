namespace PlainCore
{
    public static class Shaders
    {
        private const string SPRITEBATCH_DEFAULT_VERTEX_SHADER = @"
            #version 450
            layout(location = 0) in vec3 position;
            layout(location = 1) in vec4 color;
            layout(location = 2) in vec2 texCoords;
            layout(set = 0, binding = 0) uniform WorldView {
                mat4 viewMatrix;
            }

            layout(location = 0) out vec4 fsColor;
            layout(location = 1) out vec2 fsTexCoords;
            
            void main() {
                gl_Position = viewMatrix * vec4(position, 1)
                fsColor = color;
                fsTexCoords = texCoords;
            }
        ";

        private const string SPRITEBATCH_DEFAULT_FRAGMENT_SHADER = @"
            #version 450
            layout(location = 0) in vec4 fsColor;
            layout(location = 1) in vec2 texCoords;

            layout(set = 1, binding = 0) uniform texture2D Texture;
            layout(set = 1, binding = 1) uniform sampler TextureSampler;

            layout(location = 0) out vec4 outColor;

            void main() {
                vec4 texColor = texture(sampler2D(Texture, TextureSampler), texCoords);
                outColor = texColor * fsColor;
            }
        ";
    }
}
