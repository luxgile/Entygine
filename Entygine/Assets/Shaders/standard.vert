﻿#version 330
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 texCoord;

void main()
{
	texCoord = aTexCoord;
	gl_Position = vec4(aPosition, 1.0) * model * view * projection;
	//gl_Position = view * model * vec4(aPosition, 1.0);
	//gl_Position = vec4(aPosition, 1.0);
}