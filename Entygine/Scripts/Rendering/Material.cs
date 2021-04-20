using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Entygine.DevTools;
using Entygine.Mathematics;

namespace Entygine.Rendering
{
    public class Material
    {
        public Shader shader;
        //TODO: Switch this to an array of textures
        private BaseTexture mainTexture;
        private BaseTexture depthMap;

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

            SetInt("mainTexture", 0);
            SetInt("shadowMap", 1);
        }

        public void UseMaterial()
        {
            if (Ogl.IsProgram(shader.handle))
                Ogl.UseProgram(shader.handle);
            else
                DevConsole.Log(LogType.Error, $"Shader handle is not considered a program ({shader.handle})");

            mainTexture?.UseTexture(TextureUnit.Texture0);
            depthMap?.UseTexture(TextureUnit.Texture1);
        }

        public void SetMainTexture(BaseTexture tex)
        {
            mainTexture = tex;
        }

        public void SetDepthMap(BaseTexture tex)
        {
            depthMap = tex;
        }

        public void FreeMaterial()
        {
            Ogl.UseProgram(0);
            mainTexture?.FreeTexture();
            depthMap?.FreeTexture();
        }

        public void SetMatrix(string name, Matrix4 matrix)
        {
            if (uniforms.TryGetValue(name, out int value))
            {
                Ogl.UseProgram(shader.handle);
                Ogl.UniformMatrix4(value, true, ref matrix);
            }
            else
            {
                DevConsole.Log(LogType.Warning, $"{name} wasn't found as matrix in shader.");
            }
        }

        public void SetInt(string name, int intValue)
        {
            if (uniforms.TryGetValue(name, out int value))
            {
                Ogl.UseProgram(shader.handle);
                Ogl.Uniform1(value, intValue);
            }
            else
            {
                DevConsole.Log(LogType.Warning, $"{name} wasn't found as int in shader.");
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
                DevConsole.Log(LogType.Warning, $"{name} wasn't found as Vector3 in shader.");
            }
        }

        public void SetColor(string name, Color01 color)
        {
            if (uniforms.TryGetValue(name, out int value))
            {
                Ogl.UseProgram(shader.handle);
                Ogl.Uniform4(value, color);
            }
        }

        public bool IsValid => shader.IsValid && (mainTexture?.IsValid ?? true);
    }
}
