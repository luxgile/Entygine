#version 330 core
out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D mainTexture;
uniform vec4 color;

void main()
{
    FragColor = texture(mainTexture, TexCoords) * color;
//	FragColor = vec4(TexCoords.xy, 1.0, 1.0);
}