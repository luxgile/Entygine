using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Entygine.DevTools;

namespace Entygine.Rendering
{
    public class Material
    {
        public Shader shader;
        //TODO: Switch this to an array of textures
        public BaseTexture mainTexture;

        private Dictionary<string, int> uniforms = new Dictionary<string, int>();

        public Material(Shader shader, BaseTexture mainTexture)
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

            Ogl.GetProgram(shader.handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            for (int i = 0; i < uniformCount; i++)
            {
                string key = Ogl.GetActiveUniform(shader.handle, i, out _, out _);
                int location = Ogl.GetUniformLocation(shader.handle, key);
                if (uniforms.ContainsKey(key))
                    uniforms[key] = location;
                else
                    uniforms.Add(key, location);
            }
        }

        public void UseMaterial()
        {
            if (Ogl.IsProgram(shader.handle))
                Ogl.UseProgram(shader.handle);
            else
                DevConsole.Log($"Shader handle is not considered a program ({shader.handle})");

            mainTexture.UseTexture(TextureUnit.Texture0);
        }

        public void FreeMaterial()
        {
            Ogl.UseProgram(0);
            mainTexture.FreeTexture();
        }

        public void SetMatrix(string name, Matrix4 matrix)
        {
            if (uniforms.TryGetValue(name, out int value))
            {
                Ogl.UseProgram(shader.handle);
                Ogl.UniformMatrix4(value, true, ref matrix);
            }
        }

        public void SetVector3(string name, Vector3 vector)
        {
            if (uniforms.TryGetValue(name, out int value))
            {
                Ogl.UseProgram(shader.handle);
                Ogl.Uniform3(value, vector);
            }
            else
            {
                Console.WriteLine($"Key '{name}' not found in shader.");
            }
        }

        public bool IsValid => shader.IsValid && mainTexture.IsValid;
    }
}
