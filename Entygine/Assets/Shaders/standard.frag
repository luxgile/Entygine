#version 330 core
out vec4 FragColor;
in vec2 texCoord;
uniform sampler2D mainTexture;

void main()
{
    FragColor = texture(mainTexture, texCoord);
    //FragColor = vec4(0.5f, 0.5f, 0.2f, 1.0f);
}