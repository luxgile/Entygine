﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class Material
    {
        public Shader shader;
        public Texture mainTexture;

        private Dictionary<string, int> uniforms = new Dictionary<string, int>();

        public Material(Shader shader, Texture mainTexture)
        {
            this.shader = shader ?? throw new ArgumentNullException(nameof(shader));
            this.mainTexture = mainTexture;
        }

        public Material(Material materialResource)
        {
            shader = materialResource.shader;
            mainTexture = materialResource.mainTexture;

            uniforms = new Dictionary<string, int>(materialResource.uniforms);
        }

        public void LoadMaterial()
        {
            if (!shader.IsValid)
                shader.CompileShader();

            GL.GetProgram(shader.handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            for (int i = 0; i < uniformCount; i++)
            {
                string key = GL.GetActiveUniform(shader.handle, i, out _, out _);
                int location = GL.GetUniformLocation(shader.handle, key);
                if (uniforms.ContainsKey(key))
                    uniforms[key] = location;
                else
                    uniforms.Add(key, location);
            }
        }

        public void UseMaterial()
        {
            GL.UseProgram(shader.handle);

            mainTexture.UseTexture();
        }

        public void SetMatrix(string name, Matrix4 matrix)
        {
            if (uniforms.TryGetValue(name, out int value))
            {
                GL.UseProgram(shader.handle);
                GL.UniformMatrix4(value, true, ref matrix);
            }
            else
            {
                Console.WriteLine($"Key '{name}' not found in shader.");
            }
        }

        public bool IsValid => shader.IsValid && mainTexture.IsValid;
    }
}