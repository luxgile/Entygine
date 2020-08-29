#version 330
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightSpace;

out vec2 texCoord;
out vec3 normal;
out vec4 lightPosSpace;

void main()
{
	texCoord = aTexCoord;
	vec4 localPos = vec4(aPosition, 1.0) * model;
	lightPosSpace = localPos * lightSpace;
	gl_Position = localPos * view * projection;
	normal = transpose(inverse(mat3(model))) * aNormal;
}