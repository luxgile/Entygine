using OpenTK.Graphics.ES30;
using System;
using System.IO;
using System.Text;

namespace Entygine
{
    public class Shader
    {
        public int handle;
        public string vertexPath = "";
        public string fragmentPath = "";

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

            handle = GL.CreateProgram();
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);
            GL.LinkProgram(handle);

            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }
        public int GetAttributeLocation(string attribName)
        {
            return GL.GetAttribLocation(handle, attribName);
        }

        private int CompileShader(ShaderType type, string path)
        {
            string shaderSource;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                shaderSource = reader.ReadToEnd();

            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, shaderSource);

            GL.CompileShader(shader);
            string infoLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(infoLog))
                Console.WriteLine(infoLog);

            return shader;
        }

        public bool IsValid => handle != 0;
    }
}
