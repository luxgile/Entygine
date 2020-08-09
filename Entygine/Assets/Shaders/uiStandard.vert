#version 330
layout (location = 0) in vec4 aPosition;

uniform mat4 model;

out vec2 TexCoords;

void main()
{
	TexCoords = aPosition.zw;
	gl_Position = vec4(aPosition.xy, 0.0f, 1.0f) * model;
}