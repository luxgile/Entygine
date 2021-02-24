#version 330
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTexCoord;

uniform mat4 projection;
uniform mat4 model;

out vec2 TexCoords;

void main()
{
	TexCoords = aTexCoord;
	gl_Position = vec4(aPosition, 0.0f, 1.0f) * model * projection;
}