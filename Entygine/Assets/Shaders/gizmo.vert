#version 330
layout (location = 0) in vec3 aPosition;

uniform mat4 view;
uniform mat4 projection;

void main()
{
	vec4 localPos = vec4(aPosition, 1.0);
	gl_Position = localPos * view * projection;
}