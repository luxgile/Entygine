using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace Entygine.Rendering
{
    public class Shader
    {
        public int handle;
        public string vertexPath = "";
        public string fragmentPath = "";

        public bool IsCompiled { get; private set; }

        public static int ShadersLoaded { get; private set; }

        public Shader(string vertexPath, string fragmentPath)
        {
            this.handle = 0;
            this.vertexPath = vertexPath;
            this.fragmentPath = fragmentPath;
        }

        public void CompileShader()
        {
            int vertexShader = CompileShader(ShaderType.VertexShader, vertexPath);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentPath);

            handle = Ogl.CreateProgram();
            Ogl.AttachShader(handle, vertexShader);
            Ogl.AttachShader(handle, fragmentShader);
            Ogl.LinkProgram(handle);

            string infoLog = Ogl.GetProgramInfoLog(handle);
            if (!string.IsNullOrEmpty(infoLog))
                Console.WriteLine(infoLog);

            Ogl.DetachShader(handle, vertexShader);
            Ogl.DetachShader(handle, fragmentShader);
            Ogl.DeleteShader(vertexShader);
            Ogl.DeleteShader(fragmentShader);

            IsCompiled = true;
            ShadersLoaded++;
        }
        public int GetAttributeLocation(string attribName)
        {
            return Ogl.GetAttribLocation(handle, attribName);
        }

        private int CompileShader(ShaderType type, string path)
        {
            string shaderSource;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                shaderSource = reader.ReadToEnd();

            int shader = Ogl.CreateShader(type);
            Ogl.ShaderSource(shader, shaderSource);

            Ogl.CompileShader(shader);
            string infoLog = Ogl.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(infoLog))
                Console.WriteLine(infoLog);

            return shader;
        }

        public bool IsValid => handle != 0;
    }
}
