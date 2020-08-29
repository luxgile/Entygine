#version 330 core
out vec4 FragColor;

in vec2 texCoord;
in vec3 normal;
in vec4 lightPosSpace;

uniform sampler2D mainTexture;
uniform sampler2D shadowMap;

uniform vec3 ambientLight;
uniform vec3 directionalLight;
uniform vec3 directionalDir;

float ShadowCalculation(vec4 lightSpace)
{
    vec3 projCoords = lightSpace.xyz / lightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;

    if(projCoords.z > 1.0)
        return 0.0;

    float closestDepth = texture(shadowMap, projCoords.xy).r; 
    float currentDepth = projCoords.z;
    float bias = max(0.05 * (1.0 - dot(normal, directionalDir)), 0.005);
    float shadow = currentDepth - bias > closestDepth ? 1.0 : 0.0;
    return shadow;
}

void main()
{
    vec3 color = texture(mainTexture, texCoord).rgb;
    vec3 ambient = ambientLight;

    vec3 normalDir = normalize(normal);
    float angleDiff = max(dot(directionalDir, normalDir), 0.0f);
    vec3 diffuse = angleDiff * directionalLight * color;

    float shadow = ShadowCalculation(lightPosSpace);

    vec3 result = (ambient + (1 - shadow)) * diffuse;

    FragColor = vec4(result, 1.0);
}