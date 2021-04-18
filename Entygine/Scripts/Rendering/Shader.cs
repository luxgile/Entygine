using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace Entygine.Rendering
{
    public class Shader
    {
        public string Name { get; private set; }
        public int handle;
        private string vertexProgram = "";
        private string fragmentProgram = "";

        public bool IsCompiled { get; private set; }

        public static int ShadersLoaded { get; private set; }

        public Shader(string name)
        {
            this.Name = name;
            this.handle = 0;
        }

        public static Shader CreateShaderWithPath(string vertexPath, string fragmentPath, string name)
        {
            Shader shader = new Shader(name);
            shader.handle = 0;

            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
                shader.vertexProgram = reader.ReadToEnd();

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
                shader.fragmentProgram = reader.ReadToEnd();

            return shader;
        }

        public static Shader CreateShaderWithProgram(string vertex, string frag, string name)
        {
            Shader shader = new Shader(name)
            {
                handle = 0,
                vertexProgram = vertex,
                fragmentProgram = frag
            };
            return shader;
        }

        public void CompileShader()
        {
            int vertexShader = CompileShader(ShaderType.VertexShader, vertexProgram);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentProgram);

            handle = Ogl.CreateProgram(Name);
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
        public int GetUniformLocation(string uniform)
        {
            return Ogl.GetUniformLocation(handle, uniform);
        }

        private int CompileShader(ShaderType type, string shaderSource)
        {
            int shader = Ogl.CreateShader(type, $" {type} - {Name}");
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
