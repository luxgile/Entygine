#version 330 core
out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D mainTexture;

void main()
{
    FragColor = texture(mainTexture, TexCoords);
//	FragColor = vec4(TexCoords.xy, 1.0, 1.0);
}