#version 330 core
out vec4 FragColor;
in vec2 texCoord;
in vec3 normal;

uniform sampler2D mainTexture;
uniform vec3 ambientLight;
uniform vec3 directionalLight;
uniform vec3 directionalDir;

void main()
{
    vec4 col = texture(mainTexture, texCoord);

    float angleDiff = max(dot(directionalDir, normal), 0.0f);
    vec3 diffuse = (ambientLight * col.rgb) + directionalLight * col.rgb * angleDiff;

    FragColor = vec4(diffuse, col.a);
}